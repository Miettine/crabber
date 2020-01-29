using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : Tile
{
	public int Crab { get; set; }
	public void DebugGetCrab (){
		Debug.Log(Crab);
	}
}
