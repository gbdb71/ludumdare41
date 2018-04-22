using System.Collections;
using UnityEngine;

public class BlockPlacingTrailController : MonoBehaviour {

	public float lerpSpeed = 25f;

	TrailRenderer trail;

	public void Init(Vector3 startPos, Vector3 endPos)
	{
		transform.position = startPos;
		trail = GetComponent<TrailRenderer>();
		trail.Clear();
		
		StartCoroutine(AnimateTrajectoryTrailCoroutine(startPos, endPos));
	}
	IEnumerator AnimateTrajectoryTrailCoroutine(Vector3 startPos, Vector3 endPos)
	{
		for (float t = 0; t <= 1; t += Time.unscaledDeltaTime * lerpSpeed)
		{
			transform.position =
				Vector3.Lerp(startPos, endPos, t);
			
			yield return null;
		}
	}

}