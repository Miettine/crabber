using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour
{

	const string GameControllerGameObjectName = "GameController";
	GridTracker gridTracker;

	Dictionary<Vector3Int, int> crabPopulation;

	[SerializeField]
	int numberOfSwarms = 3;

	// Start is called before the first frame update
	void Start()
	{
		crabPopulation = GetSwarms();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public int GetCrab(Vector3Int location) {
		if (crabPopulation.TryGetValue(location, out int crabAmount)) {
			Debug.Log("Area " + location + " contained "+ crabAmount+" crab");

			if (crabAmount > 1)
				crabPopulation[location] = 1;
			else if (crabAmount >= 1)
				crabPopulation[location] = 0;

			return crabAmount;
		}
		Debug.Log("Area "+location+ " contained no crab");
		return 0;
	}

	Dictionary<Vector3Int, int> GetSwarms() {
		var swarm = new Dictionary<Vector3Int, int>();

		AddSwarm(swarm, Vector3Int.zero, 6, 3);

		return swarm;
	}

	/**
	 * The first argument is the center of the population, the next argument is one tile from the center, the third argument is two tiles away etc.
	 * */
	void AddSwarm(Dictionary<Vector3Int, int> swarmDictionary, Vector3Int swarmCenter, params int[] populationConcentration) {

		swarmDictionary.Add(new Vector3Int(0, 0, 0), populationConcentration[0]);

		swarmDictionary.Add(new Vector3Int(0, 1, 0), populationConcentration[1]);
		swarmDictionary.Add(new Vector3Int(0, -1, 0), populationConcentration[1]);

		swarmDictionary.Add(new Vector3Int(1, 0, 0), populationConcentration[1]);
		swarmDictionary.Add(new Vector3Int(-1, 0, 0), populationConcentration[1]);

		swarmDictionary.Add(new Vector3Int(-1, 1, 0), populationConcentration[1]);
		swarmDictionary.Add(new Vector3Int(-1, -1, 0), populationConcentration[1]);
	}

	Dictionary<Vector3Int, int> GetDebugSwarms() {

		var debugSwarm = new Dictionary<Vector3Int, int>();

		debugSwarm.Add(new Vector3Int(0, 0, 0), 9);
		debugSwarm.Add(new Vector3Int(0, 1, 0), 3);
		debugSwarm.Add(new Vector3Int(0, 2, 0), 2);
		debugSwarm.Add(new Vector3Int(0, 3, 0), 1);
		debugSwarm.Add(new Vector3Int(1, 0, 0), 6);
		debugSwarm.Add(new Vector3Int(2, 0, 0), 5);
		debugSwarm.Add(new Vector3Int(3, 0, 0), 4);

		return debugSwarm;
	}

	internal void SetGridTracker(GridTracker gridTracker) {
		this.gridTracker = gridTracker;
	}
}
