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

    public IEnumerator WebRequest<TRequest, TResponse>(TRequest request, Action<TResponse> onComplete)
        where TRequest : AbstractRequest
        where TResponse : AbstractResponse
    {
        string newEntry = JsonUtility.ToJson(request);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection("newEntry", newEntry));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            // timeout time
            webRequest.timeout = 10;
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Request failed: {webRequest.error}");
                onComplete?.Invoke(null);
            }
            else
            {
                // Check for valid response
                if (webRequest.downloadHandler != null && !string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    try
                    {
                        TResponse response = JsonUtility.FromJson<TResponse>(webRequest.downloadHandler.text);
                        Debug.Log($"Status: {response.status}, {response.customMessage}");
                        onComplete?.Invoke(response);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to parse response: {ex.Message}");
                        onComplete?.Invoke(null);
                    }
                }
                else
                {
                    Debug.LogError("Received an empty or null response");
                }
            }
        }
    }
}

[System.Serializable]
public abstract class AbstractRequest
{
    public string action;
}

[System.Serializable]
public abstract class AbstractResponse
{
    public string status;
    public string customMessage;
}