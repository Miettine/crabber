using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubicCrabGrid : Dictionary<Vector3Int, int> {
	public void AddCrab(int amount, Vector3Int location){
		if (TryGetValue(location, out int crabAmount)) {
			this[location] += amount;
		} else {
			Add(location, amount);
		}
	}

	public void AddCrab(int amount, params Vector3Int[] locations){
		foreach (Vector3Int location in locations){
			AddCrab(amount, location);
		}
	}

	public bool IsAcceptableSwarmPlace(Vector3Int swarmCenter){
		if (TryGetValue(swarmCenter, out int crabAmount)) {
			if (crabAmount > 0){
				return false;
			}
		}

		return true;
	}

	/**
	* There exist algorithms that allow me to draw a ring of hexagons 
	* around a certain center point of a given radius. Developing the 
	* algorithms would take time and I want to get this project over 
	* with, so I am making the swarms by hand.
	*/
	public void AddSwarm(Vector3Int center, int[] populationConcentration){

		AddCrab(populationConcentration[0], center);

		if (populationConcentration.Length > 1) {
			var p1 = new Vector3Int(center.x, center.y + 1, center.z - 1);
			var p2 = new Vector3Int(center.x + 1, center.y, center.z - 1);
			var p3 = new Vector3Int(center.x + 1, center.y - 1, center.z);
			var p4 = new Vector3Int(center.x, center.y - 1, center.z + 1);
			var p5 = new Vector3Int(center.x - 1, center.y, center.z + 1);
			var p6 = new Vector3Int(center.x - 1, center.y + 1, center.z);

			AddCrab(populationConcentration[1], p1, p2, p3, p4, p5, p6);
		}

		if (populationConcentration.Length > 2) {
			var p7 = new Vector3Int(center.x, center.y + 2, center.z - 2);

			var p8 = new Vector3Int(center.x + 1, center.y + 1, center.z - 2);
			var p9 = new Vector3Int(center.x + 2, center.y, center.z -2);

			var p10 = new Vector3Int(center.x + 2, center.y - 1, center.z - 1);
			var p11 = new Vector3Int(center.x + 2, center.y - 2, center.z);

			var p12 = new Vector3Int(center.x + 1, center.y - 2, center.z + 1);
			var p13 = new Vector3Int(center.x, center.y - 2, center.z + 2);

			var p14 = new Vector3Int(center.x - 1, center.y - 1, center.z + 2);
			var p15 = new Vector3Int(center.x - 2, center.y, center.z + 2);

			var p16 = new Vector3Int(center.x - 2, center.y + 1, center.z + 1);
			var p17 = new Vector3Int(center.x - 2, center.y + 2, center.z);

			var p18 = new Vector3Int(center.x - 1, center.y + 2, center.z -1);

			AddCrab(populationConcentration[2], p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18);
		}
	}

	public static Vector3Int OffsetToCubic(Vector3Int offsetPoint){
		//Vector3Int cubicVector = OffsetToAxial(offsetPoint);
		//cubicVector.z = CalculateCubicZ(cubicVector);
		
		return EvenQToCube(offsetPoint);
	}

	public static Vector3Int CubicToOffset(Vector3Int cubicPoint){
		return CubeToEvenQ(cubicPoint);
	}

	/// <summary>
	/// Courtesy of redblobgames.com
	/// </summary> 
	private static Vector3Int CubeToEvenQ(Vector3Int cube){
		var col = cube.x;
		var row = cube.z + (cube.x + (cube.x&1)) / 2;
		return new Vector3Int(-row, col, 0);
	}

	

	/// <summary>
	/// Courtesy of redblobgames.com
	/// </summary> 
	private static Vector3Int EvenQToCube(Vector3Int hex){
		/**
		var x = hex.col;
		var z = hex.row - (hex.col + (hex.col&1)) / 2;
		var y = -x-z;
		*/
		
		var x = hex.y;
		var z = (-hex.x) - (hex.y + (hex.y&1)) / 2;
		var y = -x-z;
		
		return new Vector3Int(x, y, z);
	}
}