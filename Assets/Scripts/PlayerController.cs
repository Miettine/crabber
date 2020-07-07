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
	[SerializeField]
	private int money = 30;

	public int Pots { get => pots; private set { 
			pots = value;
			Ui.OnPotsChanged();
		}
	}

	void Awake() {
		gridTracker = GridTracker.GetInstance();
		gameController = GameController.GetInstance();
		Ui = UIController.GetInstance();
	}

	public int GetMoney() {
		return money;
	}

	void Start()
	{
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
	}

	internal void ThrowPot() {
		if (Pots > 0) 
			Pots--;
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

		money -= gameController.GetTripCost();
		Ui.OnMoneyChanged();

		gameController.OnAllPotsLifted(Crab - crabBefore, money);
	}
}
