using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	
	const string GridTrackerName = "GridTracker";
	const string GameControllerGameObjectName = "GameController";
	const string RoundTextGameObjectName = "RoundsText";

	//GridTracker gridTracker;
	
	//PlayerController playerController;

	Text roundText;

	[SerializeField]
	private int numberOfRounds = 7;

	int currentRound = 1;

	private void Awake() {
		//gridTracker = GameObject.Find(GridTrackerName).GetComponent<GridTracker>();
		//playerController = PlayerController.GetPlayerController();
		roundText = GameObject.Find(RoundTextGameObjectName).GetComponent<Text>();
	}

	private void Start() {
		UpdateRoundsText();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public static GameController GetGameController() {
		return GameObject.Find(GameControllerGameObjectName).GetComponent<GameController>();
	}
	internal void OnAllPotsLifted() {
		OnRoundOver();
	}

	internal void OnRoundOver() {
		currentRound++;
		UpdateRoundsText();	
	}
	void UpdateRoundsText() {
		roundText.text = "Round: " + currentRound + "/" + numberOfRounds;
	}

	internal void OnPlayersPotsChanged() {
		
	}
}
