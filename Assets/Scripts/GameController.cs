using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Deals with the game's current state and the game's progression.
/// </summary>
public class GameController : Singleton<GameController>
{

	[SerializeField]
	private int numberOfRounds = 7;

	public int NumberOfRounds { get => numberOfRounds; }

	[SerializeField]
	private int tripCostIncrease = 3;
	[SerializeField]
	bool inDevelopment = true;

	public int TripCost { get; private set; }

	public bool InDevelopment { get { return inDevelopment; } }

	UIController ui;
	public int TripCostIncrease { get => tripCostIncrease; }

	private bool gameOver = false;
	public bool WonTheGame { get; private set; } = false;
	public int CurrentRound { get; private set; } = 1;

	SwarmController swarmController;
	void Awake() {
		swarmController = SwarmController.GetInstance();
		ui = UIController.GetInstance();

		/**
		* Trip cost needs to be initialized at awake because 
		* the player controller needs it during the Start-function.
		*/
		TripCost = tripCostIncrease;
	}

	private void IncreaseTripCost(int increase) {
		TripCost += increase;
		ui.OnTripCostChanged();
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
			ui.OnRoundOver(roundCrabHaul);
			GameOver(true);
		} else if (currentMoney < TripCost) {
			CurrentRound++;
			IncreaseTripCost(tripCostIncrease);
			ui.OnRoundOver(roundCrabHaul);
			GameOver(false);
		} else {
			CurrentRound++;
			if (!IsLastRound(CurrentRound)) { 
				IncreaseTripCost(tripCostIncrease);
			}
			ui.OnRoundOver(roundCrabHaul);
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
		swarmController.RevealAllSwarms(this);
		ui.OnGameOver();
	}

	internal bool IsLastRound() {
		return IsLastRound(CurrentRound);
	}
}
