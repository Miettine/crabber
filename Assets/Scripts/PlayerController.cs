﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Deals with whatever the player has and can do.
/// </summary>
public class PlayerController : Singleton<PlayerController>
{


	GridTracker gridTracker;
	GameController gameController;

	UIController ui;
	public int Crab { get; private set; } = 0;

	public delegate void AddPotDelegate();
	public delegate void ThrowPotDelegate();
	public delegate void AddCrabDelegate(int crab);

	/// <summary>
	/// Represents the number of pots that the player is capable of carrying on their boat. 
	/// In case you don't know, a pot is what a crab fishing trap is called. The trap is made of metal, so it
	/// sinks to the bottom of the water bed. It's attached with a rope to a floating buoy at the surface.
	/// </summary>
	int pots;

	int money;

	public int GetPots() { 
		return pots; 
	}

	void Awake() {
		gridTracker = GridTracker.GetInstance();
		gameController = GameController.GetInstance();
		ui = UIController.GetInstance();

		DifficultyLevel difficulty = gameController.Difficulty;
		pots = difficulty.StartingPots;

		/**
		 * The following is a simplified equation of the summed together trip costs of the first three rounds.
		 * I want to guarantee the player to always be able to play the first three rounds 
		 * without losing. This is to lessen the player's frustration.
		 * */
		money = 3 * gameController.TripCost + 3 * gameController.TripCostIncrease;

	}

	public int GetMoney() {
		return money;
	}

	void Start() {

		ui.OnMoneyChanged();
		ui.OnPotsChanged();
		ui.OnCrabChanged();
	}

	void Update() {
		if (!gameController.IsGameOver() && Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			gridTracker.PlaceOrRemovePot(mouseWorldPos, HasPotsLeft(), ThrowPot, AddPot);
		}
	}

	public bool HasPotsLeft() {
		return pots > 0;
	}

	internal void AddPot() {
		pots++;
		ui.OnPotsChanged();
	}

	internal void ThrowPot() {
		if (pots > 0) {
			pots--;
			ui.OnPotsChanged();
		}
	}

	void AddCrab(int crab) {
		if (crab < 0)
			crab = 0;

		Crab += crab;
		ui.OnCrabChanged();

		money += crab;
		ui.OnMoneyChanged();
	}

	public void OnGoClicked() {
		int crabBefore = Crab;
		gridTracker.LiftAllPots(AddPot, AddCrab);

		money -= gameController.TripCost;
		ui.OnMoneyChanged();

		gameController.OnAllPotsLifted(Crab - crabBefore, money);
	}
}
