using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
public struct TileData
{
    public int posX;
    public int posY;
    public string tileType;
}
public class PlotManager : MonoBehaviour
{
    private PackageManager manager;
    [SerializeField] private int tileSize;
    [SerializeField] private GameObject plotTilePrefab;
    private void Start()
    {
        manager = PackageManager.Instance;
        PlotGetRequest getRequest = new PlotGetRequest
        {
            token = PlayerPrefs.GetString("token")
        };
        StartCoroutine(PlotFindRequest(getRequest));
    }
    private IEnumerator PlotFindRequest(PlotGetRequest getRequest)
    {
        yield return StartCoroutine(manager.WebRequest<PlotGetRequest, GetPlotResponse>(getRequest,
            response =>
            {
                int[] plotSize = Array.ConvertAll(response.plot.Split(','), int.Parse);
                CreatePlot(plotSize);
            }));
    }
    private IEnumerator PlotSaveRequest(TileSaveRequest saveRequest)
    {
        yield return StartCoroutine(manager.WebRequest<TileSaveRequest, PlotResponse>(saveRequest,
            response =>
            {
                Debug.Log(response.customMessage);
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
public class PlotResponse : AbstractResponse
{
    public TileData plot;
}

[System.Serializable]
public class GetPlotResponse : AbstractResponse
{
    public string plot;
}
