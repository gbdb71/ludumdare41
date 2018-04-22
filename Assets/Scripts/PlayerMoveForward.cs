using UnityEngine;

public class PlayerMoveForward : MonoBehaviour {

	public Vector3 moveForwardVector;
	public float boostFwdMultiplier = 2f;
	const float boostFwdAttackRelease = 10f;

	Vector3 initialMoveFwdVector;

	bool boosting = false;


	private void Awake()
	{
		initialMoveFwdVector = moveForwardVector;
	}

	void Update()
	{
		transform.position -= moveForwardVector * Time.deltaTime;
		
		// speedup or slowdown
		switch(boosting)
		{
			case true:
				moveForwardVector =
					Vector3.Lerp(
						moveForwardVector,
						initialMoveFwdVector * boostFwdMultiplier,
						Time.deltaTime * boostFwdAttackRelease);
				break;

			case false:
				moveForwardVector =
					Vector3.Lerp(
						moveForwardVector,
						initialMoveFwdVector,
						Time.deltaTime * boostFwdAttackRelease);
				break;
		}
	}

	void LateUpdate()
	{
		boosting = false;
	}




	public void Boost()
	{
		boosting = true;
	}
}