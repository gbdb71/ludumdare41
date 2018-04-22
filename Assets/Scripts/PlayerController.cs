using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveLerpSpeed = 1f;

	[Space]
	public Block playerBlock;

	//Rigidbody2D rb;
	Player player;

	Vector3 gridPos;
	Vector3 desiredGridPos;
	Vector3 desiredWorldPos;


	void Awake()
	{
		//rb = GetComponent<Rigidbody2D>();
		player = GetComponent<Player>();
		
		UpdateCurrentGridPosition();
		desiredGridPos = gridPos;
		
		KeepMovingToDesiredPosition(); // permanent move coroutine
	}

	void Update()
	{
		UpdateCurrentGridPosition();
		UpdateChildElementGridPositions();
	}


	void UpdateCurrentGridPosition()
	{
		gridPos.x = Mathf.RoundToInt(transform.position.x / Design.gridSize);
		gridPos.y = Mathf.RoundToInt(transform.position.y / Design.gridSize);
		gridPos.z = 0;
	}
	void UpdateChildElementGridPositions()
	{
		playerBlock.RecalculateChildElementGridPositions();
	}



	// movement

	public void BoostSpeedFwd()
	{
		player.playerMoveForward.Boost();
	}

	public void MoveLeft()
	{
		desiredGridPos.x = gridPos.x - 1;
		CheckGridBounds();
		
		player.playerBlockGun.PlayerAttemptToMoveLeft();
	}
	public void MoveRight()
	{
		desiredGridPos.x = gridPos.x + 1;
		CheckGridBounds();
		
		player.playerBlockGun.PlayerAttemptToMoveRight();
	}
	void CheckGridBounds()
	{
		int clampedPosX = 
			(int)
			Mathf.Clamp(
				desiredGridPos.x,
				0 - ((Design.gridWidth - 1) / 2) - 1,
				0 + ((Design.gridWidth - 1) / 2) + 1);
		
		if (desiredGridPos.x > clampedPosX) AttemptToMoveOverRightBoundary();
		if (desiredGridPos.x < clampedPosX) AttemptToMoveOverLeftBoundary();
		
		desiredGridPos.x = clampedPosX;
	}
	void AttemptToMoveOverRightBoundary()
	{
		player.gameManager.soundManager.Play(SoundManager.soundId.actionDenied);
	}
	void AttemptToMoveOverLeftBoundary()
	{
		player.gameManager.soundManager.Play(SoundManager.soundId.actionDenied);
	}



	void KeepMovingToDesiredPosition()
	{
		StartCoroutine(MoveToDesiredPositionCorotutine());
	}
	IEnumerator MoveToDesiredPositionCorotutine()
	{
		while (true)
		{
			//Debug.Log("pos:" + gridPos + ", desired:" + desiredGridPos);
			
			desiredWorldPos.x = desiredGridPos.x * Design.gridSize;
			desiredWorldPos.y = transform.position.y; // dont try to do anything with y axis
			
			transform.position = 
				Vector3.Lerp(transform.position, desiredWorldPos, Time.deltaTime * moveLerpSpeed);
			
			yield return null;
		}
	}




	public Vector3 GetCurrentGridPosition()
	{
		return gridPos;
	}
}