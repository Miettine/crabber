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
	const string MoneyTextGameObjectName = "MoneyText";

	Text roundText;
	Text logText;
	Text moneyText;
	Button restartButton;
	PlayerController playerController;
	SwarmController swarmController;
	Button quitButton;

	[SerializeField]
	private int numberOfRounds = 7;

	[SerializeField]
	private int tripCostIncrease = 10;

	int currentRound = 1;

	bool gameOver = false;

	[SerializeField]
	bool inDevelopment = true;

	[SerializeField]
	private int tripCost = 10;


	const string TripCostTextGameObjectName = "TripCostText";
	const string FutureTripCostTextGameObjectName = "FutureTripCostText";

	Text tripCostText;
	Text futureTripCostText;

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
		moneyText = GameObject.Find(MoneyTextGameObjectName).GetComponent<Text>();
		tripCostText = GameObject.Find(TripCostTextGameObjectName).GetComponent<Text>();
		futureTripCostText = GameObject.Find(FutureTripCostTextGameObjectName).GetComponent<Text>();
	}
	
	public void IncreaseTripCost(int increase) {
		tripCost += increase;
		OnTripCostChanged();
		ShowFutureTripCost(tripCost + increase);
	}

	void OnTripCostChanged() {
		tripCostText.text = string.Format("Trip will cost ${0}", tripCost);
	}

	void ShowFutureTripCost(int futureCost) {
		futureTripCostText.text = string.Format("(After this round, trip will cost ${0})", futureCost);
	}

	public int GetTripCost() {
		return tripCost;
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
	internal void OnAllPotsLifted(int crabHaul, int currentMoney) {
		OnRoundOver(crabHaul, currentMoney);
	}

	internal void OnRoundOver(int roundCrabHaul, int currentMoney) {
		UpdateLogText(currentRound, roundCrabHaul);
		if (currentRound == numberOfRounds || currentMoney < tripCost) {
			GameOver();
		} else {
			currentRound++;
			UpdateRoundsText(currentRound, numberOfRounds);
			this.IncreaseTripCost(tripCostIncrease);
		}
	}

	public void UpdateMoneyText(int currentMoney) {
		moneyText.text = string.Format("Money: ${0}", currentMoney);
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
		logText.text += string.Format("Round {0} hauled {1} crab, gained {2}$", round, roundCrabHaul, roundCrabHaul) + "\n";
	}

	internal void OnPlayersPotsChanged() {
		
	}
}
