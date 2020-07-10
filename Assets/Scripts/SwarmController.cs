using System.Collections.Generic;
using UnityEngine;

public class SwarmController : Singleton<SwarmController> {

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
	int numberOfSwarms = 7;

	/// <summary>
	/// Setting this to a small number has the possibility that the swarms could overlap.
	/// </summary>
	[SerializeField]
	int emptySpaceBetweenCenters = 2;

	/// <summary>
	/// Describes the concentration of crab in each crab swarm. The first number is the crab in the center of the population, 
	/// the second number is the ring around the center, and the third number is the number on the outer ring.
	/// </summary>
	[SerializeField]
	int[] populationConcentration = { 6, 2, 1 };

	/// <summary>
	/// The maximum number of attempts that a random placement for each swarm is attempted at the start of the game.
	/// </summary>
	[SerializeField]
	int maximumAttempts = 100;

	// Start is called before the first frame update
	void Start() {
		CubicCrabGrid startPopulation = GetSwarms(populationConcentration, numberOfSwarms, emptySpaceBetweenCenters);
		crabPopulation = startPopulation;	
		
		//A copy is made of the start population.
		originalCrabPopulation = new CubicCrabGrid(startPopulation);
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

			bool acceptablePlacement = false;
			int attemptNumber = 0;

			while (!acceptablePlacement) {
				attemptNumber++;
				if (!crabGrid.IsAcceptableSwarmPlace(newSwarmPlace, emptySpaceAroundCenters) || !gridTracker.SwarmPlacementIsWithinPlayArea(newSwarmPlace)) {
					newSwarmPlace = gridTracker.GetRandomSwarmPlacementInCubic();

					if (attemptNumber >= maximumAttempts) {
						Debug.LogWarningFormat("Attempted {0} times and couldn't find placement for swarm number {1}", attemptNumber, i);

						acceptablePlacement = true;
					}
					
					continue;
				} else {
					acceptablePlacement = true;
				}
			}

			crabGrid.AddSwarm(newSwarmPlace, populationConcentration);
		}
		return crabGrid;
	}

	/// <summary>
	/// Reveals all swarms at the end of the game. The tiles that the player fished from during the game will be
	/// shown white. The tiles that they didn't fish from will be gray.
	/// </summary>
	/// <param name="gameController">Once during this project I accidentally called this function from UIController. 
	/// To be more prudent I am making this function so that it can be called only from GameController</param>
	public void RevealAllSwarms(GameController gameController) {
		if (gameController == null) {
			throw new System.UnauthorizedAccessException("Only GameController can call this function");
		}

		if (!gameController.IsGameOver()) {
			throw new System.UnauthorizedAccessException("Game hasn't ended yet");
		}

		/**
		 * First I go through all tiles that have the number zero in them and make them white.
		 * The original crab population doesn't know where the player tried to fish from but didn't get any crab.
		 * */
		gridTracker.SetZeroTilesWhite();

		foreach (KeyValuePair<Vector3Int, int> swarm in originalCrabPopulation) {
			if (!gridTracker.HasWaterTile(swarm.Key)) {
				continue; //I only want to reveal the crab swarms in water areas.
			}

			if (gridTracker.HasNumberTile(swarm.Key)) {
				//If this is a tile where the player has fished from during this game, we make the tile white to highlight it.
				gridTracker.SetNumberTileInCubic(swarm.Key, swarm.Value, Color.white);
			} else {
				//If the player didn't fish from this file during this game, the tile will be gray.
				gridTracker.SetNumberTileInCubic(swarm.Key, swarm.Value);
			}
		}


	}

	internal void SetGridTracker(GridTracker gridTracker) {
		this.gridTracker = gridTracker;
	}
}
