using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour {

	const string SwarmControllerGameObjectName = "SwarmController";
	GridTracker gridTracker;

	Dictionary<Vector3Int, int> crabPopulation;

	System.Random randomizer = new System.Random();

	[SerializeField]
	int XMinCoordinate = -3;
	[SerializeField]
	int XMaxCoordinate = 3;
	[SerializeField]
	int YMinCoordinate = -2;
	[SerializeField]
	int YMaxCoordinate = 2;

	[SerializeField]
	int numberOfSwarms = 3;

	[SerializeField]
	int[] populationConcentration = { 6, 2, 1 };

	// Start is called before the first frame update
	void Start() {
		crabPopulation = GetSwarms(populationConcentration);
	}

	public static SwarmController GetSwarmController() {
		return GameObject.Find(SwarmControllerGameObjectName).GetComponent<SwarmController>();
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

	Dictionary<Vector3Int, int> GetSwarms(int[] populationConcentration) {
		var swarm = new Dictionary<Vector3Int, int>();
		var takenPlaces = new List<Vector3Int>();


		for (int i = 1; i <= numberOfSwarms; i++) {

			Vector3Int newSwarmPlace = GetRandomizedVector3Int();

			var notAllowedPlacement = true;
			while (notAllowedPlacement) {
				newSwarmPlace = GetRandomizedVector3Int();
				if (takenPlaces.Contains(newSwarmPlace))
					continue;
				else 
					notAllowedPlacement = false;
			}

			AddSwarm(swarm, newSwarmPlace, populationConcentration);

			takenPlaces.Add(newSwarmPlace);
			Debug.Log("Placed swarm at " + newSwarmPlace);
		}
		return swarm;
	}
	Vector3Int GetRandomizedVector3Int() {
		return Coord(randomizer.Next(XMinCoordinate, XMaxCoordinate), randomizer.Next(YMinCoordinate, YMaxCoordinate));
	}

	Vector3Int Coord(int x, int y) {
		return new Vector3Int(y, x, 0);
	}

	internal void RevealAllSwarms() {
		foreach (KeyValuePair<Vector3Int, int> swarm in crabPopulation) {
			gridTracker.SetNumberTile(swarm.Key, swarm.Value);
		}
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

		if (populationConcentration.Length > 2) {
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
