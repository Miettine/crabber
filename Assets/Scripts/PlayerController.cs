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

	Button goButton;
	GridTracker gridTracker;
	Text potsLeftText;
	Text crabCollectedText;
	GameController gameController;

	public delegate void AddPotDelegate();
	public delegate void ThrowPotDelegate();
	public delegate void AddCrabDelegate(int crab);

	[SerializeField]
	private int pots = 6;

	int crab = 0;

	[SerializeField]
	private int money = 30;

	void Awake() {
		gridTracker = GameObject.Find(GridTrackerGameObjectName).GetComponent<GridTracker>();
		potsLeftText = GameObject.Find(PotsLeftTextGameObjectName).GetComponent<Text>();
		gameController = GameController.GetGameController();
		goButton = GameObject.Find(GoButtonName).GetComponent<Button>();
		crabCollectedText = GameObject.Find(CrabCollectedTextGameObjectName).GetComponent<Text>();
	}

	public static PlayerController GetPlayerController() {
		return GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();
	}

	public int GetMoney() {
		return money;
	}

	void Start()
	{
		OnMoneyChanged();
		OnPotsChanged();
		OnCrabChanged();
		goButton.onClick.AddListener(() => OnGoClicked());
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

		money -= gameController.GetTripCost();
		OnMoneyChanged();

		gameController.OnAllPotsLifted(crab - crabBefore, money);
	}
}
