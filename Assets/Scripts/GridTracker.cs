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
	const string PlayerControllerGameObjectName = "PlayerController";

	Grid terrainGrid;
	Tilemap terrainTilemap;

	Grid markerGrid;
	Tilemap markerTilemap;

	PlayerController playerController;

	//TODO: Get sprite with code
	[SerializeField]
	private Sprite potSprite;
	private void Awake() {
		terrainGrid = GameObject.Find(TerrainGridName).GetComponent<Grid>();
		terrainTilemap = terrainGrid.GetComponentInChildren<Tilemap>();

		markerGrid = GameObject.Find(MarkerGridName).GetComponent<Grid>();
		markerTilemap = markerGrid.GetComponentInChildren<Tilemap>();

		playerController = GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();

		if (potSprite == null)
			throw new System.Exception("ERROR: Failed to find potSprite");
	}

	internal List<Vector3Int> GetAllPotLocations() {
		throw new NotImplementedException();
	}

	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			PlaceOrRemovePot(mouseWorldPos);

		}
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

			playerController.retrievePot();
		}
	}

	bool SpriteHasName(Sprite givenSprite, string name) {
		return givenSprite == null ? false : givenSprite.name == name;
	}
}
