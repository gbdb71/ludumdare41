using UnityEngine;

public class Rotate : MonoBehaviour
{
	public float speedX, speedY, speedZ;

	Vector3 localRotationVector;

	void Awake()
	{
		localRotationVector.x = speedX;
		localRotationVector.y = speedY;
		localRotationVector.z = speedZ;
	}

	void Update()
	{
		transform.Rotate(localRotationVector * Time.deltaTime, Space.Self);
	}
}