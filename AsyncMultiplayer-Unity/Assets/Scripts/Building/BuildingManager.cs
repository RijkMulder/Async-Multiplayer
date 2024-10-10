using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct BuildingType
{
    [FormerlySerializedAs("name")] public string buildingName;
    public GameObject prefab;
}
/// <summary>
/// Reconstruct buildings from database or make new ones
/// </summary>
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance { get; private set; }
    private PlotManager plotManager;
    [SerializeField] private BuildingType[] buildingTypes;
    [SerializeField] private BuildingType currentType;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        plotManager = PlotManager.instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentType.buildingName == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // get position of tile
                MeshRenderer tileMesh = hit.transform.GetComponent<MeshRenderer>();
                Vector3 tilePos = tileMesh.bounds.center;
                
                // check if tile is already occupied
                if (plotManager.CheckTile(new Vector2(tilePos.x, tilePos.z))) return;
                
                // create building and save to database
                CreateBuilding(currentType.buildingName, tilePos);
                TileData tileData = new TileData
                {
                    posX = tilePos.x,
                    posY = tilePos.z,
                    tileType = currentType.buildingName
                };
                StartCoroutine(plotManager.PlotSaveRequest(new TileSaveRequest
                {
                    token = PlayerPrefs.GetString("token"),
                    tile = tileData
                }));
            }
        }
    }

    public void SetCurrentBuildingType(string buildingType)
    {
        currentType = Array.Find(buildingTypes, x => x.buildingName == buildingType);
    }
    public void CreateBuilding(string buildingType, Vector3 position)
    {
        // find building type
        BuildingType type = Array.Find(buildingTypes, x => x.buildingName == buildingType);
        if (type.prefab == null)
        {
            Debug.LogError($"Building type {buildingType} not found");
            return;
        }
        Instantiate(type.prefab, position, Quaternion.identity);
    }
}
