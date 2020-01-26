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
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            AllowedToPlaceAndRemovePot(mouseWorldPos, out bool allowedToPlace, out bool allowedToRemove);

            Debug.Log("allowedToPlace: "+ allowedToPlace);
            Debug.Log("allowedToRemove: " + allowedToRemove);

            if (allowedToPlace) {
                Debug.Log("Allowed to place");
            } else if (allowedToRemove) {
                Debug.Log("Allowed to remove");
            }
        }
    }
    void AllowedToPlaceAndRemovePot(Vector3 worldPos, out bool allowedToPlace, out bool allowedToRemove) {
        Vector3Int terrainCoordinate = terrainGrid.WorldToCell(worldPos);
        Sprite terrainSprite = terrainTilemap.GetSprite(terrainCoordinate);

        bool terrainIsWaterTile = SpriteHasName(terrainSprite, WaterTileName);

        Vector3Int markerCoordinate = markerGrid.WorldToCell(worldPos);
        Sprite markerSprite = markerTilemap.GetSprite(markerCoordinate);

        bool markerGridHasPlacedPotTile = SpriteHasName(markerSprite, PlacedPotTileName);

        allowedToPlace = terrainIsWaterTile && !markerGridHasPlacedPotTile;
        allowedToRemove = terrainIsWaterTile && markerGridHasPlacedPotTile;
    }

    bool SpriteHasName(Sprite givenSprite, string name) {
        
        return givenSprite == null ? false : givenSprite.name == name;
    }
}
