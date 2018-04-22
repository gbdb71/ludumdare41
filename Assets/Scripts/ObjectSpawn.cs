using UnityEngine;

public class ObjectSpawn : MonoBehaviour {

	public GameManager gameManager;
	public WorldGridManager worldGridManager;
	public PlayerController playerController;


	[Header("Obstacle Frequency")]
	public float spawnAheadPlayerDistance;
	public float firstObstacleAtDistance;
	public float obstacleFrequency;

	[Header("Obstacles to Spawn")]
	public GameObject[] obstacle;


	float nextObsctacleAt;

	float playerDistance;


	void Start()
	{
		UpdatePlayerDistance();

		nextObsctacleAt = playerDistance + firstObstacleAtDistance;
	}



	void Update()
	{
		UpdatePlayerDistance();
		
		if (playerDistance > nextObsctacleAt)
		{
			SpawnObstacle();
			nextObsctacleAt = playerDistance + obstacleFrequency;
		}
	}





	void UpdatePlayerDistance()
	{
		playerDistance = playerController.transform.position.y;
	}





	void SpawnObstacle()
	{
		// select obstacle/gate prefab
		int obstacleId = Random.Range(0, obstacle.Length);
		//Debug.Log("ObjectSpawn().SpawnObstacle().osbtacleId=" + obstacleId);


		// instantiate
		GameObject spawnedBlockGO =
			Instantiate(obstacle[obstacleId]) as GameObject;
		// and get block component
		Block spawnedBlock = spawnedBlockGO.GetComponent<Block>();

		// parent into and position in the world
		spawnedBlockGO.transform.parent = transform;
		// correct according to grid
		Vector3 spawnPosition;
		spawnPosition.y = Mathf.Floor(playerDistance + spawnAheadPlayerDistance);
		spawnPosition.z = 0;
		if (spawnedBlock.isGate)
		{
			spawnPosition.x = 0;
			if (spawnedBlock.isGateThatCanBeFlipped)
				switch (Random.value > 0.5f)
				{
					case true:
						spawnedBlockGO.transform.Rotate(0, 0, 180f);
						break;
				}
		}
		else
		{
			spawnPosition.x = Random.Range(-(Design.gridWidth - 1) / 2, (Design.gridWidth - 1) / 2) * Design.gridSize;
		}


		spawnedBlockGO.transform.position = spawnPosition;


		// register block into world grid controller
		worldGridManager.RegisterBlockIntoWorldBlockList(spawnedBlock);
	}
}