using System;
using UnityEngine;
using static AntoineFoucault.Utilities.CollectionsExtensions;

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

    [Header("Doors")]
    public bool UseDoors;
    public InnerFloor[] DoorsFloor;
}

[Serializable]
public struct InnerFloor
{
    public PonderableGameObject[] EdgeTile;
    public PonderableGameObject[] CornerTile;
}

[Serializable]
public struct OuterFloor
{
    public PonderableGameObject[] CenterTile;
    public PonderableGameObject[] EdgeTile;
    public PonderableGameObject[] CornerTile;
}
