using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private int startPots = 7;

	private int pots;
	void Awake() {
		pots = startPots;
	}


	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public bool hasPotsLeft() {
		return pots > 0;
	}

	internal void addPot() {
		pots++;
	}

	internal void throwPot() {
		if (pots > 0)
			pots--;
	}
}
