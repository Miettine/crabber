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
    //const string PotPlacerName = "PotPlacer";

    private delegate void PlacePotDelegate(Tile tile);
    private delegate void RemovePotDelegate(Tile tile);
    PlacePotDelegate placePotFunction;
    RemovePotDelegate removePotFunction;

    Grid terrainGrid;
    Tilemap terrainTilemap;

    Grid markerGrid;
    Tilemap markerTilemap;

    PotPlacer potPlacer;

    public Sprite sprite;
    private void Awake() {
        terrainGrid = GameObject.Find(TerrainGridName).GetComponent<Grid>();
        terrainTilemap = terrainGrid.GetComponentInChildren<Tilemap>();

        markerGrid = GameObject.Find(MarkerGridName).GetComponent<Grid>();
        markerTilemap = markerGrid.GetComponentInChildren<Tilemap>();

        placePotFunction = new PlacePotDelegate(PlacePot);
        removePotFunction = new RemovePotDelegate(RemovePot);
        //potPlacer = GameObject.Find(PotPlacerName).GetComponent<PotPlacer>();
    }
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            PlaceOrRemovePot(mouseWorldPos, placePotFunction, removePotFunction);

        }
    }
    void PlaceOrRemovePot(Vector3 worldPos, PlacePotDelegate placePot, RemovePotDelegate removePot) {
        Vector3Int terrainCoordinate = terrainGrid.WorldToCell(worldPos);
        var terrainTile = (Tile) terrainTilemap.GetTile(terrainCoordinate);

        bool terrainIsWaterTile = SpriteHasName(terrainTile.sprite, WaterTileName);

        Vector3Int markerCoordinate = markerGrid.WorldToCell(worldPos);
        var markerTile = (Tile) markerTilemap.GetTile(markerCoordinate);

        bool markerGridHasPlacedPotTile = SpriteHasName(markerTile.sprite, PlacedPotTileName);

        bool allowedToPlace = terrainIsWaterTile && !markerGridHasPlacedPotTile;
        bool allowedToRemove = terrainIsWaterTile && markerGridHasPlacedPotTile;
        Vector3Int location = markerCoordinate;

        if (allowedToPlace) {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            markerTilemap.SetTile(location, tile);
            //placePot(markerTile);
        } else if (allowedToRemove) {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            markerTilemap.SetTile(location, tile);
            //removePot(markerTile);
        }

    }

    void PlacePot(Tile tile) {
        Debug.Log("PlacePot "+tile);
    }

    void RemovePot(Tile tile) {
        tile.sprite = null;
        Debug.Log("RemovePot " + tile);
    }

    bool SpriteHasName(Sprite givenSprite, string name) {
        
        return givenSprite == null ? false : givenSprite.name == name;
    }
}
