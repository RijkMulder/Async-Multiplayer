using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

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
            PlotRequest request = new PlotRequest
            {
                token = PlayerPrefs.GetString("token")
            };
            StartCoroutine(PlotRequest(request));
        }
    }
    private IEnumerator PlotRequest(PlotRequest request)
    {
        yield return StartCoroutine(manager.WebRequest<PlotRequest, PlotResponse>(request,
            response =>
            {
                int[] plotSize = Array.ConvertAll(response.plot.Split(','), int.Parse);
                CreatePlot(plotSize);
            }));
    }
    private void CreatePlot(int[] plotSize)
    {
        for (int i = 0; i < plotSize[0]; i++)
        {
            for (int j = 0; j < plotSize[1]; j++)
            {
                Vector3 position = new Vector3(j, 0, i);
                GameObject newTile = Instantiate(plotTilePrefab, position, Quaternion.identity);
                newTile.name = $"{i + 1},{j + 1}";
            }
        }
    }
}
[System.Serializable]
public class PlotRequest : AbstractRequest
{
    public string token;
    public PlotRequest()
    {
        action = "getPlot";
    }
}
[System.Serializable]
public class PlotResponse : AbstractResponse
{
    public string plot;
}
