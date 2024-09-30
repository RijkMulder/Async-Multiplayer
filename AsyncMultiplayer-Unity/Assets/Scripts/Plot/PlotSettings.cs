using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlotSettings : MonoBehaviour
{
    private PackageManager manager;
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
                action = "getPlot",
                token = PlayerPrefs.GetString("token")
            };
            string json = JsonUtility.ToJson(request);
            StartCoroutine(manager.SendRequest(json));
        }
    }
}
public class PlotRequest
{
    public string action;
    public string token;
}
