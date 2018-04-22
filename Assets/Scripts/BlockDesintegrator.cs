using UnityEngine;

public class BlockDesintegrator : MonoBehaviour {

	public ParticleSystem particleSys;

	Block block;



	private void Awake()
	{
		block = GetComponent<Block>();
	}



	public void DesintegrateBlock()
	{
		// desintegrate block into separate elements
		FlyAwayWithBlockElements();
		
		// particle effect
		particleSys.Play();
	}



	void FlyAwayWithBlockElements()
	{
		for (int i = 0; i < block.childElements.Length; i++)
		{
			block.childElements[i].FlyAway();
		}
	}
}