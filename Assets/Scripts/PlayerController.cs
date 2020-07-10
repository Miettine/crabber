using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
	GridTracker gridTracker;
	GameController gameController;

	UIController Ui { get; set; }
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

	public int Pots { get => pots; private set => pots = value;}

	void Awake() {
		gridTracker = GridTracker.GetInstance();
		gameController = GameController.GetInstance();
		Ui = UIController.GetInstance();
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

		Ui.OnMoneyChanged();
		Ui.OnPotsChanged();
		Ui.OnCrabChanged();
	}

	void Update() {
		if (!gameController.IsGameOver() && Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			gridTracker.PlaceOrRemovePot(mouseWorldPos, HasPotsLeft(), ThrowPot, AddPot);
		}
	}

	public bool HasPotsLeft() {
		return Pots > 0;
	}

	internal void AddPot() {
		Pots++;
		Ui.OnPotsChanged();
	}

	internal void ThrowPot() {
		if (Pots > 0) {
			Pots--;
			Ui.OnPotsChanged();
		}
	}

	void AddCrab(int crab) {
		if (crab < 0)
			crab = 0;

		Crab += crab;
		Ui.OnCrabChanged();

		money += crab;
		Ui.OnMoneyChanged();
	}

	public void OnGoClicked() {
		int crabBefore = Crab;
		gridTracker.LiftAllPots(AddPot, AddCrab);

		money -= gameController.TripCost;
		Ui.OnMoneyChanged();

		gameController.OnAllPotsLifted(Crab - crabBefore, money);
	}
}
