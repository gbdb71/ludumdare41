using UnityEngine;
using System.Collections.Generic;

public class WorldGridManager : MonoBehaviour {

	public GameManager gameManager;

	[Header("Own Children")]
	public Transform worldObjects;

	[Space]
	public Block playerBlock;

	[Space]
	public List<Block> worldBlocks;




	void Update()
	{
		// no need to check anything when player already dead
		if (!gameManager.playerAlive) return;
		
		
		CheckForPlayerCollisions();
		
		CheckForLineCompletion();
	}



	void CheckForPlayerCollisions()
	{
		for (int i = 0; i < playerBlock.childElements.Length; i++)
		{
			foreach (Block worldBlock in worldBlocks)
			{
				for (int i3 = 0; i3 < worldBlock.childElements.Length; i3++)
				{
					if (playerBlock.childElements[i].GetGridPosition() ==
							worldBlock.childElements[i3].GetGridPosition() &&
						!worldBlock.childElements[i3].liberate)
					{
						// player collision with some block
						gameManager.PlayerCollided();
					}
				}
			}
		}
	}

	void CheckForLineCompletion()
	{
		float playerGridPosY =
			gameManager.player.playerController.GetCurrentGridPosition().y;

		bool lineComplete;

		// check line by line, ahead of player grid position
		for (float lineGridPosY = playerGridPosY;
			 lineGridPosY < playerGridPosY + Design.gridInfinityAhead;
			 lineGridPosY++)
		{
			lineComplete = true;
			
			// cell by cell in this line
			for (float cellGridPosX = (0 - (Design.gridWidth - 1) / 2) - 1;
				 cellGridPosX <= ((Design.gridWidth - 1) / 2) + 1;
				 cellGridPosX++)
			{
				// first unoccupied cell skips this whole line iteration
				if (!IsThisGridCellOccupied(new Vector3(cellGridPosX, lineGridPosY, 0)))
				{
					lineComplete = false;
					break;
				}
				//Debug.Log("line:" + lineGridPosY + " cell:" + cellGridPosX);
			}
			
			if (lineComplete)
			{
				//Debug.Log("line complete!");
				LineCompleteProcess(lineGridPosY);
				
				gameManager.LineFinished();
			}
		}
	}

	void LineCompleteProcess(float linePosY)
	{
		foreach (Block worldBlock in worldBlocks)
		{
			for (int i = 0; i < worldBlock.childElements.Length; i++)
			{
				if (linePosY == worldBlock.childElements[i].GetGridPosY() &&
					!worldBlock.childElements[i].liberate)
				{
					// element out!
					//worldBlock.childElements[i].DeactivateAsPartOfCompletedLine();

					// testing
					// whole block out! not just that element
					worldBlock.DeactivateWholeBlockAsPartOfCompletedLine();
				}
			}
		}
	}




	// registering newly placed block into the world block list
	public void RegisterBlockIntoWorldBlockList(Block newlyPlacedBlock)
	{
		worldBlocks.Add(newlyPlacedBlock);
	}





	// is this grid space occupied by something?
	// or outside of boundaries?
	public bool IsThisGridCellOccupied(Vector3 gridPos)
	{
		// check boundaries
		if (gridPos.x < 0 - (Design.gridWidth - 1) / 2 - 1 ||
			gridPos.x > 0 + (Design.gridWidth - 1) / 2 + 1)
			return true;
		
		// check elements of all registered blocks
		foreach (Block worldBlock in worldBlocks)
		{
			for (int i = 0; i < worldBlock.childElements.Length; i++)
			{
				if (gridPos == worldBlock.childElements[i].GetGridPosition() &&
					!worldBlock.childElements[i].liberate)
				{
					return true;
				}
			}
		}
		
		return false;
	}




	public void ShiftEverythingBack()
	{
		foreach (Block worldBlock in worldBlocks)
		{
			worldBlock.transform.position += Vector3.up * Design.gridSize;
			worldBlock.RecalculateChildElementGridPositions();
		}

	}
}