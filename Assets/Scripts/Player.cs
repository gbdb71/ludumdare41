using UnityEngine;

public class Player : MonoBehaviour {

	public GameManager gameManager;

	[Header("Sub Scripts")]
	public PlayerController playerController;
	public PlayerMoveForward playerMoveForward;

	public PlayerBlockGun playerBlockGun;

	public BlockDesintegrator playerBlockDesintegrator;

}