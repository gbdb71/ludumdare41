using UnityEngine;

[System.Serializable]
public class musicTrack
{
	public AudioClip game;
	public AudioClip gameOver;
}

public class MusicManager : MonoBehaviour
{

	public musicTrack musicTrack;

	[Space(10)]
	public float fadeOutTimeScaleMultiplier;

	public enum musicId : int
	{
		game,
		gameOver
	}

	public bool IsPlaying
	{
		get
		{
			return audioSource.isPlaying;
		}
	}

	float defaultMusicVolume;

	float lastRealTimeSinceStartup, unscaledDeltaTime;

	// object cache
	private AudioSource audioSource;




	void Awake()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.Pause();

		defaultMusicVolume = audioSource.volume;
	}



	public void LoadMusicTrack(musicId trackId)
	{
		switch (trackId)
		{
			case musicId.game:
				audioSource.clip = musicTrack.game;
				break;
			case musicId.gameOver:
				audioSource.clip = musicTrack.gameOver;
				break;
		}
	}

	public void Play()
	{
		CancelFadeOut();
		audioSource.Play();
	}

	public void Pause()
	{
		audioSource.Pause();
	}

	public void FadeOut()
	{
		StartCoroutine(FadeOutCoroutine());
	}
	public void CancelFadeOut()
	{
		StopAllCoroutines();
		RestoreVolume();
	}
	System.Collections.IEnumerator FadeOutCoroutine()
	{
		lastRealTimeSinceStartup = Time.realtimeSinceStartup;

		while (audioSource.volume > .001f)
		{
			unscaledDeltaTime = Time.realtimeSinceStartup - lastRealTimeSinceStartup;

			audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, unscaledDeltaTime * fadeOutTimeScaleMultiplier);
			yield return 0;
		}

		// fadout completed
		audioSource.volume = 0;
		Pause();
		RestoreVolume();
	}
	void RestoreVolume()
	{
		audioSource.volume = defaultMusicVolume;
	}
}