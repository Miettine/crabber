using System;
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

	Grid terrainGrid;
	Tilemap terrainTilemap;

	Grid markerGrid;
	Tilemap markerTilemap;

	Grid underwaterGrid;
	Tilemap underwaterTilemap;

	PlayerController playerController;

	//TODO: Get sprite with code
	[SerializeField]
	private Sprite potSprite;

	[SerializeField]
	private Sprite debugUnderwaterTileSprite;
	
	private void Awake() {
		terrainGrid = GameObject.Find(TerrainGridName).GetComponent<Grid>();
		terrainTilemap = terrainGrid.GetComponentInChildren<Tilemap>();

		markerGrid = GameObject.Find(MarkerGridName).GetComponent<Grid>();
		markerTilemap = markerGrid.GetComponentInChildren<Tilemap>();

		underwaterGrid = GameObject.Find(UnderwaterGridName).GetComponent<Grid>();
		underwaterTilemap = markerGrid.GetComponentInChildren<Tilemap>();

		playerController = GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();

		if (potSprite == null)
			throw new Exception("ERROR: Failed to find potSprite");
	}

	internal List<Vector3Int> GetAllPotLocations() {
		throw new NotImplementedException();
	}

	void Start() {
		DebugUnderwaterGrid(new Vector3Int(0, 0, 0), 47);
		DebugUnderwaterGrid(new Vector3Int(1, 0, 0), 23);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			PlaceOrRemovePot(mouseWorldPos);

		}
	}
	void DebugUnderwaterGrid(Vector3Int location, int amount) {

		UnderwaterTile underwaterTile = ScriptableObject.CreateInstance<UnderwaterTile>();
		underwaterTile.sprite = potSprite;

		underwaterTile.sprite = debugUnderwaterTileSprite;
		underwaterTile.Crab = amount;
		underwaterTile.DebugPrintCrab();
		underwaterTilemap.SetTile(location, underwaterTile);
		underwaterTile.DebugPrintCrab();
	}

	void PlaceOrRemovePot(Vector3 worldPos) {
		Vector3Int terrainCoordinate = terrainGrid.WorldToCell(worldPos);
		var terrainTile = (Tile) terrainTilemap.GetTile(terrainCoordinate);

		bool terrainIsWaterTile = SpriteHasName(terrainTile.sprite, WaterTileName);

		Vector3Int markerCoordinate = markerGrid.WorldToCell(worldPos);
		var markerTile = (Tile) markerTilemap.GetTile(markerCoordinate);

		bool markerGridHasPlacedPotTile = SpriteHasName(markerTile.sprite, PlacedPotTileName);

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

	bool SpriteHasName(Sprite givenSprite, string name) {
		return givenSprite == null ? false : givenSprite.name == name;
	}
}
