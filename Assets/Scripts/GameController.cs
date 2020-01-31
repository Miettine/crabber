using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	
	const string GridTrackerName = "GridTracker";
	const string GameControllerGameObjectName = "GameController";

	GridTracker gridTracker;
	
	PlayerController playerController;

	private void Awake() {
		gridTracker = GameObject.Find(GridTrackerName).GetComponent<GridTracker>();
		playerController = PlayerController.GetPlayerController();
	}


	// Update is called once per frame
	void Update()
	{
		
	}

	public static GameController GetGameController() {
		return GameObject.Find(GameControllerGameObjectName).GetComponent<GameController>();
	}

	internal void OnPlayersPotsChanged() {
		//goButton.enabled = !playerController.HasPotsLeft();
	}
}
