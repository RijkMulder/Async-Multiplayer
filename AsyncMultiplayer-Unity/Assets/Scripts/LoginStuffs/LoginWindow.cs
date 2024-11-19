using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class LoginWindow : MonoBehaviour
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
        TextField password = root.Q<TextField>("Password");
        Button submitButton = root.Q<Button>("Submit");
        Button newAccountButton = root.Q<Button>("MakeAccount");
        submitButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(LoginRequest(new LoginAccountRequest
            {
                email = email.value,
                password = password.value
            }));
        });
        newAccountButton.RegisterCallback<ClickEvent>(evt =>
        {
            UIPanelManager.Instance.ChangeSourceAsset(UIPanelManager.Instance.assets[1].asset);
        });
    }

    private IEnumerator LoginRequest(LoginAccountRequest request)
    {
        yield return StartCoroutine(packageManager.WebRequest<LoginAccountRequest, LoginAccountResponse>(request,
            response =>
            {
                if (response.status != "loginSuccesfull") return;
                PlayerPrefs.SetString("token", response.token);
                UIPanelManager.Instance.IsLoggedIn();
            }, url));
    }
    private IEnumerator ResetPasswordRequest(ResetPasswordRequest request)
    {
        yield return StartCoroutine(packageManager.WebRequest<ResetPasswordRequest, ResetPasswordResponse>(request,
            response =>
            {
                if (response.status != "resetPassword") return;
                UIPanelManager.Instance.ChangeSourceAsset(UIPanelManager.Instance.assets[2].asset);
            }, url));
    }
}

[System.Serializable]
public class LoginAccountRequest : AbstractRequest
{
    public string email;
    public string password;
    public LoginAccountRequest()
    {
        action = "loginAccount";
    }
}
[Serializable]
public class ResetPasswordRequest : AbstractRequest
{
    public string email;
    public ResetPasswordRequest()
    {
        action = "resetPassword";
    }
}
[System.Serializable]
public class LoginAccountResponse : AbstractResponse
{
    public string token;
}
[Serializable]
public class ResetPasswordResponse : AbstractResponse
{
}
