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

	public static Vector3Int OffsetToCubic(Vector3Int offsetPoint){
		Vector3Int cubicVector = OffsetToAxial(offsetPoint);
		cubicVector.z = CalculateCubicZ(cubicVector);
		
		return cubicVector;
	}

	public static Vector3Int CubicToOffset(Vector3Int cubicPoint){
		return new Vector3Int( cubicPoint.x + (cubicPoint.y / 2), cubicPoint.y, 0);
	}

	/// <summary> Calculates the axial coordinates from a Unity tile-system offset coordinate
	/// Taken from https://gamedevelopment.tutsplus.com/tutorials/introduction-to-axial-coordinates-for-hexagonal-tile-based-games--cms-28820
	/// </summary> 
	private static Vector3Int OffsetToAxial(Vector3Int offsetPoint){
		offsetPoint.x = offsetPoint.x-(offsetPoint.y/2);
		return offsetPoint;
	}

	/// <summary> Calculates the cubic Z-coordinate from an axial coordinate
	/// Taken from https://gamedevelopment.tutsplus.com/tutorials/introduction-to-axial-coordinates-for-hexagonal-tile-based-games--cms-28820
	/// </summary> 
	private static int CalculateCubicZ(Vector3Int axialPoint){
		return -axialPoint.x-axialPoint.y;
	}
}