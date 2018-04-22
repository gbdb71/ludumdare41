using UnityEngine;
using System.Collections;

public class Wiggle : MonoBehaviour
{

	public float speed;
	public Vector3 range;
	public float waitBeforeTurn;

	bool active = true;

	bool turning;

	Quaternion initialRotation;
	Quaternion[] endRotation;
	int direction;

	float t = 0f;



	void Awake()
	{
		initialRotation = transform.localRotation;

		endRotation = new Quaternion[2];
		endRotation[0] = initialRotation * Quaternion.Euler(range);
		endRotation[1] = initialRotation * Quaternion.Euler(-range);
	}

	void OnEnable()
	{
		Reset();
	}

	void Update()
	{
		if (!active) return;
		if (!turning) return;

		t += Time.unscaledDeltaTime * speed;

		transform.localRotation = Quaternion.Slerp(endRotation[1 - direction], endRotation[direction], t);

		if (t > 1)
		{
			t = 0f;
			turning = false;
			direction = 1 - direction; // direction switch

			// wait for a way back
			StartCoroutine(WaitBeforeTurnCoroutine());
		}
	}
	IEnumerator WaitBeforeTurnCoroutine()
	{
		yield return new WaitForSecondsRealtime(waitBeforeTurn);
		turning = true;
	}





	void Deactivate()
	{
		active = false;
	}

	void Reset()
	{
		turning = true;
		direction = 0;
		t = 0;
	}
}