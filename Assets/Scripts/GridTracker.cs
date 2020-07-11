using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static PlayerController;

/// <summary>
/// Deals with the hexagon grids in the Unity scene.
/// </summary>
public class GridTracker : Singleton<GridTracker> {

	[SerializeField]
	string waterTileName = "HexTilesetv3_5";
	[SerializeField] 
	string TerrainGridName = "TerrainGrid";
	[SerializeField]
	string MarkerGridName = "MarkerGrid";
	[SerializeField]
	string NumberGridName = "NumberGrid";
	[SerializeField]
	string SwarmControllerName = "SwarmController";
	[SerializeField]
	string RandomizerGridName = "RandomizerGrid";
	
	/// <summary>
	/// Shows the environment graphically, such as the water-areas.
	/// </summary>
	Grid terrainGrid;
	Tilemap terrainTilemap;

	/// <summary>
	/// Shows the pot sprites.
	/// </summary>
	Grid markerGrid;
	Tilemap markerTilemap;

	/// <summary>
	/// Shows the number icons how many crab the player lifted from each water-area
	/// </summary>
	Grid numberGrid;
	Tilemap numberTilemap;

	/// <summary>
	/// This grid is used to paint the locations where the crab swarms can be placed. 
	/// This is used to avoid placing any crab outside the play-area.
	/// </summary>
	Grid randomizerGrid;
	Tilemap randomizerTilemap;

	SwarmController swarmController;

	[SerializeField]
	public Sprite potSprite;

	[SerializeField]
	Sprite[] numberSprites;

	[SerializeField]
	Color previousRoundColor;

	System.Random randomizer = new System.Random();

