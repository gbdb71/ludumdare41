using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public AppManager appManager;

	[Space]
	public SoundManager soundManager;
	public Player player;
	public BackgroundImgController bgImgController;
	public CameraBgColorController cameraBgColController;
	public CameraEffector cameraEffector;
	public WorldGridManager worldGridManager;
	public GUI gui;
	public HUD hud;
	public GameFader gameFader;

	[HideInInspector]
	public bool playerAlive = true;


	void Awake ()
	{
	}

	void Start()
	{
		// game begins!

		Game.InitNewGameValues();

		gui.SetGameState();
		hud.UpdateHUD();

		soundManager.musicManager.LoadMusicTrack(MusicManager.musicId.game);
		soundManager.musicManager.Play();

		// reload block gun
		player.playerBlockGun.Reload();
	}




	// gameplay

	public void BlockCannonFired()
	{
		StopAllCoroutines();
		StartCoroutine(BlockCannonFiredCheckIfLineFinishedCoroutine());
	}
	IEnumerator BlockCannonFiredCheckIfLineFinishedCoroutine()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		
		// if fired cannon and no line done (nothing stopped this coroutine)
		// shift world up
		worldGridManager.ShiftEverythingBack();
	}


	public void LineFinished()
	{
		// stop upper coroutine for shifting the world if no line finished on cannon fire
		StopAllCoroutines();

		soundManager.Play(SoundManager.soundId.point);
		
		Game.IncrementScore();
		
		hud.UpdateHUD();
	}



	public void PlayerCollided()
	{
		if (!playerAlive) return; // player already dead

		playerAlive = false;

		appManager.SlowDownTimescale();
		bgImgController.GameOverColorChange();
		cameraBgColController.GameOverColorChange();

		// desintegrate player block
		player.playerBlockDesintegrator.DesintegrateBlock();

		// deisntegrate block loaded in playerBlockGun
		player.playerBlockGun.FlyAwayWithGunLoadedBlockElements();

		soundManager.Play(SoundManager.soundId.gameOver);

		soundManager.musicManager.LoadMusicTrack(MusicManager.musicId.gameOver);
		soundManager.musicManager.Play();


		gameFader.LerpToSemiTransparent(true);

		gui.SetGameOverState();
	}
}