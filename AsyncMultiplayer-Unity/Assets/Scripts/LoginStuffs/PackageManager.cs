using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PackageManager : MonoBehaviour
{
    public static PackageManager Instance { get; private set; }
    private string url = "http://127.0.0.1/edsa-webdev/PackageManager.php";

    private void Awake()
    {
        Instance = this;
    }

    private string[] errorMsgs = new string[]
    {
        "invalidJson",
        "dataBaseConnectionError",
        "invalidAction",
        "emailNotInDatabase",
        "invalidPassword",
        "invalidEmail",
        "emailAlreadyExists",
        "usernameAlreadyExists",
        "usernameLength",
        "wrongToken"
    };
    public IEnumerator SendRequest(string newRequest) 
    {
        // add to request form
        List<IMultipartFormSection> form = new();
        form.Add(new MultipartFormDataSection("newEntry", newRequest));
        
        // send request
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            // wait for request
            webRequest.timeout = 10;
            yield return webRequest.SendWebRequest();
            
            // check for errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                if (webRequest.downloadHandler != null && !string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    try
                    {
                        // get response and check it
                        Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
                        Debug.Log($" {response.status} - {response.customMessage} - {response.token}"); // dont forget to remove this line
                        CheckResponse(response);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to parse response: {ex.Message}");
                    }
                }
                else
                {
                    Debug.LogError("Response is empty or null");    
                }
            }
        }
    }

    private void CheckResponse(Response response)
    {
        // check for error logs
        for (int i = 0; i < errorMsgs.Length; i++)
        {
            if (errorMsgs[i] == response.status)
            {
                Debug.LogError(response.customMessage);
                return;
            }
        }
        
        // check for success logs
        switch (response.status)
        {
            case "loginSuccesfull":
                PlayerPrefs.SetString("token", response.token);
                UIPanelManager.Instance.IsLoggedIn();
                break;
            case "loggedOut":
                UIPanelManager.Instance.ChangeSourceAsset(UIPanelManager.Instance.assets[0].asset);
                break;
            case "tokenFound":
                UIPanelManager.Instance.IsLoggedIn();
                break;
        }
    }
}

[System.Serializable]
public class Response
{
    public string status;
    public string customMessage;
    public string token;
}