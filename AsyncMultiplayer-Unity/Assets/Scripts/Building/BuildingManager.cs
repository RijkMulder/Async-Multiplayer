using System;
using System.Collections;
using UnityEngine;

[Serializable]
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
        if (Input.GetMouseButtonDown(0))
        {
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
            tile = new TileData { posX = pos.x, posY = pos.z, tileType = currentType.buildingName },
        };

        bool result = false;
        yield return StartCoroutine(plotManager.TileCheckRequest(checkRequest, outcome => result = outcome));
        
        // build building if not occupied
        if (result) yield break;
        
        // check if building type exists
        BuildingType type = Array.Find(buildingTypes, x => x.buildingName == currentType.buildingName);
        if (type.prefab == null)
        {
            Debug.LogError($"Building type {currentType.buildingName} not found");
            yield break;
        }
        CreateBuilding(type, pos);
        
        // save building to database
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
    public void CreateBuilding(BuildingType type, Vector3 position, string tileType = "", string lastUpdate = "")
    {
        // override building type if tileType is not empty
        if (tileType != string.Empty) type = Array.Find(buildingTypes, x => x.buildingName == tileType);
        if (type.prefab == null)
        {
            Debug.LogError($"Building type {type.buildingName} not found");
            return;
        }
        // create building on top of tile
        Vector3 offset = new Vector3(0, type.prefab.transform.localScale.y / 2, 0);
        GameObject newBuilding = Instantiate(type.prefab, position + offset, Quaternion.identity);
        if (lastUpdate != null && newBuilding.TryGetComponent(out IBuildingData buildingData))
        {
            buildingData.GetInterval(lastUpdate);
        }
    }
}
