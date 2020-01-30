using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UnderwaterTile : Tile {
	int crab = 0;
	public int Crab {
		get {
			return crab;
		}
		set {
			if (value < 0)
				crab = 0;
			crab = value;
		}
	}
	public void DebugPrintCrab() {
		Debug.Log(Crab + " crab!");
	}



}
