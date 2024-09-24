using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckLoginToken : MonoBehaviour
{
    private PackageManager packageManager;

    private void Start()
    {
        packageManager = PackageManager.Instance;
        // check if token exists
        string token = PlayerPrefs.GetString("token");
        if (token == string.Empty) return;
        
        // send request
        string json = JsonUtility.ToJson(new CheckTokenRequest { token = token });
        StartCoroutine(packageManager.SendRequest(json));
    }
}
[System.Serializable]
public class CheckTokenRequest
{
    public string action = "checkToken";
    public string token;
}
