using UnityEngine;

[CreateAssetMenu(fileName = "TilesetData", menuName = "Tileset Data")]
public class TilesetData : ScriptableObject
{
    [Header("Data")]
    public float BlockSize;
    public Vector3 BlockRotation;

    [Header("Blocks")]
    public GameObject DownCenterTile;
    public GameObject DownEdgeTile;
    public GameObject DownCornerTile;
    public GameObject MiddleCenterTile;
    public GameObject MiddleEdgeTile;
    public GameObject UpCenterTile;
    public GameObject UpEdgeTile;
    public GameObject UpCornerTile;
}
