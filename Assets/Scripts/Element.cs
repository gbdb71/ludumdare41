using System.Collections;
using UnityEngine;

public class Element : MonoBehaviour {

	Vector3 gridPos;

	[HideInInspector]
	public bool liberate = false; // valid obstacle or not

	[HideInInspector]
	public Block parentBlock;

	SpriteRenderer sRen;
	Vector3 initialElementLocalScale;



	private void Awake()
	{
		parentBlock = GetComponentInParent<Block>();
		
		sRen = GetComponent<SpriteRenderer>();
		initialElementLocalScale = transform.localScale;
	}


	void Start ()
	{
		Init();
	}




	public void Init()
	{
		CalculateGridPosition();
	}


	public void CalculateGridPosition()
	{
		// find element own grid position
		gridPos.x = Mathf.RoundToInt(transform.position.x / Design.gridSize);
		gridPos.y = Mathf.RoundToInt(transform.position.y / Design.gridSize);
	}


	public Vector3 GetGridPosition()
	{
		return gridPos;
	}
	public float GetGridPosY()
	{
		return gridPos.y;
	}
	public float GetGridPosX()
	{
		return gridPos.x;
	}





	// renderer qualities

	public void GhostifyRenderer(Material ghostMaterial)
	{
		sRen.material = ghostMaterial;
	}





	// liberation process

	// explosion fx
	public void FlyAway()
	{
		if (liberate) return;
		liberate = true;


		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		//rb.isKinematic = true;
		
		const float liberationForce = 10f;
		
		rb.AddForce(Random.insideUnitCircle * liberationForce, ForceMode2D.Impulse);

		ZoomOut();
	}


	public void DeactivateAsPartOfCompletedLine()
	{
		if (parentBlock.isGate) FlyAway(); // if gate is broken with flyaway style

		if (liberate) return;
		liberate = true;
		
		ElementPartOfLineCompletionEffect();
	}




	// effects

	public void ZoomOut()
	{
		StartCoroutine(ZoomOutCoroutine());
	}
	IEnumerator ZoomOutCoroutine()
	{
		const float zoomOutSpeed = 1f;

		for (float t = 0; t < 1; t += Time.unscaledDeltaTime * zoomOutSpeed)
		{
			transform.localScale = Vector3.Lerp(initialElementLocalScale, Vector3.zero, t);
			yield return null;
		}

		transform.localScale = Vector3.zero;
	}

	public void ZoomIn()
	{
		StartCoroutine(ZoomInCoroutine());
	}
	IEnumerator ZoomInCoroutine()
	{
		const float zoomInSpeed = 5f;
		
		for (float t = 0; t < 1; t += Time.unscaledDeltaTime * zoomInSpeed)
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, initialElementLocalScale, t);
			yield return null;
		}

		transform.localScale = initialElementLocalScale;
	}

	public void ElementPartOfLineCompletionEffect()
	{
		StartCoroutine(ElementPartOfLineCompletionEffectCoroutine());
	}
	IEnumerator ElementPartOfLineCompletionEffectCoroutine()
	{
		const float zoomOutSpeed = 2.5f;

		for (float t = 0; t < 1; t += Time.unscaledDeltaTime * zoomOutSpeed)
		{
			transform.localScale = Vector3.Lerp(initialElementLocalScale, Vector3.zero, t);
			yield return null;
		}

		transform.localScale = Vector3.zero;

	}
}