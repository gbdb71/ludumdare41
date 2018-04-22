using UnityEngine;

public class Follow : MonoBehaviour {

	public GameObject target = null;

	Vector3 relPosition;
	Vector3 initialPosition;

	public bool freezeHorizontalAxis;




	void Awake ()
	{
		initialPosition = transform.position;
		
		SetCurrentPositionAsRelative();
	}

	void LateUpdate ()
	{
		if (!target) return;
		
		MoveToDesiredPosition();
	}




	public void MoveToDesiredPosition()
	{
		transform.position = relPosition + target.transform.position;

		if (freezeHorizontalAxis)
		{
			transform.position =
				new Vector3(
					initialPosition.x,
					transform.position.y,
					transform.position.z);
		}
	}

	/*public void SetTarget(GameObject newTarget)
	{
		target = newTarget;
		
		MoveToDesiredPosition();
	}*/

	public void SetCurrentPositionAsRelative()
	{
		if (target)
			relPosition = initialPosition - target.transform.position;
		else
			relPosition = initialPosition;
	}
}