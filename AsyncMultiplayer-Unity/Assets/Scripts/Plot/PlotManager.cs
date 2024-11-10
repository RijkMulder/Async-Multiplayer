using System;
using System.Collections;
using UnityEngine;
using Events;

[System.Serializable]
public struct TileData
{
    public float posX;
    public float posY;
    public string tileType;
    public string lastUpdate;
}
[System.Serializable]
public struct UserData
{
    public int gold;
    public int beet;
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
                // construct plot from size and user buildings
                int[] plotSize = Array.ConvertAll(response.plotSize.Split(','), int.Parse);
                CreatePlot(plotSize, response.tiles);
                
                // invoke user data
                EventManager.OnUserDataUpdate(response.userData);
            }, url));
    }
    public IEnumerator PlotSaveRequest(TileSaveRequest saveRequest)
    {
        yield return StartCoroutine(manager.WebRequest<TileSaveRequest, PlotResponse>(saveRequest,
            response =>
            {
                Debug.Log(response.customMessage);
                EventManager.OnCropUpdate(response.tile);
            }, url));
    }
    public IEnumerator SellRequest(SellRequest sellRequest)
    {
        yield return StartCoroutine(manager.WebRequest<SellRequest, PlotResponse>(sellRequest,
            response =>
            {
                Debug.Log(response.customMessage);
                
                // invoke user data
                EventManager.OnUserDataUpdate(response.userData);
            }, url));
    }

    public IEnumerator TileCheckRequest(TileCheckRequest checkRequest, Action<bool> onComplete)
    {
        yield return StartCoroutine(manager.WebRequest<TileCheckRequest, PlotResponse>(checkRequest,
            response =>
            {
                // bool true if tile not free or not enough money
                bool outcome = response.status != "tileFree";
                onComplete(outcome);
                
                // invoke user data and crop update
                EventManager.OnUserDataUpdate(response.userData);
                EventManager.OnCropUpdate(response.tile);
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
            buildingManager.CreateBuilding(new(), position, tile.tileType, tile.lastUpdate);
        }
    }
}
[Serializable]
public class PlotGetRequest : AbstractRequest
{
    public string token;
    public PlotGetRequest()
    {
        action = "getPlot";
    }
}
[Serializable]
public class SellRequest : AbstractRequest
{
    public string token;
    public string type;
    public SellRequest()
    {
        action = "sell";
    }
}
[Serializable]
public class TileSaveRequest : AbstractRequest
{
    public string token;
    public TileData tile;
    public TileSaveRequest()
    {
        action = "savePlot";
    }
}
[Serializable]
public class TileCheckRequest : AbstractRequest
{
    public string token;
    public TileData tile;
    public TileCheckRequest()
    {
        action = "checkTile";
    }
}
[Serializable]
public class PlotResponse : AbstractResponse
{
    public TileData tile;
    public UserData userData;
}

[Serializable]
public class GetPlotResponse : AbstractResponse
{
    public string plotSize;
    public TileData[] tiles;
    public UserData userData;
}
