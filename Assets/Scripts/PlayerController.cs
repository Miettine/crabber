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

	GridTracker gridTracker;
	Text potsLeftText;
	GameController gameController;

	public delegate void AddPotDelegate();
	public delegate void ThrowPotDelegate();

	[SerializeField]
	private int startPots = 7;

	private int pots;
	void Awake() {
		pots = startPots;
		gridTracker = GameObject.Find(GridTrackerGameObjectName).GetComponent<GridTracker>();
		potsLeftText = GameObject.Find(PotsLeftTextGameObjectName).GetComponent<Text>();
		gameController = GameController.GetGameController();
	}

	internal static PlayerController GetPlayerController() {
		return GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();
	}

	void Start()
	{
		OnPotsChanged();
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
}
