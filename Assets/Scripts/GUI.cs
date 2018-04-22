using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public AppManager appManager;


	[Space]
	public Button buttonRetry;





	
	// states of gui

	public void SetGameState()
	{
		buttonRetry.interactable = false;
	}

	public void SetGameOverState()
	{
		buttonRetry.interactable = true;
	}






	// reactions on gui

	public void RetryButtonDown()
	{
		appManager.RestartCurrentScene();
	}
}