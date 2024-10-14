using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct BuildingType
{
    public string buildingName;
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
        if (currentType.buildingName == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (currentType.buildingName == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            if (Physics.Raycast(ray, out RaycastHit hit) && !isOverUI)
            {
                // get position of tile
                MeshRenderer tileMesh = hit.transform.GetComponent<MeshRenderer>();
                Vector3 tilePos = tileMesh.bounds.center;
                
                // try to build
                StartCoroutine(TryBuild(tilePos));
            }
        }
    }

    private IEnumerator TryBuild(Vector3 pos)
    {
        // check if tile is already occupied
        TileCheckRequest checkRequest = new TileCheckRequest
        {
            token = PlayerPrefs.GetString("token"),
            tile = new TileData { posX = pos.x, posY = pos.z }
        };

        bool result = false;
        yield return StartCoroutine(plotManager.TileCheckRequest(checkRequest, outcome => result = outcome));
        
        // build building if not occupied
        if (result) yield break;
        CreateBuilding(currentType.buildingName, pos);
        TileData tileData = new TileData
        {
            posX = pos.x,
            posY = pos.z,
            tileType = currentType.buildingName
        };
        StartCoroutine(plotManager.PlotSaveRequest(new TileSaveRequest
        {
            token = PlayerPrefs.GetString("token"),
            tile = tileData
        }));
        
        // reset current building type
        currentType = new();
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
