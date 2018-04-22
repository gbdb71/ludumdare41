using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Text scoreText;



	public void UpdateHUD()
	{
		scoreText.text = Game.score.ToString();
	}

}