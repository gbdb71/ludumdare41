using UnityEngine;

public static class Game {
	
	public static int score;
	public static int topScore;
	
	static bool itemPresent;




	public static void Init()
	{
		topScore = PlayerPrefs.GetInt("topscore", 0);
	}

	public static void InitNewGameValues()
	{
		score = 0;
	}




	public static void IncrementScore()
	{
		score++;
		
		if (score > topScore)
		{
			PlayerPrefs.SetInt("topscore", score);
			topScore = score;
		}
	}
}