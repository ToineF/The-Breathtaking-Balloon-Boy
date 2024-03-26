using System.Collections.Generic;
using UnityEngine;
using AntoineFoucault.Utilities;

public class Tiling3D : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public TilesetData TilesetData { get; private set; }
    [field: Space(10)]

    [field: Header("Properties")]
    [field: SerializeField, Range(3, 25)] public int Length { get; private set; }
    [field: SerializeField, Range(3, 25)] public int Width { get; private set; }
    [field: SerializeField, Range(3, 25)] public int Height { get; private set; }

    private List<GameObject> _buildingBlocks = new List<GameObject>();
    private InnerFloor[] _middleFloors;

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
        _middleFloors = new InnerFloor[Height-2];
        for (int i = 1; i < Height - 1; i++)
        {
            _middleFloors[i-1] = GetRandomTile(TilesetData.MiddleFloor);
        }


        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (GetBlockScore(x, y, z) == 0) continue;

                    GameObject block = GetBlockType(x, y, z);
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

    private int GetBlockScore(int x, int y, int z, int interiorOffset = 0)
    {
        int score = 0;
        if (z == interiorOffset) score++;
        if (z == Length - 1 - interiorOffset) score++;
        if (x == interiorOffset) score++;
        if (x == Width - 1 - interiorOffset) score++;
        if (y == interiorOffset) score++;
        if (y == Height - 1 - interiorOffset) score++;

        return score;
    }

    private GameObject GetBlockType(int x, int y, int z)
    {
        GameObject block;
        int score = GetBlockScore(x, y, z);

        if (score <= 1) // Center
        {
            if (y == 0)
                block = TilesetData.DownFloor[0].CenterTile;
            else if (y == Height - 1)
            {
                int topScore = GetBlockScore(x, y, z, 1);

                if (topScore >= 2) block = TilesetData.TopFloor[0].CornerTile;
                else if (topScore >= 1) block = TilesetData.TopFloor[0].EdgeTile;
                else block = TilesetData.TopFloor[0].CenterTile;
            }
            else
                block = _middleFloors[y-1].CornerTile;
        }
        else if (score == 2) // Edges
        {
            if (y == 0)
                block = TilesetData.DownFloor[0].EdgeTile;
            else if (y == Height - 1)
                block = TilesetData.UpFloor[0].EdgeTile;
            else
                block = _middleFloors[y-1].EdgeTile;
        }
        else // Corners
        {
            if (y == 0)
                block = TilesetData.DownFloor[0].CornerTile;
            else
                block = TilesetData.UpFloor[0].CornerTile;
        }

        return block;
    }

    private Vector3 GetRotation(int x, int y, int z)
    {
        int score = GetBlockScore(x, y, z);
        int interiorOffset = 0;
        if ((y == Height - 1 && score <= 1)) interiorOffset++;
        int relativeScore = GetBlockScore(x, y, z, interiorOffset);

        int targetX = 0;
        int targetY = 0;
        int targetZ = 0;


        if (x == Width - 1 - interiorOffset) targetY = 180;
        if (z == Length - 1 - interiorOffset) targetY = 90;
        if (z == interiorOffset) targetY = 270;

        // Corners
        if (x == interiorOffset && z == Length - 1 - interiorOffset) targetY -= 45;
        if (x == Width - 1 - interiorOffset && z == interiorOffset) targetY -= 45;
        if (x == interiorOffset && z == interiorOffset) targetY += 45;
        if (x == Width - 1 - interiorOffset && z == Length - 1 - interiorOffset) targetY += 45;

        // Top
        if (interiorOffset == 1)
        {
            if (Length <= 3 || Width <= 3) return Vector3.zero;
            targetZ += 45;
            if (relativeScore != 2) targetZ += 45;
        }

        return new Vector3(targetX, targetY, targetZ);
    }

    private OuterFloor GetRandomTile(OuterFloor[] floor)
    {
        int randomAmount = (int)Mathf.Clamp(TilesetData.RandomnessAmount, 0, floor.Length - 1);
        return floor[Random.Range(0, randomAmount)];
    }

    private InnerFloor GetRandomTile(InnerFloor[] floor)
    {
        int randomAmount = (int)Mathf.Clamp(TilesetData.RandomnessAmount, 0, floor.Length - 1);
        Debug.Log(randomAmount);
        return floor[Random.Range(0, randomAmount+1)];
    }
}
