using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AppManager : MonoBehaviour {

	public GameManager gameManager;


	float defaultTimescale;
	const float slowDownSpeed = 1f;


	private void Awake()
	{
		defaultTimescale = Time.timeScale;
	}



	// timescale management

	public void SlowDownTimescale()
	{
		StartCoroutine(SlowDownTimescaleCoroutine());
	}
	IEnumerator SlowDownTimescaleCoroutine()
	{
		float newTimeScale;

		while (Time.timeScale > 0)
		{
			newTimeScale = Mathf.Clamp(Time.timeScale - Time.unscaledDeltaTime * slowDownSpeed, 0, 1);
			Time.timeScale = newTimeScale;
			yield return null;
		}
	}
	void RestoreTimescale()
	{
		Time.timeScale = defaultTimescale;
	}





	// scene management

	public void RestartCurrentScene()
	{
		RestoreTimescale();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}




	// input management
	public void EscapeOrBackKeysPressed()
	{
		Exit();
	}
	
	
	
	
	
	// OS
	
	public void Exit()
	{
#if UNITY_EDITOR
		Debug.Log("Unable to exit app in editor");
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
		using (AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
		AndroidJavaObject unityActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
		unityActivity.Call<bool>("moveTaskToBack", true);
		}
#else
		Application.Quit();
#endif
	}
}