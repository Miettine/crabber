using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour {

	const string GameControllerGameObjectName = "GameController";
	GridTracker gridTracker;

	Dictionary<Vector3Int, int> crabPopulation;

	[SerializeField]
	int numberOfSwarms = 3;

	// Start is called before the first frame update
	void Start() {
		crabPopulation = GetSwarms();
	}
	
	public int GetCrab(Vector3Int location) {
		if (crabPopulation.TryGetValue(location, out int crabAmount)) {
			Debug.Log("Area " + location + " contained " + crabAmount + " crab");

			if (crabAmount > 1)
				crabPopulation[location] = 1;
			else if (crabAmount >= 1)
				crabPopulation[location] = 0;

			return crabAmount;
		}
		Debug.Log("Area " + location + " contained no crab");
		return 0;
	}

	Dictionary<Vector3Int, int> GetSwarms() {
		var swarm = new Dictionary<Vector3Int, int>();

		AddSwarm(swarm, Coord(-4, 2), 9, 3, 1);
		AddSwarm(swarm, Coord(5, 0), 9, 3, 1);
		AddSwarm(swarm, Coord(-1, -1), 9, 3, 1);

		//AddSwarm(swarm, new Vector3Int(0, -3, 0), 6, 3);

		return swarm;
	}

	Vector3Int Coord(int x, int y) {
		return new Vector3Int(y, x, 0);
	}

	/**
	 * The first argument is the center of the population, the next argument is one tile from the center, the third argument is two tiles away etc.
	 * */
	void AddSwarm(Dictionary<Vector3Int, int> swarmDictionary, Vector3Int swarmCenter, params int[] populationConcentration) {

		AddCrab(swarmDictionary, swarmCenter, populationConcentration[0]);

		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x, swarmCenter.y + 1, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x, swarmCenter.y - 1, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x + 1, swarmCenter.y, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y + 1, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y - 1, 0), populationConcentration[1]);

		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x, swarmCenter.y + 2, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x, swarmCenter.y - 2, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x + 2, swarmCenter.y, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 2, swarmCenter.y, 0), populationConcentration[2]);

		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x + 1, swarmCenter.y - 1, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 2, swarmCenter.y + 1, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x + 1, swarmCenter.y + 2, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 2, swarmCenter.y - 1, 0), populationConcentration[2]);

		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x + 1, swarmCenter.y + 1, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x + 1, swarmCenter.y - 2, 0), populationConcentration[2]);

		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y - 2, 0), populationConcentration[2]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y + 2, 0), populationConcentration[2]);
	}
	
	void AddCrab(Dictionary<Vector3Int, int> swarmDictionary, Vector3Int location, int amount){
		if (swarmDictionary.TryGetValue(location, out int crabAmount)) {
			swarmDictionary[location] += amount;
		} else {
			swarmDictionary.Add(location, amount);
		}
		
	}

	internal void SetGridTracker(GridTracker gridTracker) {
		this.gridTracker = gridTracker;
	}
}
