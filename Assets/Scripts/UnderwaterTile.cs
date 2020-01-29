using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnderwaterTile : Tile
{
	public int Crab { get; set; }
	public void DebugPrintCrab (){
		Debug.Log(Crab + " crab!");
	}
}
