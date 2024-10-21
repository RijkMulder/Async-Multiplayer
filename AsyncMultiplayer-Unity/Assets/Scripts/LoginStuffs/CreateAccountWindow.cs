using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.Text.RegularExpressions;
public class CreateAccountWindow : MonoBehaviour
{
    private PackageManager packageManager;
    private UIDocument uiDocument;
    private string url = "http://127.0.0.1/edsa-webdev/Account/AccountManager.php";

    private void Start()
    {
        packageManager = PackageManager.Instance;
    }

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
            CreateAccountRequest request = new CreateAccountRequest
            {
                email = email.value,
                username = username.value,
                password = password.value
            };
            StartCoroutine(CreateRequest(request));
        });
        toLoginButton.RegisterCallback<ClickEvent>(evt =>
        {
            UIPanelManager.Instance.ChangeSourceAsset(UIPanelManager.Instance.assets[0].asset);
        });
        email.RegisterCallback<ChangeEvent<string>>(evt => {
            string filtered = FilterToEmailCharacters(evt.newValue);
            if (evt.newValue != filtered) {
                email.SetValueWithoutNotify(filtered);
            }
        });
    }
    private string FilterToEmailCharacters(string input) 
    {
        // valid chars
        Regex emailRegex = new Regex(@"^[a-zA-Z0-9@._\-]+$");

        string result = "";
        foreach (char c in input) {
            if (emailRegex.IsMatch(c.ToString())) {
                result += c;
            }
        }
        return result;
    }

    private IEnumerator CreateRequest(CreateAccountRequest request)
    {
        yield return StartCoroutine(packageManager.WebRequest<CreateAccountRequest, CreateAccountResponse>(request,
            response =>
            {
                if (response.status != "loginSuccesfull") return;
                PlayerPrefs.SetString("token", response.token);
                UIPanelManager.Instance.IsLoggedIn();
            }, url));
    }
}
[System.Serializable]
public class CreateAccountRequest : AbstractRequest
{
    public string email;
    public string username;
    public string password;
    public CreateAccountRequest()
    {
        action = "createAccount";
    }
}
[System.Serializable]
public class CreateAccountResponse : AbstractResponse
{
    public string token;
}
