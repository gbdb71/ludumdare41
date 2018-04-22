using System.Collections;
using UnityEngine;

public class CameraEffector : MonoBehaviour {

	Camera cam;

	float initialFOV;
//	Vector3 initialLocalPosition;

	Quaternion initialLocalRotation;
	Quaternion modLocalRotation;


	private void Awake()
	{
		cam = GetComponent<Camera>();
		initialFOV = cam.orthographicSize;
//		initialLocalPosition = transform.localPosition;
		
		initialLocalRotation = transform.localRotation;
		modLocalRotation = Quaternion.Euler(0, 0, Random.Range(-5, 5));
	}

	public void ModifyCam(float f)
	{
		cam.orthographicSize = initialFOV - f;
		
		cam.gameObject.transform.localRotation =
			Quaternion.Lerp(
				initialLocalRotation,
				modLocalRotation,
				-f / 10f);
	}
}