	private void Awake() {
		terrainGrid = GameObject.Find(TerrainGridName).GetComponent<Grid>();
		terrainTilemap = terrainGrid.GetComponentInChildren<Tilemap>();
		
		markerGrid = GameObject.Find(MarkerGridName).GetComponent<Grid>();
		markerTilemap = markerGrid.GetComponentInChildren<Tilemap>();

		numberGrid = GameObject.Find(NumberGridName).GetComponent<Grid>();
		numberTilemap = numberGrid.GetComponentInChildren<Tilemap>();

		randomizerGrid = GameObject.Find(RandomizerGridName).GetComponent<Grid>();
		randomizerTilemap = randomizerGrid.GetComponentInChildren<Tilemap>();

		swarmController = GameObject.Find(SwarmControllerName).GetComponent<SwarmController>();
		swarmController.SetGridTracker(this);

		if (potSprite == null)
			throw new Exception("ERROR: Failed to find potSprite");
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="addPotDelegate">The function that gets called whenever the player retreives a pot</param>
	/// <param name="addCrabDelegate">The function that gets called whenever the player gains crab</param>
	public void LiftAllPots(AddPotDelegate addPotDelegate, AddCrabDelegate addCrabDelegate) {

		/**
		 * First make each number tile from previous round a little grayer (previousRoundColor).
		 * This is to help the player read which tiles they just lifted pots up from.
		 */
		foreach (var location in numberTilemap.cellBounds.allPositionsWithin) {
			var numberTile = (Tile)numberTilemap.GetTile(location);

			if (numberTile != null && numberTile.sprite != null)
				SetNumberTileInOffsetCoordinates(location, numberTile.sprite, previousRoundColor);
		}

		/**
		 * Next, take each tile that has a pot in it, find out how many crab that contains, and place
		 * a tile that marks the number of crab the player lifted.
		 * */
		foreach (var location in markerTilemap.cellBounds.allPositionsWithin) {
			
			Tile potTile = (Tile) markerTilemap.GetTile(location);
			
			if (potTile != null && potTile.sprite != null && potTile.sprite.name == potSprite.name) {
				
				int liftedCrab = swarmController.GetCrab(location);

				addCrabDelegate(liftedCrab);
				addPotDelegate();

				ClearMarkerTile(location);
				SetNumberTileInOffsetCoordinates(location, liftedCrab, Color.white);
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="location"></param>
	/// <returns>Whether the given tile contains a number tile</returns>
	internal bool HasNumberTile(Vector3Int locationInCubic) {
		Tile tile = (Tile) numberTilemap.GetTile(CubicCrabGrid.CubicToOffset(locationInCubic));
		if (tile != null && tile.sprite != null) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Finds all number tiles with the number zero on them and makes them white. 
	/// This is done at the end of the game to mark the tiles that the player fished from that contained no crab.
	/// </summary>
	internal void SetZeroTilesWhite() {
		foreach (var location in numberTilemap.cellBounds.allPositionsWithin) {
			var numberTile = (Tile)numberTilemap.GetTile(location);

			if (numberTile != null && numberTile.sprite != null && TileHasSpriteWithName(numberTile, numberSprites[0].name))
				SetNumberTileInOffsetCoordinates(location, numberTile.sprite, Color.white);
		}
	}

	internal bool HasWaterTile(Vector3Int locationInCubic) {
		return HasWaterTileInOffset(CubicCrabGrid.CubicToOffset(locationInCubic));
	}
	bool HasWaterTileInOffset(Vector3Int locationInOffset) {
		var terrainTile = (Tile)terrainTilemap.GetTile(locationInOffset);
		return TileHasSpriteWithName(terrainTile, waterTileName);
	}

	public Vector3Int GetRandomSwarmPlacementInCubic() {
		var locations = new List<Vector3Int>();
		foreach (var location in randomizerTilemap.cellBounds.allPositionsWithin) {

			Tile tile = (Tile)randomizerTilemap.GetTile(location);

			if (tile != null && tile.sprite != null) {
				locations.Add(location);
			}
		}
		int random = randomizer.Next(0, locations.Count);
		return CubicCrabGrid.OffsetToCubic(locations[random]);
	}
	public bool SwarmPlacementIsWithinPlayArea(Vector3Int locationInCubic) {

		var tile = (Tile)randomizerTilemap.GetTile(CubicCrabGrid.CubicToOffset(locationInCubic));

		return tile != null && tile.sprite != null;
	}

	void ClearMarkerTile(Vector3Int location) {
		Tile emptyTile = ScriptableObject.CreateInstance<Tile>();
		markerTilemap.SetTile(location, emptyTile);
	}

	public void SetNumberTileInCubic(Vector3Int locationInCubicCoord, int number) {
		SetNumberTileInCubic(locationInCubicCoord, number, previousRoundColor);
	}

	public void SetNumberTileInCubic(Vector3Int locationInCubicCoord, int number, Color color) {
		var offset = CubicCrabGrid.CubicToOffset(locationInCubicCoord);
		Debug.LogFormat("Number tile cubic {0} offset {1} amount {2}", locationInCubicCoord, offset, number);
		SetNumberTileInOffsetCoordinates(offset, number, color);
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

		var terrainCoordinate = terrainGrid.WorldToCell(worldPos);
		bool terrainIsWaterTile = HasWaterTileInOffset(terrainCoordinate);

		Vector3Int markerCoordinate = markerGrid.WorldToCell(worldPos);
		var markerTile = (Tile) markerTilemap.GetTile(markerCoordinate);

		bool markerGridHasPlacedPotTile = TileHasSpriteWithName(markerTile, potSprite.name);

		bool allowedToPlace = terrainIsWaterTile && !markerGridHasPlacedPotTile;
		bool allowedToRemove = terrainIsWaterTile && markerGridHasPlacedPotTile;
		Vector3Int location = markerCoordinate;

		/**
		 * Not all of the information gathered above is actually necessary if playerHasPotsLeft equals 'false'. 
		 * This function could be "optimized", but what's the point? The execution is lighning-fast anyway.
		 * */
		if (allowedToPlace && playerHasPotsLeft) {
			Tile tile = ScriptableObject.CreateInstance<Tile>();
			tile.sprite = potSprite;
			markerTilemap.SetTile(location, tile);

			Debug.LogFormat("Placed pot at offset {0}, cubic {1}", location, CubicCrabGrid.OffsetToCubic(location));
	
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
