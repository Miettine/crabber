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
	Text roundText;
	Text logText;
	Button restartButton;
	PlayerController playerController;
	SwarmController swarmController;

	[SerializeField]
	private int numberOfRounds = 7;

	int currentRound = 1;

	bool gameOver = false;

	private void Awake() {
		//gridTracker = GameObject.Find(GridTrackerName).GetComponent<GridTracker>();
		//playerController = PlayerController.GetPlayerController();
		roundText = GameObject.Find(RoundTextGameObjectName).GetComponent<Text>();
		logText = GameObject.Find(LogTextGameObjectName).GetComponent<Text>();
		restartButton = GameObject.Find(RestartButtonGameObjectName).GetComponent<Button>();
		playerController = PlayerController.GetPlayerController();
		swarmController = SwarmController.GetSwarmController();
	}

	private void Start() {
		restartButton.onClick.AddListener(() => RestartGame());
		restartButton.gameObject.SetActive(false);

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


	internal bool GameIsOver() {
		return gameOver;
	}

	void UpdateRoundsText(int currentRound, int numberOfRounds) {
		roundText.text = "Round: " + currentRound + "/" + numberOfRounds;
	}
	
	void UpdateLogText(int round, int roundCrabHaul) {
		logText.text += string.Format("Round {0} hauled {1} crab", round, roundCrabHaul) + "\n";
	}


	internal void OnPlayersPotsChanged() {
		
	}
}
