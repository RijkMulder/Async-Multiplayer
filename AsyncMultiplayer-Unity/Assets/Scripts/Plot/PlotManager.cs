using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
public struct TileData
{
    public Vector2Int coord;
    public Vector3 position;
    public GameObject tileOccupent;
}
public class PlotManager : MonoBehaviour
{
    private PackageManager manager;
    [SerializeField] private GameObject plotTilePrefab;
    private void Start()
    {
        manager = PackageManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlotGetRequest getRequest = new PlotGetRequest
            {
                token = PlayerPrefs.GetString("token")
            };
            StartCoroutine(PlotFindRequest(getRequest));
        }
    }
    private IEnumerator PlotFindRequest(PlotGetRequest getRequest)
    {
        yield return StartCoroutine(manager.WebRequest<PlotGetRequest, PlotResponse>(getRequest,
            response =>
            {
                if (response.status == "gotPlot") return;
                int[] plotSize = Array.ConvertAll(response.plot.Split(','), int.Parse);
                CreatePlot(plotSize);
            }));
    }
    private IEnumerator PlotSaveRequest(PlotSaveRequest saveRequest)
    {
        yield return StartCoroutine(manager.WebRequest<PlotSaveRequest, PlotResponse>(saveRequest,
            response =>
            {
                Debug.Log(response.customMessage);
            }));
    }
    private void CreatePlot(int[] plotSize)
    {
        float tileSize = plotTilePrefab.transform.localScale.x;
        List<TileData> tiles = new List<TileData>();
        for (int i = 0; i < plotSize[0]; i++)
        {
            for (int j = 0; j < plotSize[1]; j++)
            {
                // set position and coord
                Vector3 position = new Vector3(j * tileSize, 0, i * tileSize);
                Vector2Int coord = new Vector2Int(i + 1, j + 1);
                
                // create tile and add to list
                tiles.Add(InitTile(coord, position));
            }
        }
        
        // save plot
        PlotSaveRequest saveRequest = new PlotSaveRequest
        {
            token = PlayerPrefs.GetString("token"),
            plot = JsonUtility.ToJson(new TileDataList { tiles = tiles }, true)
        };
        StartCoroutine(PlotSaveRequest(saveRequest));
    }

    private TileData InitTile(Vector2Int coord, Vector3 position)
    {
        GameObject newTile = Instantiate(plotTilePrefab, position, Quaternion.identity);
        TileData data = new();
        (data.coord, data.position) = (coord, position);
        return data;
    }
}
[System.Serializable]
public class TileDataList
{
    public List<TileData> tiles;
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
public class PlotSaveRequest : AbstractRequest
{
    public string token;
    public string plot;
    public PlotSaveRequest()
    {
        action = "savePlot";
    }
}
[System.Serializable]
public class PlotResponse : AbstractResponse
{
    public string plot;
}
