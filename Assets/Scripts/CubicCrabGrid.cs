using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubicCrabGrid : Dictionary<Vector3Int, int> {
	public void AddCrab(Vector3Int location, int amount){
		if (this.TryGetValue(location, out int crabAmount)) {
			this[location] += amount;
		} else {
			this.Add(location, amount);
		}
	}

	public void AddCrab(int amount, params Vector3Int[] locations){
		foreach (Vector3Int location in locations){
			AddCrab(location, amount);
		}
	}
	/**
	* There exists algorithms that allow me to draw a ring of hexagons.
	* However, developing the algorithms would take time and I want to 
	* get this project over with, so I am making the swarms by hand.
	*/
	public void AddSwarm(Vector3Int center, int amount){
		var p1 = new Vector3Int(center.x, center.y + 1, center.z - 1);
		var p2 = new Vector3Int(center.x + 1, center.y, center.z - 1);
		var p3 = new Vector3Int(center.x + 1, center.y - 1, center.z);
		var p4 = new Vector3Int(center.x, center.y - 1, center.z + 1);
		var p5 = new Vector3Int(center.x - 1, center.y, center.z + 1);
		var p6 = new Vector3Int(center.x - 1, center.y + 1, center.z);

		AddCrab(amount, p1, p2, p3, p4, p5, p6);
	}

	public static Vector3Int OffsetToCubic(Vector3Int offsetPoint){
		//Vector3Int cubicVector = OffsetToAxial(offsetPoint);
		//cubicVector.z = CalculateCubicZ(cubicVector);
		
		return EvenQToCube(offsetPoint);
	}

	public static Vector3Int CubicToOffset(Vector3Int cubicPoint){
		return CubeToEvenQ(cubicPoint);
	}

	/// <summary> Calculates the axial coordinates from a Unity tile-system offset coordinate
	/// Taken from https://gamedevelopment.tutsplus.com/tutorials/introduction-to-axial-coordinates-for-hexagonal-tile-based-games--cms-28820
	/// </summary> 
	private static Vector3Int OffsetToAxial(Vector3Int offsetPoint){
		offsetPoint.x = offsetPoint.x+(offsetPoint.y/2);
		return offsetPoint;
	}

	/// <summary> Calculates the cubic Z-coordinate from an axial coordinate
	/// Taken from https://gamedevelopment.tutsplus.com/tutorials/introduction-to-axial-coordinates-for-hexagonal-tile-based-games--cms-28820
	/// </summary> 
	private static int CalculateCubicZ(Vector3Int axialPoint){
		return -axialPoint.x-axialPoint.y;
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