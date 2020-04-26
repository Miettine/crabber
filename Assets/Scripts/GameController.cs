using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	
	//const string GridTrackerName = "GridTracker";
	const string GameControllerGameObjectName = "GameController";
	const string RoundTextGameObjectName = "RoundsText";
	const string LogTextGameObjectName = "LogText";
	const string RestartButtonGameObjectName = "RestartButton";
	const string QuitButtonGameObjectName = "QuitButton";

	Text roundText;
	Text logText;
	Button restartButton;
	PlayerController playerController;
	SwarmController swarmController;
	Button quitButton;

	[SerializeField]
	private int numberOfRounds = 7;

	int currentRound = 1;

	bool gameOver = false;

	[SerializeField]
	bool inDevelopment = true;

	public bool InDevelopment { get { return inDevelopment; } }

	private void Awake() {
		//gridTracker = GameObject.Find(GridTrackerName).GetComponent<GridTracker>();
		//playerController = PlayerController.GetPlayerController();
		roundText = GameObject.Find(RoundTextGameObjectName).GetComponent<Text>();
		logText = GameObject.Find(LogTextGameObjectName).GetComponent<Text>();
		restartButton = GameObject.Find(RestartButtonGameObjectName).GetComponent<Button>();
		playerController = PlayerController.GetPlayerController();
		swarmController = SwarmController.GetSwarmController();
		quitButton = GameObject.Find(QuitButtonGameObjectName).GetComponent<Button>();
	}

	private void Start() {
		restartButton.onClick.AddListener(() => RestartGame());
		restartButton.gameObject.SetActive(false);

		quitButton.onClick.AddListener(() => GameOver());
		quitButton.gameObject.SetActive(InDevelopment);

		logText.text = "";
		UpdateRoundsText(currentRound, numberOfRounds);
	}

	void RestartGame() {
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	public static GameController GetGameController() {
		return GameObject.Find(GameControllerGameObjectName).GetComponent<GameController>();
	}
	internal void OnAllPotsLifted(int crabHaul) {
		OnRoundOver(crabHaul);
	}

	internal void OnRoundOver(int roundCrabHaul) {
		UpdateLogText(currentRound, roundCrabHaul);
		if (currentRound == numberOfRounds) {
			GameOver();
		} else {
			currentRound++;
			UpdateRoundsText(currentRound, numberOfRounds);
		}
	}

	void GameOver() {
		gameOver = true;
		restartButton.gameObject.SetActive(true);
		playerController.OnGameOver();
		swarmController.RevealAllSwarms();
	}
	public bool GameIsOver() {
		return gameOver;
	}

	void UpdateRoundsText(int currentRound, int numberOfRounds) {
		roundText.text = string.Format("Round: {0}/{1}", currentRound, numberOfRounds);
	}
	
	void UpdateLogText(int round, int roundCrabHaul) {
		logText.text += string.Format("Round {0} hauled {1} crab", round, roundCrabHaul) + "\n";
	}

	internal void OnPlayersPotsChanged() {
		
	}
}
