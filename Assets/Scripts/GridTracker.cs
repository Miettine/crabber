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

    Grid terrainGrid;
    Tilemap terrainTilemap;

    Grid markerGrid;
    Tilemap markerTilemap;
    private void Awake() {
        terrainGrid = GameObject.Find(TerrainGridName).GetComponent<Grid>();
        terrainTilemap = terrainGrid.GetComponentInChildren<Tilemap>();

        markerGrid = GameObject.Find(MarkerGridName).GetComponent<Grid>();
        markerTilemap = markerGrid.GetComponentInChildren<Tilemap>();
    }
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        bool allowed = AllowedToPlacePot(mouseWorldPos);
        Debug.Log( allowed );
    }
    bool AllowedToPlacePot(Vector3 worldPos) {
        Vector3Int terrainCoordinate = terrainGrid.WorldToCell(worldPos);
        Sprite terrainSprite = terrainTilemap.GetSprite(terrainCoordinate);

        bool allowedToPlaceTerrain = SpriteHasName(terrainSprite, WaterTileName);


        Vector3Int markerCoordinate = markerGrid.WorldToCell(worldPos);
        Sprite markerSprite = terrainTilemap.GetSprite(markerCoordinate);

        bool allowedToPlaceMarker = SpriteHasName(markerSprite, PlacedPotTileName);

        return allowedToPlaceTerrain && allowedToPlaceMarker;
    }
     bool SpriteHasName(Sprite givenSprite, string name) {
        
        return givenSprite == null ? false : givenSprite.name == name;
    }
}
