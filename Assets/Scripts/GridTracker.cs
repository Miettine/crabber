using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static PlayerController;

public class GridTracker : MonoBehaviour {

	[SerializeField]
	string waterTileName = "HexTilesetv3_5";

	const string TerrainGridName = "TerrainGrid";
	const string MarkerGridName = "MarkerGrid";
	const string NumberGridName = "NumberGrid";
	const string SwarmControllerName = "SwarmController";
	const string RandomizerGridName = "RandomizerGrid";
	
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

		randomizerGrid = GameObject.Find(RandomizerGridName).GetComponent<Grid>();
		randomizerTilemap = randomizerGrid.GetComponentInChildren<Tilemap>();

		swarmController = GameObject.Find(SwarmControllerName).GetComponent<SwarmController>();
		swarmController.SetGridTracker(this);
		//playerController = GameObject.Find(PlayerControllerGameObjectName).GetComponent<PlayerController>();

		if (potSprite == null)
			throw new Exception("ERROR: Failed to find potSprite");
	}

	public void LiftAllPots(AddPotDelegate addPotDelegate, AddCrabDelegate addCrabDelegate) {

		/**
		 * First make each number tile from previous round a little grayer (previousRoundColor).
		 * This is to help the player read which tiles they just lifted pots up from.
		 */
		foreach (var location in numberTilemap.cellBounds.allPositionsWithin) {
			var markerTile = (Tile)numberTilemap.GetTile(location);

			if (markerTile != null && markerTile.sprite != null)
				SetNumberTileInOffsetCoordinates(location, markerTile.sprite, previousRoundColor);
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

	System.Random randomizer = new System.Random();

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
		var offset = CubicCrabGrid.CubicToOffset(locationInCubicCoord);
		Debug.Log(string.Format("Number tile cubic {0} offset {1} amount {2}", locationInCubicCoord, offset, number));
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

		bool terrainIsWaterTile = TileHasSpriteWithName(terrainTile, waterTileName);

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

			Debug.Log(string.Format("Placed pot at offset {0}, cubic {1}", location, CubicCrabGrid.OffsetToCubic(location)));
	
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
