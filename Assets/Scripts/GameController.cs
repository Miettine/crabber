using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{

	[SerializeField]
	private int numberOfRounds = 7;

	public int NumberOfRounds { get => numberOfRounds; }

	[SerializeField]
	private int tripCostIncrease = 3;
	[SerializeField]
	bool inDevelopment = true;

	[SerializeField]
	private int tripCost = 10;
	public int TripCost { get { return tripCost; } }

	public bool InDevelopment { get { return inDevelopment; } }

	UIController Ui { get; set; }
	public int TripCostIncrease { get => tripCostIncrease; }

	private bool gameOver = false;
	public bool WonTheGame { get; private set; } = false;
	public int CurrentRound { get; set; } = 1;

	void Awake() {
		Ui = UIController.GetInstance();
	}

	private void IncreaseTripCost(int increase) {
		tripCost += increase;
		Ui.OnTripCostChanged();
	}

	public int GetTripCost() {
		return tripCost;
	}

	public void RestartGame() {
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	internal void OnAllPotsLifted(int crabHaul, int currentMoney) {
		OnRoundOver(crabHaul, currentMoney);
	}

	internal void OnRoundOver(int roundCrabHaul, int currentMoney) {

		if (IsLastRound(CurrentRound)) {
			Ui.OnRoundOver(roundCrabHaul);
			GameOver(true);
		} else if (currentMoney < tripCost) {
			CurrentRound++;
			IncreaseTripCost(tripCostIncrease);
			Ui.OnRoundOver(roundCrabHaul);
			GameOver(false);
		} else {
			CurrentRound++;
			if (!IsLastRound(CurrentRound)) { 
				IncreaseTripCost(tripCostIncrease);
			}
			Ui.OnRoundOver(roundCrabHaul);
		}	
	}

	internal bool IsGameOver() {
		return gameOver;
	}

	bool IsLastRound(int currentRound) {
		return currentRound == numberOfRounds;
	}

	public void GameOver(bool wonTheGame) {
		gameOver = true;
		WonTheGame = wonTheGame;
		Ui.OnGameOver();
	}

	internal bool IsLastRound() {
		return IsLastRound(CurrentRound);
	}
}
