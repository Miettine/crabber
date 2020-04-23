using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static PlayerController;

public class GridTracker : MonoBehaviour {

	const string WaterTileName = "HexTilesetv3_5";
	const string PlacedPotTileName = "HexTilesetv3_41";
	const string TerrainGridName = "TerrainGrid";
	const string MarkerGridName = "MarkerGrid";
	const string NumberGridName = "NumberGrid";
	const string SwarmControllerName = "SwarmController";

	Grid terrainGrid;
	Tilemap terrainTilemap;

	Grid markerGrid;
	Tilemap markerTilemap;

	Grid numberGrid;
	Tilemap numberTilemap;

	SwarmController swarmController;

	//TODO: Get sprite with code
	[SerializeField]
	private Sprite potSprite;

	[SerializeField]
	private Sprite[] numberSprites;

	[SerializeField]
	Color previousRoundColor;
	private void Awake() {
		terrainGrid = GameObject.Find(TerrainGridName).GetComponent<Grid>();
		terrainTilemap = terrainGrid.GetComponentInChildren<Tilemap>();
		
		markerGrid = GameObject.Find(MarkerGridName).GetComponent<Grid>();
		markerTilemap = markerGrid.GetComponentInChildren<Tilemap>();

		numberGrid = GameObject.Find(NumberGridName).GetComponent<Grid>();
		numberTilemap = numberGrid.GetComponentInChildren<Tilemap>();

		swarmController = GameObject.Find(SwarmControllerName).GetComponent<SwarmController>();
		swarmController.SetGridTracker(this);
		//playerController = GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();

		if (potSprite == null)
			throw new Exception("ERROR: Failed to find potSprite");
	}

	internal void LiftAllPots(AddPotDelegate addPotDelegate, AddCrabDelegate addCrabDelegate) {
		//var potTiles = new List<Tile>();

		foreach (var location in numberTilemap.cellBounds.allPositionsWithin) {
			var markerTile = (Tile)numberTilemap.GetTile(location);

			if (markerTile != null && markerTile.sprite != null)
				SetNumberTileInOffsetCoordinates(location, markerTile.sprite, previousRoundColor);
		}

		foreach (var location in markerTilemap.cellBounds.allPositionsWithin) {
			
			Tile potTile = (Tile) markerTilemap.GetTile(location);
			
			if (potTile != null && potTile.sprite != null && potTile.sprite.name == PlacedPotTileName) {
				//potTiles.Add(potTile);
				
				int liftedCrab = swarmController.GetCrab(location);

				addCrabDelegate(liftedCrab);
				addPotDelegate();

				ClearMarkerTile(location);
				SetNumberTileInOffsetCoordinates(location, liftedCrab, Color.white);
			}
		}
		//return potTiles;
	}


	void Start() {

		//PlaceUnderWaterTile(new Vector3Int(0, 0, 2), 3);
	}

	void ClearMarkerTile(Vector3Int location) {
		Tile emptyTile = ScriptableObject.CreateInstance<Tile>();
		markerTilemap.SetTile(location, emptyTile);
	}

	public void SetNumberTileInCubic(Vector3Int locationInCubicCoord, int number) {
		var offset = CubicCrabGrid.CubicToOffset(locationInCubicCoord);
		Debug.Log(String.Format("Number tile cubic {0} offset {1} amount {2}", locationInCubicCoord, offset, number));
		SetNumberTileInOffsetCoordinates(offset, number, previousRoundColor);
	}

	void SetNumberTileInOffsetCoordinates(Vector3Int locationInOffsetCoord, int number, Color color){
		SetNumberTileInOffsetCoordinates(locationInOffsetCoord, numberSprites[number], color);
	}

	void SetNumberTileInOffsetCoordinates(Vector3Int location, Sprite sprite, Color color) {
		Tile numberTile = ScriptableObject.CreateInstance<Tile>();
		numberTile.sprite = sprite;

		if (color != null)
			numberTile.color = color;

		numberTilemap.SetTile(location, numberTile);
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

			Debug.Log(String.Format("Placed pot at offset {0}, cubic {1}", location, CubicCrabGrid.OffsetToCubic(location)));
	
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
