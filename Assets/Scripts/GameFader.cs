using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameFader : MonoBehaviour
{

	[Header("When Lerping")]
	public float lerpSpeed;
	public float additionalCamMod;

	[Space]
	public CameraEffector destCamera;

	[HideInInspector]
	public float currentCamModifier = 0f;

	const float transparent = 0f;
	const float semiTransparent = 0.8f;

	// runtime
	Color c;
	float a;

	// object cache
	Image image;


	void Awake()
	{
		image = GetComponent<Image>();
		image.enabled = false;
	}






	public void SetTransparent()
	{
		SetAlpha(transparent);
		image.enabled = false;

		// reset all processes
		SetCameraModifier(0);
		StopAllCoroutines();
	}
	public void SetSemiTransparent()
	{
		SetAlpha(semiTransparent);
		image.enabled = true;
	}


	public void LerpToTransparent()
	{
		// initial state - reset
		SetSemiTransparent();
		
		StartCoroutine(LerpToTransparentCoroutine());
	}
	IEnumerator LerpToTransparentCoroutine()
	{
		a = semiTransparent;
		while (a > transparent + 0.01f)
		{
			a = Mathf.Lerp(a, transparent, Time.unscaledDeltaTime * lerpSpeed);
			SetAlpha(a);

			yield return null;
		}
		SetTransparent();
	}


	public void LerpToSemiTransparent(bool withFOVZoom)
	{
		// initial reset and enable
		SetTransparent();
		image.enabled = true;

		StartCoroutine(LerpToSemiTransparentCoroutine(withFOVZoom));
	}
	IEnumerator LerpToSemiTransparentCoroutine(bool withCamModFx)
	{
		a = transparent;
		while (a < semiTransparent - 0.01f)
		{
			a = Mathf.Lerp(a, semiTransparent, Time.unscaledDeltaTime * lerpSpeed);
			SetAlpha(a);
			
			if (withCamModFx)
				SetCameraModifier(Mathf.Lerp(currentCamModifier, additionalCamMod, Time.unscaledDeltaTime * lerpSpeed));
			
			yield return null;
		}
		SetSemiTransparent();
	}



	private void SetAlpha(float alpha)
	{
		c = image.color;
		c.a = Mathf.Clamp(alpha, 0, 1);
		image.color = c;
	}
	private void SetCameraModifier(float f)
	{
		currentCamModifier = f;
		
		destCamera.ModifyCam(currentCamModifier);
	}
}