using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Deals with the game's current state and the game's progression.
/// </summary>
public class GameController : Singleton<GameController>
{
	[SerializeField]
	DifficultyLevel difficulty;

	public int NumberOfRounds { get; private set; }


	[SerializeField]
	bool inDevelopment = true;

	public int TripCost { get; private set; }

	public bool InDevelopment { get { return inDevelopment; } }

	UIController ui;
	public int TripCostIncrease { get; private set; }

	private bool gameOver = false;
	public bool WonTheGame { get; private set; } = false;
	public int CurrentRound { get; private set; } = 1;
	public DifficultyLevel Difficulty { get => difficulty; }

	SwarmController swarmController;
	void Awake() {
		swarmController = SwarmController.GetInstance();
		ui = UIController.GetInstance();
	}

	void Start() {
		/**
		* The trip cost on the first round is equal to the amount it increases on further rounds.
		*/
		TripCost = difficulty.TripCostIncrease;
		TripCostIncrease = difficulty.TripCostIncrease;
		NumberOfRounds = difficulty.NumberOfRounds;

		ui.OnTripCostChanged();
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
			IncreaseTripCost(TripCostIncrease);
			ui.OnRoundOver(roundCrabHaul);
			GameOver(false);
		} else {
			CurrentRound++;
			if (!IsLastRound(CurrentRound)) { 
				IncreaseTripCost(TripCostIncrease);
			}
			ui.OnRoundOver(roundCrabHaul);
		}	
	}

	internal bool IsGameOver() {
		return gameOver;
	}

	bool IsLastRound(int currentRound) {
		return currentRound == NumberOfRounds;
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

	internal void ReturnToMenu() {
		SceneManager.LoadScene("Scene select");
	}
}
