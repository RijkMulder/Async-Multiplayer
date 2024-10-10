using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct TileData
{
    public float posX;
    public float posY;
    public string tileType;
}
public class PlotManager : MonoBehaviour
{
    public static PlotManager instance { get; private set; }
    private PackageManager manager;
    [SerializeField] private int tileSize;
    [SerializeField] private GameObject plotTilePrefab;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        manager = PackageManager.Instance;
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
                int[] plotSize = Array.ConvertAll(response.plot.Split(','), int.Parse);
                CreatePlot(plotSize);
            }));
    }
    public IEnumerator PlotSaveRequest(TileSaveRequest saveRequest)
    {
        yield return StartCoroutine(manager.WebRequest<TileSaveRequest, PlotResponse>(saveRequest,
            response =>
            {
                Debug.Log(response.customMessage);
            }));
    }

    private IEnumerator TileCheckRequestCoroutine(TileCheckRequest checkRequest, Action<bool> onComplete)
    {
        yield return StartCoroutine(manager.WebRequest<TileCheckRequest, PlotResponse>(checkRequest,
            response =>
            {
                bool outcome = response.status == "tileExists";
                onComplete(outcome);
            }));
    }
    private void CreatePlot(int[] plotSize)
    {
        for (int i = 0; i < plotSize[0]; i++)
        {
            for (int j = 0; j < plotSize[1]; j++)
            {
                // set position
                Vector2Int position = new Vector2Int(j * tileSize, i * tileSize);
                Instantiate(plotTilePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// returns true when tile exists
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckTile(Vector2 position)
    {
        TileCheckRequest checkRequest = new TileCheckRequest
        {
            token = PlayerPrefs.GetString("token"),
            tile = new TileData { posX = position.x, posY = position.y }
        };

        bool result = false;
        StartCoroutine(TileCheckRequestCoroutine(checkRequest, outcome => result = outcome));
        return result;
    }
}
[System.Serializable]
public class TileDataList
{
    public TileData[] tiles;
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
    public string plot;
}
