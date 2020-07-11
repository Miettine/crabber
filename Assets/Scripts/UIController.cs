using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Deals with the canvas UI-elements
/// </summary>
public class UIController : Singleton<UIController> {

	/*
	 * I use the SerializeField-annotation to make the field serialized 
	 * in the Unity editor but still keep it a private member in the code.
	 * */
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
	[SerializeField]
	string PotsLeftTextGameObjectName = "PotsLeftText";
	[SerializeField]
	string GoButtonName = "GoButton";
	[SerializeField]
	string CrabCollectedTextGameObjectName = "CrabCollectedText";
	
	Button goButton;
	Text potsLeftText;
	Text crabCollectedText;
	Text roundText;
	Text logText;
	Text moneyText;
    Button restartButton;
    Button quitButton;
	Text notificationText;
	Text tripCostText;
	Text futureTripCostText;

	PlayerController playerController;
	GameController gameController;

	private void Awake() {

		gameController = GameController.GetInstance();
		playerController = PlayerController.GetInstance();

		/*
		 * With 2 years working experience with Unity, 
		 * I determined that this is the best way to find object references to components in a Unity scene:
		 * */
		roundText = GameObject.Find(RoundTextGameObjectName).GetComponent<Text>();
		logText = GameObject.Find(LogTextGameObjectName).GetComponent<Text>();
		restartButton = GameObject.Find(RestartButtonGameObjectName).GetComponent<Button>();

		quitButton = GameObject.Find(QuitButtonGameObjectName).GetComponent<Button>();
		moneyText = GameObject.Find(MoneyTextGameObjectName).GetComponent<Text>();
		tripCostText = GameObject.Find(TripCostTextGameObjectName).GetComponent<Text>();
		futureTripCostText = GameObject.Find(FutureTripCostTextGameObjectName).GetComponent<Text>();
		notificationText = GameObject.Find(NotificationTextGameObjectName).GetComponent<Text>();
		potsLeftText = GameObject.Find(PotsLeftTextGameObjectName).GetComponent<Text>();
		goButton = GameObject.Find(GoButtonName).GetComponent<Button>();
		crabCollectedText = GameObject.Find(CrabCollectedTextGameObjectName).GetComponent<Text>();

		goButton.onClick.AddListener(() => playerController.OnGoClicked());
	}
	internal void OnPotsChanged() {
		potsLeftText.text = "Pots left: " + playerController.GetPots();

		if (!gameController.InDevelopment)
			goButton.interactable = !playerController.HasPotsLeft();
	}

	internal void OnRoundOver(int roundCrabHaul) {
		int currentRound = gameController.CurrentRound;
		UpdateRoundsText(currentRound, gameController.NumberOfRounds);
		UpdateLogText(currentRound-1, roundCrabHaul);

		if (!gameController.IsGameOver() && gameController.IsLastRound()) {
			ShowLastRoundPromptText();
			HideNotificationText();
		}
	}

	internal void OnGameOver() {
		goButton.gameObject.SetActive(false);
		HideNotificationText();
		if (gameController.WonTheGame) {
			HideTripCostTexts();
			ShowVictoryNotification();
		} else {
			ShowLostNotification();
		}
		restartButton.gameObject.SetActive(true);
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

	internal void OnCrabChanged() {
		int crab = playerController.Crab;
		crabCollectedText.gameObject.SetActive(crab > 0);
		crabCollectedText.text = string.Format("Haul: {0} crab", crab);
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
