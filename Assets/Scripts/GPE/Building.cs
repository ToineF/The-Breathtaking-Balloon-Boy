using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Building : MonoBehaviour
{
    public GameObject GhostBuilding { get => _ghostBuilding; private set => _ghostBuilding = value; }

    [Header("References")]
    [SerializeField] private int _cubeSize = 3;
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Renderer _originalBlock;
    [SerializeField] private GameObject _buildingCubesParent;
    [SerializeField] private Material _ghostMaterial;

    private GameObject _ghostBuilding;
    private Vector3Int _buildingSize;

    private void Awake()
    {
        _buildingSize = Vector3Int.FloorToInt(_originalBlock.bounds.size);
        //CreatingSmallCubes();
        SetUpBuildingCubes();
    }

    private void CreatingSmallCubes()
    {
        for (int x = 0; x < _buildingSize.x; x += _cubeSize)
        {
            for (int y = 0; y < _buildingSize.y; y += _cubeSize)
            {
                for (int z = 0; z < _buildingSize.z; z += _cubeSize)
                {
                    Vector3 targetPosition = _originalBlock.transform.position/2 + new Vector3(x, y, z);
                    Instantiate(_cubePrefab, targetPosition, Quaternion.identity, _buildingCubesParent.transform);
                }
            }
        }
    }


    private void SetUpBuildingCubes()
    {
        _ghostBuilding = Instantiate(_buildingCubesParent, _buildingCubesParent.transform.position, _buildingCubesParent.transform.rotation, _buildingCubesParent.transform.parent);
        int childCount = _ghostBuilding.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject cube = _ghostBuilding.transform.GetChild(i).gameObject;
            if (!cube.TryGetComponent(out Collider collider)) return;
            Destroy(collider);
            if (!cube.TryGetComponent(out MeshRenderer meshRenderer)) return;
            meshRenderer.material = _ghostMaterial;
        }
        _ghostBuilding.SetActive(false);
        _ghostBuilding.transform.localScale = _ghostBuilding.transform.localScale * 0.9999f;
    }
}
