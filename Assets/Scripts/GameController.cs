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
	const string WarningLayoutGameObjectName = "WarningLayout";

	Text roundText;
	Text logText;
	Text moneyText;
	Button restartButton;
	PlayerController playerController;
	SwarmController swarmController;
	Button quitButton;
	GameObject warningLayout;
	Text warningText;

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

		warningLayout = GameObject.Find(WarningLayoutGameObjectName);
		warningText = warningLayout.GetComponentInChildren<Text>();
	}

	void Start() {
		restartButton.onClick.AddListener(() => RestartGame());
		restartButton.gameObject.SetActive(false);

		quitButton.onClick.AddListener(() => GameOver());
		quitButton.gameObject.SetActive(InDevelopment);

		logText.text = "";
		UpdateRoundsText(currentRound, numberOfRounds);

		OnTripCostChanged();
		ShowFutureTripCost(tripCost, tripCost + tripCostIncrease);
		HideWarningLayout();
	}

	public void IncreaseTripCost(int increase) {
		tripCost += increase;
		OnTripCostChanged();
		ShowFutureTripCost(tripCost, tripCost + increase);
	}

	void OnTripCostChanged() {
		tripCostText.text = string.Format("This day's trip will cost ${0}", tripCost);
	}

	void ShowFutureTripCost(int nextRoundCost, int futureCost) {
		string message = string.Format("(On the next day, trip will cost ${0})", futureCost);

		int costs = nextRoundCost + futureCost;
		if (playerController.GetMoney() < costs) {
			ShowWarningLayout(costs - playerController.GetMoney());
		} else {
			HideWarningLayout();
		}

		futureTripCostText.text = message;
	}

	void ShowWarningLayout(int needToMakeAmountOnThisTrip) {
		warningLayout.SetActive(true);
		warningText.text = string.Format("You must make ${0} on this day or else you lose!", needToMakeAmountOnThisTrip);
	}

	void HideWarningLayout() {
		warningLayout.SetActive(false);
	}


	void ShowLastRoundPromptText() {
		futureTripCostText.text = "(This is the last day)";
	}

	public int GetTripCost() {
		return tripCost;
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
		if ( IsLastRound(currentRound) || currentMoney < tripCost) {
			GameOver();
		} else {
			currentRound++;
			UpdateRoundsText(currentRound, numberOfRounds);
			
			if (IsLastRound(currentRound)) {
				ShowLastRoundPromptText();
			} else {
				IncreaseTripCost(tripCostIncrease);
			}
		}
	}

	bool IsLastRound(int currentRound) {
		return currentRound == numberOfRounds;
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
		roundText.text = string.Format("Day: {0}/{1}", currentRound, numberOfRounds);
	}
	
	void UpdateLogText(int round, int roundCrabHaul) {
		logText.text += string.Format("Day {0} hauled {1} crab, gained ${2}", round, roundCrabHaul, roundCrabHaul) + "\n";
	}

	internal void OnPlayersPotsChanged() {
		
	}
}
