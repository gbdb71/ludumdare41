using System.Collections;
using UnityEngine;

public class CameraBgColorController : MonoBehaviour {

	public Color gameOverColor;
	public float lerpSpeed;

	Camera cam;
	
	Color initialColor;


	private void Awake()
	{
		cam = GetComponent<Camera>();

		initialColor = cam.backgroundColor;
	}



	public void GameOverColorChange()
	{
		StartCoroutine(GameOverColorChangeCoroutine());
	}
	IEnumerator GameOverColorChangeCoroutine()
	{
		for (float t = 0; t < 1; t += Time.unscaledDeltaTime * lerpSpeed)
		{
			cam.backgroundColor = Color.Lerp(initialColor, gameOverColor, t);
			
			yield return null;
		}
	}

}