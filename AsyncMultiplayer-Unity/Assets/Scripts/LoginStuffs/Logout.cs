using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    private PackageManager packageManager;
    private string url = "http://127.0.0.1/edsa-webdev/Account/AccountManager.php";

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
        LogoutRequest request = new LogoutRequest
        {
            token = token
        };
        StartCoroutine(LogoutRequest(request));
    }
    private IEnumerator LogoutRequest(LogoutRequest request)
    {
        yield return StartCoroutine(packageManager.WebRequest<LogoutRequest, LogoutResponse>(request,
            response =>
            {
                SceneManager.LoadScene("LoginScene");
            }, url));
    }
}

[System.Serializable]
public class LogoutRequest : AbstractRequest
{
    public string token;
    public LogoutRequest()
    {
        action = "logout";
    }
}
[System.Serializable]
public class LogoutResponse : AbstractResponse
{
    
}