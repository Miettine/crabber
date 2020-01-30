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

	internal void DebugSwarms() {
		gridTracker.PlaceUnderWaterTile(new Vector3Int(0, 0, 0), 0);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(0, 1, 0), 1);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(0, 2, 0), 1);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(0, 3, 0), 1);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(0, -1, 0), 2);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(0, -2, 0), 2);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(0, -3, 0), 2);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(1, 0, 0), 3);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(2, 0, 0), 3);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(3, 0, 0), 3);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(-1, 0, 0), 4);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(-2, 0, 0), 4);
		gridTracker.PlaceUnderWaterTile(new Vector3Int(-3, 0, 0), 4);
	}

	internal void setGridTracker(GridTracker gridTracker) {
		this.gridTracker = gridTracker;
	}
}
