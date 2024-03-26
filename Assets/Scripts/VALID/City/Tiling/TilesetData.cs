using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TilesetData", menuName = "Tileset Data")]
public class TilesetData : ScriptableObject
{
    [Header("Data")]
    public float BlockSize;
    public Vector3 BlockRotation;
    public float RandomnessAmount;

    [Header("Blocks")]
    public OuterFloor[] DownFloor;
    public InnerFloor[] MiddleFloor;
    public InnerFloor[] UpFloor;
    public OuterFloor[] TopFloor;
}

[Serializable]
public struct InnerFloor
{
    public GameObject[] EdgeTile;
    public GameObject[] CornerTile;
}

[Serializable]
public struct OuterFloor
{
    public GameObject[] CenterTile;
    public GameObject[] EdgeTile;
    public GameObject[] CornerTile;
}