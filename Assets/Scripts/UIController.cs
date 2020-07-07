using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Deals with the canvas UI-elements
/// </summary>
public class UIController : MonoBehaviour
{

	const string UIControllerGameObjectName = "UIController";

	[SerializeField]
	string RoundTextGameObjectName = "RoundsText";
	[SerializeField]
	string LogTextGameObjectName = "LogText";
	[SerializeField]
	string RestartButtonGameObjectName = "RestartButton";
	[SerializeField]
	string QuitButtonGameObjectName = "QuitButton";
	[SerializeField]
	string MoneyTextGameObjectName = "MoneyText";
	[SerializeField]
	string NotificationTextGameObjectName = "NotificationText";
	[SerializeField]
	string TripCostTextGameObjectName = "TripCostText";
	[SerializeField]
	string FutureTripCostTextGameObjectName = "FutureTripCostText";

	Text roundText;
	Text logText;
	Text moneyText;
    Button restartButton;
    PlayerController playerController;
    SwarmController swarmController;
    Button quitButton;
	Text notificationText;
	Text tripCostText;
	Text futureTripCostText;

	GameController gameController;

	public static UIController GetUIController() {
		return GameObject.Find(UIControllerGameObjectName).GetComponent<UIController>();
	}

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
		notificationText = GameObject.Find(NotificationTextGameObjectName).GetComponent<Text>();

		gameController = GameController.GetGameController();
	}

	internal void OnRoundOver(int roundCrabHaul) {
		int currentRound = gameController.CurrentRound;
		UpdateRoundsText(currentRound, gameController.NumberOfRounds);
		UpdateLogText(currentRound, roundCrabHaul);

		if (!gameController.IsGameOver() && gameController.IsLastRound()) {
			ShowLastRoundPromptText();
			HideNotificationText();
		}
	}

	internal void OnGameOver() {
		HideNotificationText();
		if (gameController.WonTheGame) {
			HideTripCostTexts();
			ShowVictoryNotification();
		} else {
			ShowLostNotification();
		}
		restartButton.gameObject.SetActive(true);
		playerController.OnGameOver();
		swarmController.RevealAllSwarms();
	}

	void Start() {
		restartButton.onClick.AddListener(() => gameController.RestartGame());
		restartButton.gameObject.SetActive(false);

		quitButton.onClick.AddListener(() => gameController.GameOver(false));
		quitButton.gameObject.SetActive(gameController.InDevelopment);

		logText.text = "";
		UpdateRoundsText(gameController.CurrentRound, gameController.NumberOfRounds);

		OnTripCostChanged();
		HideNotificationText();
	}

	internal void OnMoneyChanged() {
		moneyText.text = string.Format("Money: ${0}", playerController.GetMoney());
	}

	public void OnTripCostChanged() {
		int tripCost = gameController.TripCost;
		tripCostText.text = string.Format("This day's trip costs ${0}", tripCost);
		ShowFutureTripCost(tripCost, tripCost + gameController.TripCostIncrease, playerController.GetMoney());
	}


	void ShowFutureTripCost(int nextRoundCost, int futureCost, int playerMoney) {
		string message = string.Format("(On the next day, trip will cost ${0})", futureCost);

		int costs = nextRoundCost + futureCost;
		if (playerMoney < costs) {
			tripCostText.color = Color.yellow;
			futureTripCostText.color = Color.yellow;
			ShowWarningLayout(costs - playerMoney);
		} else {
			tripCostText.color = Color.white;
			futureTripCostText.color = Color.white;
			HideNotificationText();
		}

		futureTripCostText.text = message;
	}

	void ShowNotificationText(string text, Color color) {
		notificationText.gameObject.SetActive(true);
		notificationText.color = color;
		notificationText.text = text;
	}

	void HideNotificationText() {
		notificationText.gameObject.SetActive(false);
	}


	void ShowLastRoundPromptText() {
		tripCostText.color = Color.white;
		futureTripCostText.color = Color.white;
		futureTripCostText.text = "(This is the last day)";
	}

	void HideTripCostTexts() {
		tripCostText.gameObject.SetActive(false);
		futureTripCostText.gameObject.SetActive(false);
	}

	void ShowVictoryNotification() {
		ShowNotificationText("You've won the game!", Color.green);
	}

	void ShowLostNotification() {
		tripCostText.color = Color.red;
		ShowNotificationText("You cannot pay the trip cost. You have lost!", Color.red);
	}

	void UpdateRoundsText(int currentRound, int numberOfRounds) {
		roundText.text = string.Format("Day: {0}/{1}", currentRound, numberOfRounds);
	}

	void UpdateLogText(int round, int roundCrabHaul) {
		logText.text += string.Format("Day {0} hauled {1} crab, gained ${2}", round, roundCrabHaul, roundCrabHaul) + "\n";
	}
	void ShowWarningLayout(int needToMakeAmountOnThisTrip) {
		ShowNotificationText(string.Format("You must make ${0} on this day or you lose!", needToMakeAmountOnThisTrip), Color.yellow);
	}

}
