using System.Collections;
using UnityEngine;

public class PlayerBlockGun : MonoBehaviour {

	public Player player;

	[Space]
	public GameObject blockPlacingEffect;
	public GameObject blockPlacingTrajectory;
	
	[Space]
	public float reloadTimeout;

	[Space]
	public GameObject[] blockPrefabs;


	const int blockDistFromGun = 2;


	bool blockLoaded = false;
	GameObject loadedBlockGameObject;
	Block loadedBlock;


	void Awake()
	{
	}


	void Update()
	{
		// check if gun-loaded block should not be placed in the world
		// because it is going to run into some world obstacle
		if (blockLoaded && player.gameManager.playerAlive)
		{
			// if current step collisional (made by rotation)
			// or next grid step up would be collisional, place as obstacle
			if (loadedBlock.IsGridPosCollisional(player.gameManager.worldGridManager) ||
				loadedBlock.IsOneGridStepThereCollisional(new Vector3(0, 1, 0), player.gameManager.worldGridManager))
			{
				PlaceLoadedBlockIntoTheWorld(Vector3.zero); // zero = current pos
				
				blockLoaded = false;
				ReloadAfterTimeout();
			}
		}
	}

	void LateUpdate()
	{
		// follow player position.y
		transform.position = new Vector3(transform.position.x, player.transform.position.y, 0);
	}




	public void PlayerAttemptToMoveLeft()
	{
		if (!loadedBlock) return;
		
		loadedBlock.RecalculateChildElementGridPositions();
		TryToShiftLoadedBlockLeft(); // try to move with checking bounds and collisions
	}
	public void PlayerAttemptToMoveRight()
	{
		if (!loadedBlock) return;
		
		loadedBlock.RecalculateChildElementGridPositions();
		TryToShiftLoadedBlockRight();
	}





	public void PlaceLoadedBlockIntoTheWorld(Vector3 gridPosition)
	{
		// place on gridded position in the world
		loadedBlock.PlaceIntoTheWorld(
			player.gameManager.worldGridManager.worldObjects, // parent
			gridPosition); // no specified position - place where it is now (aligned to grid)
		
		// register into world grid controller
		// so collisions and actions can be controlled
		player.gameManager.worldGridManager.RegisterBlockIntoWorldBlockList(loadedBlock);

		// effect of placing
		Destroy(
			Instantiate(
				blockPlacingEffect,
				loadedBlock.pivoter.transform.position,
				Quaternion.identity,
				transform) as GameObject, 3f);

		// fake effect of flying there with trail
		GameObject blockTrail;
		Destroy(blockTrail =
			Instantiate(blockPlacingTrajectory) as GameObject, 3f);
		blockTrail.GetComponent<BlockPlacingTrailController>().Init(
			player.transform.position + new Vector3(0, Design.gridSize * blockDistFromGun, 0),
			gridPosition * Design.gridSize + new Vector3(0, Design.gridSize * blockDistFromGun, 0));

		player.gameManager.soundManager.Play(SoundManager.soundId.blockPlaced);
	}




	void ReloadAfterTimeout()
	{
		StopAllCoroutines();
		StartCoroutine(ReloadAfterTimeoutCoroutine());
	}
	IEnumerator ReloadAfterTimeoutCoroutine()
	{
		yield return new WaitForSeconds(reloadTimeout);

		if (player.gameManager.playerAlive)
		{
			Reload();
		}
	}

