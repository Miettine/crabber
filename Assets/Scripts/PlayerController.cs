using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	const string GridTrackerGameObjectName = "GridTracker";
	const string PotsLeftTextGameObjectName = "PotsLeftText";
	const string PlayerControllerGameObjectName = "PlayerController";
	const string GoButtonName = "GoButton";

	Button goButton;
	GridTracker gridTracker;
	Text potsLeftText;
	GameController gameController;

	public delegate void AddPotDelegate();
	public delegate void ThrowPotDelegate();
	public delegate void AddCrabDelegate(int crab);

	[SerializeField]
	private int startPots = 7;

	int pots;
	int crab = 0;
	void Awake() {
		pots = startPots;
		gridTracker = GameObject.Find(GridTrackerGameObjectName).GetComponent<GridTracker>();
		potsLeftText = GameObject.Find(PotsLeftTextGameObjectName).GetComponent<Text>();
		gameController = GameController.GetGameController();
		goButton = GameObject.Find(GoButtonName).GetComponent<Button>();
	}

	internal static PlayerController GetPlayerController() {
		return GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();
	}

	void Start()
	{
		OnPotsChanged();
		goButton.onClick.AddListener(() => OnGoClicked());
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			gridTracker.PlaceOrRemovePot(mouseWorldPos, HasPotsLeft(), ThrowPot, AddPot);

		}
	}

	public bool HasPotsLeft() {
		return pots > 0;
	}

	internal void AddPot() {
		pots++;
		OnPotsChanged();
	}
	void OnPotsChanged() {
		potsLeftText.text = "Pots left: " + pots;
		gameController.OnPlayersPotsChanged();

	}
	internal void ThrowPot() {
		if (pots > 0) 
			pots--;
		OnPotsChanged();
	}

	void AddCrab(int crab) {
		if (crab < 0)
			crab = 0;
		this.crab += crab;
	}

	public void OnGoClicked() {
		Debug.Log("Going! :D");

		gridTracker.GetAllPotTiles(AddPot, AddCrab);
		//gridTracker.removeAllPotMarkers();
		//playerController.resetAll
	}
}
