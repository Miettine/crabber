using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	const string PotsLeftTextGameObjectName = "PotsLeftText";

	Text potsLeftText;

	[SerializeField]
	private int startPots = 7;

	private int pots;
	void Awake() {
		pots = startPots;
		potsLeftText = GameObject.Find(PotsLeftTextGameObjectName).GetComponent<Text>();
	}


	// Start is called before the first frame update
	void Start()
	{
		UpdatePotsLeftText();
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
