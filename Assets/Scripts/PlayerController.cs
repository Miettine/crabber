using UnityEngine;
using UnityEngine.UI;

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
	/// Represents the number of pots that the player is capable of carrying on their boat. Pot is what a crab fishing trap is called.
	/// </summary>
	[SerializeField]
	private int pots = 6;

	int money = 30;

	public int GetPots() { 
		return pots; 
	}

	void Awake() {
		gridTracker = GridTracker.GetInstance();
		gameController = GameController.GetInstance();
		ui = UIController.GetInstance();
	}

	public int GetMoney() {
		return money;
	}

	void Start() {
		/**
		 * The following is a simplified equation of the summed together trip costs of the first three rounds.
		 * I want to guarantee the player to always be able to play the first three rounds 
		 * without losing. This is to lessen the player's frustration.
		 * */
		money = 3 * gameController.TripCost + 3 * gameController.TripCostIncrease;

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
