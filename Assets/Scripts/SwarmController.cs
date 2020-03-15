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
		crabPopulation = GetDebugSwarms();
		//GetSwarms(populationConcentration, numberOfSwarms);
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

	Dictionary<Vector3Int, int> GetDebugSwarms() {
		var swarm = new Dictionary<Vector3Int, int>();

		AddSwarm(swarm, new Vector3Int(0, 0, 0), new int[]{9, 2} );
		AddSwarm(swarm, new Vector3Int(-1, -4, 0), new int[]{9, 2} );
		AddSwarm(swarm, new Vector3Int(1, 4, 0), new int[]{9, 2} );
		AddSwarm(swarm, new Vector3Int(3, 8, 0), new int[]{9, 2} );
		AddSwarm(swarm, new Vector3Int(-3, -8, 0), new int[]{9, 2} );
		return swarm;
	}

	Dictionary<Vector3Int, int> GetSwarms(int[] populationConcentration, int number) {
		var swarm = new Dictionary<Vector3Int, int>();
		var takenPlaces = new List<Vector3Int>();

		for (int i = 1; i <= number; i++) {

			Vector3Int newSwarmPlace = GetRandomizedVector3Int();

			bool notAllowedPlacement = true;

			while (notAllowedPlacement) {
				
				if (unacceptableSwarmPlace(takenPlaces, newSwarmPlace)){
					newSwarmPlace = GetRandomizedVector3Int();
					continue;
				} else {
					notAllowedPlacement = false;
				}
			}

			AddSwarm(swarm, newSwarmPlace, populationConcentration);

			takenPlaces.Add(newSwarmPlace);
			Debug.Log("Placed swarm at " + newSwarmPlace);
		}
		return swarm;
	}

	bool unacceptableSwarmPlace(List<Vector3Int> takenPlaces, Vector3Int newSwarmPlace){
		return takenPlaces.Contains(newSwarmPlace);
	}

	Vector3Int GetDebugVector3Int() {
		return new Vector3Int(0, 1, 0);
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

		int swarmCenterY = swarmCenter.y;
		int swarmCenterX = swarmCenter.x;
		AddCrab(swarmDictionary, swarmCenter, populationConcentration[0]);

		int location1x = swarmCenterX + 1;
		int location1y = swarmCenterY;
		AddCrab(swarmDictionary, new Vector3Int(location1x, location1y, 0), 1);

		int location2x = location1x - 1;
		int location2y = swarmCenterY + 1;
		AddCrab(swarmDictionary, new Vector3Int(location2x, location2y, 0), 2);

		int location3x = swarmCenterX - 1;
		int location3y = location2y;
		AddCrab(swarmDictionary, new Vector3Int(location3x, location3y, 0), 3);

		int location4x = location3x;
		int location4y = swarmCenterY;
		AddCrab(swarmDictionary, new Vector3Int(location4x, location4y, 0), 4);

		int location5x = location4x;
		int location5y = location4y - 1;
		AddCrab(swarmDictionary, new Vector3Int(location5x, location5y, 0), 5);
	
		int location6x = swarmCenterX;
		int location6y = location5y;
		AddCrab(swarmDictionary, new Vector3Int(location6x, location6y, 0), 6);

		/**
				AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x, swarmCenter.y + 1, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x, swarmCenter.y - 1, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x + 1, swarmCenter.y, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y + 1, 0), populationConcentration[1]);
		AddCrab(swarmDictionary, new Vector3Int(swarmCenter.x - 1, swarmCenter.y - 1, 0), populationConcentration[1]);
*/

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
