﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTracker : MonoBehaviour
{

	const string WaterTileName = "HexTilesetv3_5";
	const string PlacedPotTileName = "HexTilesetv3_41";
	const string TerrainGridName = "TerrainGrid";
	const string MarkerGridName = "MarkerGrid";
	const string UnderwaterGridName = "UnderwaterGrid";
	const string PlayerControllerGameObjectName = "PlayerController";
	const string SwarmControllerName = "SwarmController";

	Grid terrainGrid;
	Tilemap terrainTilemap;

	Grid markerGrid;
	Tilemap markerTilemap;

	Grid underwaterGrid;
	Tilemap underwaterTilemap;

	PlayerController playerController;

	SwarmController swarmController;

	//TODO: Get sprite with code
	[SerializeField]
	private Sprite potSprite;

	[SerializeField]
	private Sprite debugUnderwaterTileSprite;

	[SerializeField]
	private Sprite[] numberSprites;

	private void Awake() {
		terrainGrid = GameObject.Find(TerrainGridName).GetComponent<Grid>();
		terrainTilemap = terrainGrid.GetComponentInChildren<Tilemap>();

		markerGrid = GameObject.Find(MarkerGridName).GetComponent<Grid>();
		markerTilemap = markerGrid.GetComponentInChildren<Tilemap>();

		underwaterGrid = GameObject.Find(UnderwaterGridName).GetComponent<Grid>();
		underwaterTilemap = markerGrid.GetComponentInChildren<Tilemap>();

		swarmController = GameObject.Find(SwarmControllerName).GetComponent<SwarmController>();
		swarmController.setGridTracker(this);

		playerController = GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();

		if (potSprite == null)
			throw new Exception("ERROR: Failed to find potSprite");
	}

	internal List<Vector3Int> GetAllPotLocations() {
		throw new NotImplementedException();
	}

	void Start() {

		swarmController.DebugSwarms();

		//PlaceUnderWaterTile(new Vector3Int(0, 0, 2), 3);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			PlaceOrRemovePot(mouseWorldPos);

		}
	}
	public void PlaceUnderWaterTile(Vector3Int location, int crabAmount) {

		UnderwaterTile underwaterTile = ScriptableObject.CreateInstance<UnderwaterTile>();

		//underwaterTile.sprite = debugUnderwaterTileSprite;
		
		underwaterTile.Crab = crabAmount;

		underwaterTile.sprite = this.numberSprites[crabAmount];
		underwaterTilemap.SetTile(location, underwaterTile);
		underwaterTile.DebugPrintCrab();
	}

	void PlaceOrRemovePot(Vector3 worldPos) {
		Vector3Int terrainCoordinate = terrainGrid.WorldToCell(worldPos);
		var terrainTile = (Tile) terrainTilemap.GetTile(terrainCoordinate);

		bool terrainIsWaterTile = TileHasSpriteWithName(terrainTile, WaterTileName);

		Vector3Int markerCoordinate = markerGrid.WorldToCell(worldPos);
		var markerTile = (Tile) markerTilemap.GetTile(markerCoordinate);

		bool markerGridHasPlacedPotTile = TileHasSpriteWithName(markerTile, PlacedPotTileName);

		bool allowedToPlace = terrainIsWaterTile && !markerGridHasPlacedPotTile;
		bool allowedToRemove = terrainIsWaterTile && markerGridHasPlacedPotTile;
		Vector3Int location = markerCoordinate;

		if (allowedToPlace && playerController.hasPotsLeft()) {
			Tile tile = ScriptableObject.CreateInstance<Tile>();
			tile.sprite = potSprite;
			markerTilemap.SetTile(location, tile);

			playerController.throwPot();
		} else if (allowedToRemove) {
			Tile tile = ScriptableObject.CreateInstance<Tile>();
			markerTilemap.SetTile(location, tile);

			playerController.addPot();
		}
	}

	bool TileHasSpriteWithName(Tile givenTile, string name) {
		return givenTile == null ? false : SpriteHasName(givenTile.sprite, name);
	}
	bool SpriteHasName(Sprite givenSprite, string name) {
		return givenSprite == null ? false : givenSprite.name == name;
	}
}
