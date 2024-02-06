using UnityEngine;

[CreateAssetMenu(fileName = "TilesetData", menuName = "Tileset Data")]
public class TilesetData : ScriptableObject
{
    public float BlockSize;
    public GameObject DownCenterTile;
    public GameObject DownEdgeTile;
    public GameObject DownCornerTile;
    public GameObject MiddleCenterTile;
    public GameObject MiddleEdgeTile;
    public GameObject UpCenterTile;
    public GameObject UpEdgeTile;
    public GameObject UpCornerTile;
}