	// choose random block, ghostify, place in front of player, get ready to fire it
	public void Reload()
	{
		// on reload always repositoin gun back to center - to the player
		transform.position =
			new Vector3(
				player.playerController.GetCurrentGridPosition().x * Design.gridSize,
				transform.position.y,
				0);
		

		loadedBlockGameObject = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)]) as GameObject;
		// reparent into player gun (to drag with)
		loadedBlockGameObject.transform.parent = transform;
		// position up to player ship
		float armedLocalPos = Design.gridSize * blockDistFromGun;
		loadedBlockGameObject.transform.localPosition = new Vector3(0, armedLocalPos, 0);
		
		loadedBlock = loadedBlockGameObject.GetComponent<Block>();
		if (loadedBlock.evenWidth)
		{
			// even width blocks has origin in the middle
			// need random half-shifted spawn
			if (Random.value > 0.5f)
				loadedBlockGameObject.transform.position =
					new Vector3(
						loadedBlockGameObject.transform.position.x - Design.gridSize,
						loadedBlockGameObject.transform.position.y,
						0);
		}
		loadedBlock.RotateRandom();
		loadedBlock.InitAndRegisterChildElements();

		// is it possible to spawn at this current place at all?
		if (loadedBlock.IsGridPosCollisional(player.gameManager.worldGridManager))
		{
			Destroy(loadedBlockGameObject);
			
			blockLoaded = false;
			ReloadAfterTimeout();
			return;
		}

		// ghostify to visualize functional difference
		loadedBlock.GhostifyElementRenderers();
		// and animate in
		loadedBlock.ZoomInChildElements();
		
		blockLoaded = true;
	}



	public void RotateLoadedBlockCW()
	{
		if (!blockLoaded || !player.gameManager.playerAlive) return;
		
		loadedBlock.RotateBlock(Quaternion.Euler(0, 0, 90f));
		
		// is it possible to rotate at this current place like that?
		if (loadedBlock.IsGridPosCollisional(player.gameManager.worldGridManager))
		{
			// player left or right
			if (player.playerController.GetCurrentGridPosition().x < 0)
				TryToShiftLoadedBlockRight();
			else
				TryToShiftLoadedBlockLeft();
		}
	}
	public void RotateLoadedBlockCCW()
	{
		if (!blockLoaded || !player.gameManager.playerAlive) return;
		
		loadedBlock.RotateBlock(Quaternion.Euler(0, 0, -90f));
		
		// is it possible to rotate at this current place like that?
		if (loadedBlock.IsGridPosCollisional(player.gameManager.worldGridManager))
		{
			// player left or right
			if (player.playerController.GetCurrentGridPosition().x < 0)
				TryToShiftLoadedBlockRight();
			else
				TryToShiftLoadedBlockLeft();
		}
	}

	public void TryToShiftLoadedBlockLeft()
	{
		if (!blockLoaded || !player.gameManager.playerAlive) return;
		
		if (!loadedBlock.IsOneGridStepThereCollisional(new Vector3(-1, 0, 0), player.gameManager.worldGridManager))
		{
			// able to move loaded block left
			loadedBlock.MoveBlockOneGridStep(new Vector3(-1, 0, 0));
		}
	}
	public void TryToShiftLoadedBlockRight()
	{
		if (!blockLoaded || !player.gameManager.playerAlive) return;

		if (!loadedBlock.IsOneGridStepThereCollisional(new Vector3(1, 0, 0), player.gameManager.worldGridManager))
		{
			// able to move loaded block right
			loadedBlock.MoveBlockOneGridStep(new Vector3(1, 0, 0));
		}
	}




	public void Fire()
	{
		if (!blockLoaded || !player.gameManager.playerAlive) return;


		// find position to land
		Vector3 landDelta =
			new Vector3(
				0,
				loadedBlock.UpmostLandingGridDistance(player.gameManager.worldGridManager),
				0);
		Vector3 destinationGridPos = loadedBlock.GetCurrentGridPosition() + landDelta;
			
		// no position to land found (no obstacle in way = infinity)
		if (landDelta == Vector3.zero)
		{
			// TODO sound of no firing available
			return;
		}


		// place where it belongs to land in the world
		PlaceLoadedBlockIntoTheWorld(destinationGridPos);

		// register that cannon fired a block
		player.gameManager.BlockCannonFired();

		// element renderers
		loadedBlock.SolidifyElementRenderers();
		
		blockLoaded = false;
		ReloadAfterTimeout();
	}






	// make loaded block disappear on game over
	// analogy to desintegration of player block ship, but no fx, just liberate elements
	public void FlyAwayWithGunLoadedBlockElements()
	{
		// no block loaded
		if (!blockLoaded) return;

		for (int i = 0; i < loadedBlock.childElements.Length; i++)
		{
			loadedBlock.childElements[i].FlyAway();
		}

	}
}