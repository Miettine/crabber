using UnityEngine;
[CreateAssetMenu(fileName = "GridGameObjectNames", menuName = "Grid Game Object names", order = 2)]
public class GridGameObjectNames : ScriptableObject {
	[SerializeField]
	string waterTileName = "HexTilesetv3_5";
	[SerializeField]
	string terrainGridGameObjectName = "TerrainGrid";
	[SerializeField]
	string markerGridGameObjectName = "MarkerGrid";
	[SerializeField]
	string numberGridGameObjectName = "NumberGrid";
	[SerializeField]
	string randomizerGridGameObjectName = "RandomizerGrid";

	public string WaterTileName { get => waterTileName; }
	public string TerrainGridGameObjectName { get => terrainGridGameObjectName; }
	public string MarkerGridGameObjectName { get => markerGridGameObjectName; }
	public string NumberGridGameObjectName { get => numberGridGameObjectName; }
	public string RandomizerGridGameObjectName { get => randomizerGridGameObjectName; }
}
