using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTracker : MonoBehaviour
{
    Grid grid;
    private void Awake() {
        grid = GameObject.Find( "Grid" ).GetComponent<Grid>();
    }
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
        Debug.Log(coordinate);
    }
}
