using System;
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
	const string NotificationTextGameObjectName = "NotificationText";

	Text roundText;
	Text logText;
	Text moneyText;
	Button restartButton;
	PlayerController playerController;
	SwarmController swarmController;
	Button quitButton;
	Text notificationText;

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
		notificationText = GameObject.Find(NotificationTextGameObjectName).GetComponent<Text>();
	}

	void Start() {
		restartButton.onClick.AddListener(() => RestartGame());
		restartButton.gameObject.SetActive(false);

		quitButton.onClick.AddListener(() => GameOver(false));
		quitButton.gameObject.SetActive(InDevelopment);

		logText.text = "";
		UpdateRoundsText(currentRound, numberOfRounds);

		OnTripCostChanged();
		ShowFutureTripCost(tripCost, tripCost + tripCostIncrease);
		HideNotificationText();
	}

	public void IncreaseTripCost(int increase) {
		tripCost += increase;
		OnTripCostChanged();
		ShowFutureTripCost(tripCost, tripCost + increase);
	}

	void OnTripCostChanged() {
		tripCostText.text = string.Format("This day's trip costs ${0}", tripCost);
	}

	void ShowFutureTripCost(int nextRoundCost, int futureCost) {
		string message = string.Format("(On the next day, trip will cost ${0})", futureCost);

		int costs = nextRoundCost + futureCost;
		if (playerController.GetMoney() < costs) {
			tripCostText.color = Color.yellow;
			futureTripCostText.color = Color.yellow;
			ShowWarningLayout(costs - playerController.GetMoney());
		} else {
			tripCostText.color = Color.white;
			futureTripCostText.color = Color.white;
			HideNotificationText();
		}

		futureTripCostText.text = message;
	}

	void ShowWarningLayout(int needToMakeAmountOnThisTrip) {
		ShowNotificationText(string.Format("You must make ${0} on this day or you lose!", needToMakeAmountOnThisTrip), Color.yellow);
	}

	void ShowNotificationText(String text, Color color) {
		notificationText.gameObject.SetActive(true);
		notificationText.color = color;
		notificationText.text = text;
	}

	void HideNotificationText() {
		notificationText.gameObject.SetActive(false);
	}


	void ShowLastRoundPromptText() {
		futureTripCostText.color = Color.white;
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
		if (IsLastRound(currentRound)) {
			GameOver(true);
		} else if (currentMoney < tripCost) {
			GameOver(false);
		} else {
			currentRound++;
			UpdateRoundsText(currentRound, numberOfRounds);
			
			if (IsLastRound(currentRound)) {
				ShowLastRoundPromptText();
				HideNotificationText();
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

	void GameOver(bool wonTheGame) {
		gameOver = true;
		HideNotificationText();
		if (wonTheGame) {
			ShowVictoryNotification();
		} else {
			ShowLostNotification();
		}
		restartButton.gameObject.SetActive(true);
		playerController.OnGameOver();
		swarmController.RevealAllSwarms();
	}

	void ShowVictoryNotification() {
		ShowNotificationText("You've won the game!", Color.green);
	}

	void ShowLostNotification() {
		tripCostText.color = Color.red;
		ShowNotificationText("You cannot pay the trip cost. You have lost!", Color.red);
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
		//Apparently nothing happens
	}
}
