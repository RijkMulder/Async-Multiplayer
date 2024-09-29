using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Logout : MonoBehaviour
{
    private PackageManager packageManager;

    private void Start()
    {
        packageManager = PackageManager.Instance;
    }

    public void LogoutUser()
    {
        // check if token exists, should not be empty
        string token = PlayerPrefs.GetString("token");
        if (token == string.Empty) return;
        
        // send request
        string json = JsonUtility.ToJson(new LogoutRequest { token = token });
        StartCoroutine(packageManager.SendRequest(json));
    }
}

[System.Serializable]
public class LogoutRequest
{
    public string action = "logout";
    public string token;
}