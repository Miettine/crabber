using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTracker : MonoBehaviour
{
    const string waterTileName = "HexTilesetv3_5";

    Grid grid;
    Tilemap tilemap;
    private void Awake() {
        grid = GameObject.Find( "Grid" ).GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
    }
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);

        bool allowed = AllowedToPlacePot(tilemap.GetSprite(coordinate));
        Debug.Log( allowed );
    }

    bool AllowedToPlacePot(Sprite givenSprite) {
        
        return givenSprite == null ? false : givenSprite.name == waterTileName;
    }
}
