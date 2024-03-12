using System.Collections.Generic;
using UnityEngine;
using AntoineFoucault.Utilities;

public class Tiling3D : MonoBehaviour
{
    [field:Header("References")]
    [field:SerializeField] public TilesetData TilesetData { get; private set; }
    [field: Space(10)]

    [field: Header("Properties")]
    [field:SerializeField, Range(3, 25)] public int Length { get; private set; }
    [field: SerializeField, Range(3, 25)] public int Width { get; private set; }
    [field: SerializeField, Range(3, 25)] public int Height { get; private set; }

    private List<GameObject> _buildingBlocks = new List<GameObject>();

    public void CreateBuilding()
    {
        ClearBuildingChildren();
        BuildBuilding();
    }

    private void ClearBuilding()
    {
        if (_buildingBlocks == null) return;
        foreach (var item in _buildingBlocks)
        {
            DestroyImmediate(item);
        }
        _buildingBlocks.Clear();
    }

    private void BuildBuilding()
    {
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (GetBlockScore(x,y,z) == 0) continue;

                    GameObject block = GetBlockType(x,y,z);
                    Vector3 position = new Vector3(x, y, z) * TilesetData.BlockSize;
                    Vector3 rotation = GetRotation(x, y, z);
                    GameObject newBlock = Instantiate(block, transform.position + position, Quaternion.Euler(TilesetData.BlockRotation + rotation), transform);
                    _buildingBlocks.Add(newBlock);
                }
            }
        }
    }

    public void ClearBuildingChildren()
    {
        ClearBuilding();
        transform.ClearImmediate();
    }

    private int GetBlockScore(int x, int y, int z)
    {
        int score = 0;
        if (z == 0) score++;
        if (z == Length - 1) score++;
        if (x == 0) score++;
        if (x == Width - 1) score++;
        if (y == 0) score++;
        if (y == Height - 1) score++;

        return score;
    }

    private GameObject GetBlockType(int x, int y, int z)
    {
        GameObject block;
        int score = GetBlockScore(x, y, z);

        if (score <= 1) // Center
        {
            if (y == 0)
                block = TilesetData.DownCenterTile;
            else if (y == Height - 1)
                block = TilesetData.UpCenterTile;
            else
                block = TilesetData.MiddleCenterTile;
        }
        else if (score == 2) // Edges
        {
            if (y == 0)
                block = TilesetData.DownEdgeTile;
            else if (y == Height - 1)
                block = TilesetData.UpEdgeTile;
            else
                block = TilesetData.MiddleEdgeTile;
        }
        else // Corners
        {
            if (y == 0) 
                block = TilesetData.DownCornerTile;
            else
                block = TilesetData.UpCornerTile;
        }


        return block;
    }

    private Vector3 GetRotation(int x, int y, int z)
    {
        int score = GetBlockScore(x, y, z);

        int targetX = 0;
        int targetY = 0;
        int targetZ = 0;

        if (x == Width - 1) targetY = 180;
        if (z == Length - 1) targetY = 90;
        if (z == 0) targetY = 270;

        // Corners
        if (x == 0 && z == Length - 1) targetY -= 45;
        if (x == Width - 1 && z == 0) targetY -= 45;
        if (x == 0 && z == 0) targetY += 45;
        if (x == Width - 1 && z == Length - 1) targetY += 45;

        return new Vector3(targetX, targetY, targetZ);
    }
}
