using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	const string GridTrackerGameObjectName = "GridTracker";
	const string PotsLeftTextGameObjectName = "PotsLeftText";

	GridTracker gridTracker;
	Text potsLeftText;

	public delegate void AddPotDelegate();
	public delegate void ThrowPotDelegate();

	[SerializeField]
	private int startPots = 7;

	private int pots;
	void Awake() {
		pots = startPots;
		gridTracker = GameObject.Find(GridTrackerGameObjectName).GetComponent<GridTracker>();
		potsLeftText = GameObject.Find(PotsLeftTextGameObjectName).GetComponent<Text>();
	}

	void Start()
	{
		UpdatePotsLeftText();
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
		UpdatePotsLeftText();
	}
	void UpdatePotsLeftText() {
		potsLeftText.text = "Pots left: " + pots;
	}
	internal void ThrowPot() {
		if (pots > 0) 
			pots--;
		UpdatePotsLeftText();
	}
}
