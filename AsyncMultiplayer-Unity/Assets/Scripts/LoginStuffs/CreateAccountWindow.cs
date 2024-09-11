using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;

public class CreateAccountWindow : MonoBehaviour
{
    private UIDocument uiDocument;
    private string url = "http://127.0.0.1/edsa-webdev/Login.php";

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        TextField email = root.Q<TextField>("Email");
        TextField username = root.Q<TextField>("Username");
        TextField password = root.Q<TextField>("Password");
        Button submitButton = root.Q<Button>("Submit");
        Button toLoginButton = root.Q<Button>("ToLogin");
        submitButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(CreateAccount(new CreateAccountRequest
            {
                email = email.value,
                username = username.value,
                password = password.value
            }));
        });
        toLoginButton.RegisterCallback<ClickEvent>(evt =>
        {
            UIPanelManager.Instance.ChangeSourceAsset(UIPanelManager.Instance.loginWindowAsset);
        });
    }
    private IEnumerator CreateAccount(CreateAccountRequest newRequest)
    {
        // Convert the request to a JSON string
        string newEntry = JsonUtility.ToJson(newRequest, true);
        
        // add to request form
        List<IMultipartFormSection> form = new();
        form.Add(new MultipartFormDataSection("newEntry", newEntry));
        
        // send request
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            // wait for request
            webRequest.timeout = 10;
            yield return webRequest.SendWebRequest();
            
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
                        CreateAccountResponse response = JsonUtility.FromJson<CreateAccountResponse>(webRequest.downloadHandler.text);
                        Debug.Log($"Account created. {response.status} - {response.customMessage}");
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
}
[System.Serializable]
public class CreateAccountRequest 
{
    public string action = "createAccount";
    public string email;
    public string username;
    public string password;
}

[System.Serializable]
public class CreateAccountResponse
{
    public string status;
    public string customMessage;
}
