using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static PlayerController;

public class GridTracker : MonoBehaviour
{

	const string WaterTileName = "HexTilesetv3_5";
	const string PlacedPotTileName = "HexTilesetv3_41";
	const string TerrainGridName = "TerrainGrid";
	const string MarkerGridName = "MarkerGrid";
	const string UnderwaterGridName = "UnderwaterGrid";
	const string SwarmControllerName = "SwarmController";

	Grid terrainGrid;
	Tilemap terrainTilemap;

	Grid markerGrid;
	Tilemap markerTilemap;

	Grid underwaterGrid;
	Tilemap underwaterTilemap;

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
		underwaterTilemap = underwaterGrid.GetComponentInChildren<Tilemap>();

		swarmController = GameObject.Find(SwarmControllerName).GetComponent<SwarmController>();
		swarmController.SetGridTracker(this);

		//playerController = GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();

		if (potSprite == null)
			throw new Exception("ERROR: Failed to find potSprite");
	}

	internal List<Tile> GetAllPotTiles(AddPotDelegate addPotDelegate, AddCrabDelegate addCrabDelegate) {
		var potTiles = new List<Tile>();

		foreach (var location in markerTilemap.cellBounds.allPositionsWithin) {
			//Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
			Tile potTile = (Tile) markerTilemap.GetTile(location);
			
			if (potTile != null && potTile.sprite != null && potTile.sprite.name == PlacedPotTileName) {
				potTiles.Add(potTile);
				UnderwaterTile underwaterTile = RevealUnderWaterTile(location);

				int liftedCrab = underwaterTile.Crab;

				addCrabDelegate(liftedCrab);
				addPotDelegate();

				if (liftedCrab > 1)
					underwaterTile.Crab = 1;
				else if (liftedCrab == 1)
					underwaterTile.Crab = 0;

				potTile.sprite = null;

				markerTilemap.RefreshTile(location);
				underwaterTilemap.RefreshTile(location);
			}
				
		}

		return potTiles;
	}


	void Start() {

		swarmController.PlaceDebugSwarms();

		//PlaceUnderWaterTile(new Vector3Int(0, 0, 2), 3);
	}

	public UnderwaterTile PlaceUnderWaterTile(Vector2Int location, int crabAmount) {
		return PlaceUnderWaterTile(new Vector3Int(location.x, location.y, 0), crabAmount);
	}
	public UnderwaterTile PlaceUnderWaterTile(Vector3Int location, int crabAmount) {

		UnderwaterTile underwaterTile = ScriptableObject.CreateInstance<UnderwaterTile>();

		underwaterTile.Crab = crabAmount;

		//underwaterTile.sprite = this.numberSprites[crabAmount];
		underwaterTilemap.SetTile(location, underwaterTile);

		return underwaterTile;
	}

	public UnderwaterTile RevealUnderWaterTile(Vector2Int location) {
		return RevealUnderWaterTile( new Vector3Int(location.x, location.y, 0) );
	}

	public UnderwaterTile RevealUnderWaterTile(Vector3Int location) {

		UnderwaterTile underwaterTile = GetUnderwaterTile(location);

		if (underwaterTile == null)
			underwaterTile = PlaceUnderWaterTile(location, 0);

		underwaterTile.sprite = numberSprites[underwaterTile.Crab];

		underwaterTile.DebugPrintCrab();
		underwaterTilemap.RefreshTile(location);

		return underwaterTile;
	}

	UnderwaterTile GetUnderwaterTile(Vector3Int location) {
		return (UnderwaterTile) underwaterTilemap.GetTile(location);
	}

	public void PlaceOrRemovePot(Vector3 worldPos, bool playerHasPotsLeft, ThrowPotDelegate throwPotDelegate, AddPotDelegate addPotDelegate) {
		Vector3Int terrainCoordinate = terrainGrid.WorldToCell(worldPos);
		var terrainTile = (Tile) terrainTilemap.GetTile(terrainCoordinate);

		bool terrainIsWaterTile = TileHasSpriteWithName(terrainTile, WaterTileName);

		Vector3Int markerCoordinate = markerGrid.WorldToCell(worldPos);
		var markerTile = (Tile) markerTilemap.GetTile(markerCoordinate);

		bool markerGridHasPlacedPotTile = TileHasSpriteWithName(markerTile, PlacedPotTileName);

		bool allowedToPlace = terrainIsWaterTile && !markerGridHasPlacedPotTile;
		bool allowedToRemove = terrainIsWaterTile && markerGridHasPlacedPotTile;
		Vector3Int location = markerCoordinate;


		if (allowedToPlace && playerHasPotsLeft) {
			Tile tile = ScriptableObject.CreateInstance<Tile>();
			tile.sprite = potSprite;
			markerTilemap.SetTile(location, tile);

			throwPotDelegate();
		} else if (allowedToRemove) {
			Tile tile = ScriptableObject.CreateInstance<Tile>();
			markerTilemap.SetTile(location, tile);

			addPotDelegate();
		}
	}

	bool TileHasSpriteWithName(Tile givenTile, string name) {
		return givenTile == null ? false : SpriteHasName(givenTile.sprite, name);
	}
	bool SpriteHasName(Sprite givenSprite, string name) {
		return givenSprite == null ? false : givenSprite.name == name;
	}
}
