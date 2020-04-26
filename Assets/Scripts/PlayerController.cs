using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	const string GridTrackerGameObjectName = "GridTracker";
	const string PotsLeftTextGameObjectName = "PotsLeftText";
	const string PlayerControllerGameObjectName = "PlayerController";
	const string GoButtonName = "GoButton";
	const string CrabCollectedTextGameObjectName = "CrabCollectedText";
	const string TripCostTextGameObjectName = "TripCostText";

	Button goButton;
	GridTracker gridTracker;
	Text potsLeftText;
	Text crabCollectedText;
	GameController gameController;
	Text tripCostText;

	public delegate void AddPotDelegate();
	public delegate void ThrowPotDelegate();
	public delegate void AddCrabDelegate(int crab);

	[SerializeField]
	private int startPots = 7;

	[SerializeField]
	private int startMoney = 30;

	[SerializeField]
	private int tripCost = 15;

	private int money;

	int pots;
	int crab = 0;
	void Awake() {
		pots = startPots;
		gridTracker = GameObject.Find(GridTrackerGameObjectName).GetComponent<GridTracker>();
		potsLeftText = GameObject.Find(PotsLeftTextGameObjectName).GetComponent<Text>();
		gameController = GameController.GetGameController();
		goButton = GameObject.Find(GoButtonName).GetComponent<Button>();
		crabCollectedText = GameObject.Find(CrabCollectedTextGameObjectName).GetComponent<Text>();
		tripCostText = GameObject.Find(TripCostTextGameObjectName).GetComponent<Text>();
	}

	public static PlayerController GetPlayerController() {
		return GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();
	}

	void Start()
	{
		money = startMoney;
		OnMoneyChanged();
		OnTripCostChanged();
		OnPotsChanged();
		OnCrabChanged();
		goButton.onClick.AddListener(() => OnGoClicked());
	}

	void SetTripCost(int tripCost) {
		this.tripCost = tripCost;
		OnTripCostChanged();
	}

	void OnTripCostChanged() {
		tripCostText.text = string.Format("Trip will cost ${0}", tripCost);
	}

	public int GetTripCost() {
		return tripCost;
	}

	void Update() {
		if (!gameController.GameIsOver() && Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			gridTracker.PlaceOrRemovePot(mouseWorldPos, HasPotsLeft(), ThrowPot, AddPot);
		}
	}

	public bool HasPotsLeft() {
		return pots > 0;
	}

	internal void AddPot() {
		pots++;
		OnPotsChanged();
	}

	void OnPotsChanged() {
		potsLeftText.text = "Pots left: " + pots;
		gameController.OnPlayersPotsChanged();

		if (!gameController.InDevelopment)
			goButton.interactable = !HasPotsLeft();
	}

	internal void OnGameOver() {
		goButton.interactable = false;
	}

	internal void ThrowPot() {
		if (pots > 0) 
			pots--;
		OnPotsChanged();
	}

	void AddCrab(int crab) {
		if (crab < 0)
			crab = 0;

		this.crab += crab;
		OnCrabChanged();

		money += crab;
		OnMoneyChanged();
	}

	void OnCrabChanged() {
		crabCollectedText.gameObject.SetActive(crab > 0);
		crabCollectedText.text = string.Format("Haul: {0} crab", crab);
	}

	void OnMoneyChanged() {
		gameController.UpdateMoneyText(money);
	}

	public void OnGoClicked() {
		Debug.Log("Going! :D");
		int crabBefore = crab;
		gridTracker.LiftAllPots(AddPot, AddCrab);

		money -= tripCost;
		OnMoneyChanged();

		gameController.OnAllPotsLifted(crab - crabBefore, money);
	}
}
