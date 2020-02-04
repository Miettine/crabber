using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	
	//const string GridTrackerName = "GridTracker";
	const string GameControllerGameObjectName = "GameController";
	const string RoundTextGameObjectName = "RoundsText";
	const string LogTextGameObjectName = "LogText";

	Text roundText;
	Text logText;

	[SerializeField]
	private int numberOfRounds = 7;

	int currentRound = 1;

	private void Awake() {
		//gridTracker = GameObject.Find(GridTrackerName).GetComponent<GridTracker>();
		//playerController = PlayerController.GetPlayerController();
		roundText = GameObject.Find(RoundTextGameObjectName).GetComponent<Text>();
		logText = GameObject.Find(LogTextGameObjectName).GetComponent<Text>();
	}

	private void Start() {
		UpdateRoundsText(currentRound, numberOfRounds);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public static GameController GetGameController() {
		return GameObject.Find(GameControllerGameObjectName).GetComponent<GameController>();
	}
	internal void OnAllPotsLifted(int crabHaul) {
		OnRoundOver(crabHaul);

	}

	internal void OnRoundOver(int roundCrabHaul) {
		UpdateLogText(currentRound, roundCrabHaul);
		currentRound++;
		UpdateRoundsText(currentRound, numberOfRounds);

	}
	void UpdateRoundsText(int currentRound, int numberOfRounds) {
		roundText.text = "Round: " + currentRound + "/" + numberOfRounds;
	}
	
	void UpdateLogText(int round, int roundCrabHaul) {
		logText.text += string.Format("Round {0} hauled {1} crab", round, roundCrabHaul) + "\n";
	}


	internal void OnPlayersPotsChanged() {
		
	}
}
