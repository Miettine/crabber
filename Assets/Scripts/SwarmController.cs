using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour
{

	GridTracker gridTracker;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	internal void PlaceDebugSwarms() {
		gridTracker.PlaceUnderWaterTile(new Vector2Int(0, 0), 7);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(0, 1), 1);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(0, 2), 1);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(0, 3), 1);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(0, -1), 2);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(0, -2), 2);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(0, -3), 2);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(1, 0), 3);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(2, 0), 3);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(3, 0), 3);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(-1, 0), 4);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(-2, 0), 4);
		gridTracker.PlaceUnderWaterTile(new Vector2Int(-3, 0), 4);

		gridTracker.RevealUnderWaterTile(new Vector2Int(0, 0));

		gridTracker.RevealUnderWaterTile(new Vector2Int(-3, 0));
		gridTracker.RevealUnderWaterTile(new Vector2Int(3, 3));

	}

	internal void SetGridTracker(GridTracker gridTracker) {
		this.gridTracker = gridTracker;
	}
}
