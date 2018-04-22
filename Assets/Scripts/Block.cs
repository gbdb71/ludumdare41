using UnityEngine;

public class Block : MonoBehaviour {

	[Header("Randomly move one step left on arm")]
	public bool evenWidth;

	[Header("Gates are spawned at constant x pos")]
	public bool isGate;
	public bool isGateThatCanBeFlipped;

	[Space]
	public Material ghostMaterial;
	public Material defaultMaterial;

	[HideInInspector]
	public GameObject pivoter;

	[Header("Children Lookup Dynamic")]
	public Element[] childElements;

	bool blockInitialised = false;



	// runtime only
	Vector3 gridPosition;



	void Awake()
	{
		pivoter = transform.GetChild(0).gameObject;
	}



	void Start()
	{
		if (!blockInitialised) InitAndRegisterChildElements();
	}




	public void InitAndRegisterChildElements()
	{
		childElements = GetComponentsInChildren<Element>();
		blockInitialised = true;
	}




	public void RecalculateChildElementGridPositions()
	{
		for (int i = 0; i < childElements.Length; i ++)
		{
			childElements[i].CalculateGridPosition();
		}
	}





	// game mechanics of a block

	// check if all child elements can exist where they are in a grid
	public bool IsGridPosCollisional(WorldGridManager worldGridManager)
	{
		RecalculateChildElementGridPositions();
		
		for (int i = 0; i < childElements.Length; i++)
		{
			// is there something?
			if (worldGridManager.IsThisGridCellOccupied(childElements[i].GetGridPosition()))
				return true;
		}

		return false;
	}

	// check if all child elements can exist one grid step in some direction
	public bool IsOneGridStepThereCollisional(Vector3 gridDirThere, WorldGridManager worldGridManager)
	{
		RecalculateChildElementGridPositions();

		Vector3 checkedGridPosition;

		for (int i = 0; i < childElements.Length; i++)
		{
			// one grid step in specified direction
			checkedGridPosition =
				childElements[i].GetGridPosition() + gridDirThere;
			
			// is there something?
			if (worldGridManager.IsThisGridCellOccupied(checkedGridPosition))
				return true;

			// or is it out of boundaries?
			if (childElements[i].GetGridPosX() + gridDirThere.x < 0 - ((Design.gridWidth - 1) / 2) - 1 ||
				childElements[i].GetGridPosX() + gridDirThere.x > 0 + ((Design.gridWidth - 1) / 2) + 1)
				return true;

	}
		
		// all block child elements are in that one-step-up position without collision
		return false;
	}

	public int UpmostLandingGridDistance(WorldGridManager worldGridManager)
	{
		RecalculateChildElementGridPositions();

		Vector3 checkedGridPosition;

		for (int x = 0; x < Design.gridInfinityAhead; x++)
		{
			for (int i = 0; i < childElements.Length; i++)
			{
				// xth + 1 grid step up
				checkedGridPosition =
					childElements[i].GetGridPosition() + new Vector3(0, x + 1, 0);

				// is there something?
				if (worldGridManager.IsThisGridCellOccupied(checkedGridPosition))
				{
					return x;
				}
			}
		}
		
		return 0;
	}


	public void PlaceIntoTheWorld(Transform worldObjectParent, Vector3 placeOnGridPosition)
	{
		// no specified placing position
		// place this block on its current grid position
		if (placeOnGridPosition == Vector3.zero)
		{
			// round-up current position to the grid
			gridPosition = GetCurrentGridPosition();
			
			transform.position = gridPosition * Design.gridSize;
		}
		else
		{
			transform.position = placeOnGridPosition * Design.gridSize;
		}



		// elements should know their placed position now
		RecalculateChildElementGridPositions();

		// reparent block into the world/objects
		transform.parent = worldObjectParent;

		SolidifyElementRenderers();

		ZoomInChildElements();
	}


	// visualy block not being an obstacle
	// just in front of player ready to be fired
	public void GhostifyElementRenderers()
	{
		for (int i = 0; i < childElements.Length; i++)
		{
			childElements[i].GhostifyRenderer(ghostMaterial);
		}
	}

	public void SolidifyElementRenderers()
	{
		for (int i = 0; i < childElements.Length; i++)
		{
			childElements[i].GhostifyRenderer(defaultMaterial);
		}
	}

	public void ZoomInChildElements()
	{
		for (int i = 0; i < childElements.Length; i++)
		{
			childElements[i].ZoomIn();
		}
	}

	public void DeactivateWholeBlockAsPartOfCompletedLine()
	{
		for (int i = 0; i < childElements.Length; i++)
		{
			childElements[i].DeactivateAsPartOfCompletedLine();
		}
	}




	public void RotateRandom()
	{
		RotateBlock(Quaternion.Euler(0, 0, Random.Range(0,4) * 90));
	}

	public void RotateBlock(Quaternion rot)
	{
		//transform.rotation *= rot;
		pivoter.transform.rotation *= rot;
	}

	public void MoveBlockOneGridStep(Vector3 stepDirection)
	{
		transform.position += stepDirection * Design.gridSize;
	}



	public Vector3 GetCurrentGridPosition()
	{
		return new Vector3(
			Mathf.RoundToInt(transform.position.x / Design.gridSize),
			Mathf.RoundToInt(transform.position.y / Design.gridSize),
			0); // z is hopefully always 0 anyway
	}




}