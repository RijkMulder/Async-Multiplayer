using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class LoginWindow : MonoBehaviour
{
    private UIDocument uiDocument;
    private string url = "http://127.0.0.1/edsa-webdev/Login.php";
    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        TextField email = root.Q<TextField>("Email");
        TextField password = root.Q<TextField>("Password");
        Button submitButton = root.Q<Button>("Submit");
        Button newAccountButton = root.Q<Button>("MakeAccount");
        submitButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(LoginAccount(new LoginAccountRequest
            {
                email = email.value,
                password = password.value
            }));
        });
        newAccountButton.RegisterCallback<ClickEvent>(evt =>
        {
            UIPanelManager.Instance.ChangeSourceAsset(UIPanelManager.Instance.registerWindowAsset);
        });
    }

    private IEnumerator LoginAccount(LoginAccountRequest request) 
    {
        string newEntry = JsonUtility.ToJson(request, true);
        List<IMultipartFormSection> form = new();
        form.Add(new MultipartFormDataSection("newEntry", newEntry));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class LoginAccountRequest
{
    public string action = "loginAccount";
    public string email;
    public string password;
}

[System.Serializable]
public class LoginAccountResponse
{
    public string status;
    public string customMessage;
    public string token;
}
