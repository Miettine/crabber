using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	const string GoButtonName = "GoButton";
	const string GridTrackerName = "GridTracker";
	const string GameControllerGameObjectName = "GameController";

	GridTracker gridTracker;
	Button goButton;
	PlayerController playerController;

	private void Awake() {
		gridTracker = GameObject.Find(GridTrackerName).GetComponent<GridTracker>();
		goButton = GameObject.Find(GoButtonName).GetComponent<Button>();
		playerController = PlayerController.GetPlayerController();
	}
	void Start()
	{
		goButton.onClick.AddListener( () => OnGoClicked() );
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public static GameController GetGameController() {
		return GameObject.Find(GameControllerGameObjectName).GetComponent<GameController>();
	}

	public void OnGoClicked() {
		Debug.Log("Going! :D");

		gridTracker.GetAllPotTiles();
		//gridTracker.removeAllPotMarkers();
		//playerController.resetAll
	}

	internal void OnPlayersPotsChanged() {
		//goButton.enabled = !playerController.HasPotsLeft();
	}
}
