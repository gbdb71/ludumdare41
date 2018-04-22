using UnityEngine;

public class InputController : MonoBehaviour {

	public AppManager appManager;


	Vector3 touchPosBegin; // touch beginning
	Vector3 touchPosCurrent; // touch (drag) end

	float minDragDistance;

	void Awake()
	{
		minDragDistance = Screen.height * 5 / 100;
	}

	void Update()
	{
		// input handled always
		if (Input.GetKeyDown(KeyCode.Escape)) appManager.EscapeOrBackKeysPressed();



		// editor debug ingame input
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R)) appManager.RestartCurrentScene();
		if (Input.GetKeyDown(KeyCode.K)) appManager.gameManager.PlayerCollided();
		if (Input.GetKeyDown(KeyCode.Q)) appManager.gameManager.worldGridManager.ShiftEverythingBack();
#endif



			// player input when not in the game
			// and nothing more
			if (!appManager.gameManager.playerAlive)
		{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) appManager.RestartCurrentScene();
			
			return;
		}
		


		// keyboard
		if (Input.GetKeyDown(KeyCode.LeftArrow)) Left();
		if (Input.GetKeyDown(KeyCode.RightArrow)) Right();
		if (Input.GetKeyDown(KeyCode.UpArrow)) Up();
		if (Input.GetKeyDown(KeyCode.DownArrow)) Down();
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) Action();
		


		// touch
		if (Input.touchCount == 1) // single touch
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				touchPosBegin = touch.position;
				touchPosCurrent = touch.position;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				touchPosCurrent = touch.position;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				touchPosCurrent = touch.position;
				
				if (Mathf.Abs(touchPosCurrent.x - touchPosBegin.x) > minDragDistance ||
					Mathf.Abs(touchPosCurrent.y - touchPosBegin.y) > minDragDistance)
				{
					// drag
					// horizontal vs vertical
					if (Mathf.Abs(touchPosCurrent.x - touchPosBegin.x) > Mathf.Abs(touchPosCurrent.y - touchPosBegin.y))
					{
						if (touchPosCurrent.x > touchPosBegin.x)
							Right();
						else
							Left();
					}
					else
					{
						if (touchPosCurrent.y > touchPosBegin.y)
							// Up not up but toggle action (fire)
							//Up();
							Action();
						else
							Down();
					}
				}
				else
				{
					Action();
				}
			}
		}
	}



	void Left()
	{
		appManager.gameManager.player.playerController.MoveLeft();
		appManager.gameManager.soundManager.Play(SoundManager.soundId.action);
	}
	void Right()
	{
		appManager.gameManager.player.playerController.MoveRight();
		appManager.gameManager.soundManager.Play(SoundManager.soundId.action);
	}
	void Up()
	{
		appManager.gameManager.player.playerBlockGun.RotateLoadedBlockCCW();
		appManager.gameManager.soundManager.Play(SoundManager.soundId.action);
	}
	void Down()
	{
		appManager.gameManager.player.playerBlockGun.RotateLoadedBlockCW();
		appManager.gameManager.soundManager.Play(SoundManager.soundId.action);
	}
	void Action()
	{
		appManager.gameManager.player.playerBlockGun.Fire();
	}
}