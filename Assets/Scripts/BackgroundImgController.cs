using System.Collections;
using UnityEngine;

public class BackgroundImgController : MonoBehaviour {

	public Color gameOverColor;
	public float lerpSpeed;

	SpriteRenderer sRen;
	
	Color initialColor;
	Vector3 initialLocalPos;


	void Awake()
	{
		sRen = GetComponent<SpriteRenderer>();
		
		initialColor = sRen.color;
		initialLocalPos = transform.localPosition;
	}

	void Start()
	{
		RollInEffect();
	}





	public void RollInEffect()
	{
		StartCoroutine(RollInEffectCoroutine());
	}
	IEnumerator RollInEffectCoroutine()
	{
		Vector3 rolledLocalPos = initialLocalPos;
		rolledLocalPos.y -= 20f;
		const float rollInSpeed = 3f;
		
		for (float t = 0; t <= 1; t += Time.deltaTime * rollInSpeed)
		{
			transform.localPosition = Vector3.Lerp(rolledLocalPos, initialLocalPos, t);
			yield return null;
		}
		
		transform.localPosition = initialLocalPos;
	}




	public void GameOverColorChange()
	{
		StartCoroutine(GameOverColorChangeCoroutine());
	}
	IEnumerator GameOverColorChangeCoroutine()
	{
		for (float t = 0; t < 1; t += Time.unscaledDeltaTime * lerpSpeed)
		{
			sRen.color = Color.Lerp(initialColor, gameOverColor, t);
			
			yield return null;
		}
	}

}