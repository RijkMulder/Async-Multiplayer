using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct TileData
{
    public float posX;
    public float posY;
    public string tileType;
}
[System.Serializable]
public struct UserData
{
    public int gold;
}
public class PlotManager : MonoBehaviour
{
    public static PlotManager instance { get; private set; }
    private string url = "http://127.0.0.1/edsa-webdev/Plot/PlotManager.php";
    private PackageManager manager;
    private BuildingManager buildingManager;
    [SerializeField] private int tileSize;
    [SerializeField] private GameObject plotTilePrefab;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        manager = PackageManager.Instance;
        buildingManager = BuildingManager.instance;
        PlotGetRequest getRequest = new PlotGetRequest
        {
            token = PlayerPrefs.GetString("token")
        };
        StartCoroutine(PlotConstructRequest(getRequest));
    }
    private IEnumerator PlotConstructRequest(PlotGetRequest getRequest)
    {
        yield return StartCoroutine(manager.WebRequest<PlotGetRequest, GetPlotResponse>(getRequest,
            response =>
            {
                int[] plotSize = Array.ConvertAll(response.plotSize.Split(','), int.Parse);
                CreatePlot(plotSize, response.tiles);
            }, url));
    }
    public IEnumerator PlotSaveRequest(TileSaveRequest saveRequest)
    {
        yield return StartCoroutine(manager.WebRequest<TileSaveRequest, PlotResponse>(saveRequest,
            response =>
            {
                Debug.Log(response.customMessage);
            }, url));
    }

    public IEnumerator TileCheckRequest(TileCheckRequest checkRequest, Action<bool> onComplete)
    {
        yield return StartCoroutine(manager.WebRequest<TileCheckRequest, PlotResponse>(checkRequest,
            response =>
            {
                bool outcome = response.status == "tileExists";
                onComplete(outcome);
            }, url));
    }
    private void CreatePlot(int[] plotSize, TileData[] tiles)
    {
        // create plot
        for (int i = 0; i < plotSize[0]; i++)
        {
            for (int j = 0; j < plotSize[1]; j++)
            {
                // set position
                Vector2Int position = new Vector2Int(j * tileSize, i * tileSize);
                Instantiate(plotTilePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
        }
        // place buildings
        foreach (TileData tile in tiles)
        {
            Vector3 position = new Vector3(tile.posX, 0.5f,  tile.posY);
            buildingManager.CreateBuilding(tile.tileType, position);
        }
    }
}
[System.Serializable]
public class PlotGetRequest : AbstractRequest
{
    public string token;
    public PlotGetRequest()
    {
        action = "getPlot";
    }
}
[System.Serializable]
public class TileSaveRequest : AbstractRequest
{
    public string token;
    public TileData tile;
    public TileSaveRequest()
    {
        action = "savePlot";
    }
}
[System.Serializable]
public class TileCheckRequest : AbstractRequest
{
    public string token;
    public TileData tile;
    public TileCheckRequest()
    {
        action = "checkTile";
    }
}
[System.Serializable]
public class PlotResponse : AbstractResponse
{
    public TileData tile;
}

[System.Serializable]
public class GetPlotResponse : AbstractResponse
{
    public string plotSize;
    public TileData[] tiles;
    public UserData userData;
}
