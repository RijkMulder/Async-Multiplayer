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
        StartCoroutine(CheckTokenRequest());
    }

    private IEnumerator CheckTokenRequest()
    {
        CheckTokenRequest request = new CheckTokenRequest
        {
            token = PlayerPrefs.GetString("token")
        };
        yield return StartCoroutine(packageManager.WebRequest<CheckTokenRequest, CheckTokenResponse>(request,
            response =>
            {
                if (response.status == "tokenFound")
                {
                    UIPanelManager.Instance.IsLoggedIn();
                }
            }));
    }
}
[System.Serializable]
public class CheckTokenRequest : AbstractRequest
{ 
    public string token;
    public CheckTokenRequest()
    {
        action = "checkToken";
    }
}

[System.Serializable]
public class CheckTokenResponse : AbstractResponse
{
    
}
