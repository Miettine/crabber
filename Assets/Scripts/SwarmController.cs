﻿using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour {

	const string SwarmControllerGameObjectName = "SwarmController";
	GridTracker gridTracker;

	/// <summary>
	/// The current layout of the crab population. The amount of crab changes during the game. 
	/// Athe player fishes crab, the crab are substracted from this CubicCrabGrid
	/// </summary>
	CubicCrabGrid crabPopulation;

	/// <summary>
	/// This crab population is made at the start and never modified. This is used at the end of the game 
	/// when revealing to the player the original layout of the crab swarms.
	/// </summary>
	CubicCrabGrid originalCrabPopulation;

	[SerializeField]
	int numberOfSwarms = 3;

	/// <summary>
	/// Describes the concentration of crab in each crab swarm. The first number is the crab in the center of the population, 
	/// the second number is the ring around the center, and the third number is the number on the outer ring.
	/// </summary>
	[SerializeField]
	int[] populationConcentration = { 6, 2, 1 };

	// Start is called before the first frame update
	void Start() {
		CubicCrabGrid startPopulation = GetSwarms(populationConcentration, numberOfSwarms);
		crabPopulation = startPopulation;	
		
		//A copy is made of the start population.
		originalCrabPopulation = new CubicCrabGrid(startPopulation);
	}

	public static SwarmController GetSwarmController() {
		return GameObject.Find(SwarmControllerGameObjectName).GetComponent<SwarmController>();
	}
	public int GetCrab(Vector3Int locationInOffsetCoord) {

		Vector3Int locationInCubic = CubicCrabGrid.OffsetToCubic(locationInOffsetCoord);
		
		if (crabPopulation.TryGetValue(locationInCubic, out int crabAmount)) {
			Debug.Log("Area " + locationInCubic + " contained " + crabAmount + " crab");

			if (crabAmount > 1)
				crabPopulation[locationInCubic] = 1;
			else if (crabAmount >= 1)
				crabPopulation[locationInCubic] = 0;

			return crabAmount;
		}
		Debug.Log("Area " + locationInCubic + " contained no crab");
		return 0;
	}

	CubicCrabGrid GetSwarms(int[] populationConcentration, int number, int emptySpaceAroundCenters) {
		var crabGrid = new CubicCrabGrid();

		for (int i = 1; i <= number; i++) {

			Vector3Int newSwarmPlace = gridTracker.GetRandomSwarmPlacementInCubic();

			bool notAllowedPlacement = true;

			while (notAllowedPlacement) {
				
				if (!crabGrid.IsAcceptableSwarmPlace(newSwarmPlace, emptySpaceAroundCenters) || !gridTracker.SwarmPlacementIsWithinPlayArea(newSwarmPlace)) {
					newSwarmPlace = gridTracker.GetRandomSwarmPlacementInCubic();
					continue;
				} else {
					notAllowedPlacement = false;
				}
			}

			crabGrid.AddSwarm(newSwarmPlace, populationConcentration);
		}
		return crabGrid;
	}

	public void RevealAllSwarms() {
		foreach (KeyValuePair<Vector3Int, int> swarm in originalCrabPopulation) {
			gridTracker.SetNumberTileInCubic(swarm.Key, swarm.Value);
		}
	}

	internal void SetGridTracker(GridTracker gridTracker) {
		this.gridTracker = gridTracker;
	}
}
