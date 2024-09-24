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
    private string url = "http://127.0.0.1/edsa-webdev/CheckLogin.php";

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
        Button forgotPasswordButton = root.Q<Button>("ForgotPassword");
        submitButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(packageManager.SendRequest(JsonUtility.ToJson(new LoginAccountRequest
            {
                email = email.value,
                password = password.value
            })));
        });
        newAccountButton.RegisterCallback<ClickEvent>(evt =>
        {
            UIPanelManager.Instance.ChangeSourceAsset(UIPanelManager.Instance.assets[1].asset);
        });
        // forgotPasswordButton.RegisterCallback<ClickEvent>(evt =>
        // {
        //     StartCoroutine(LoginAccount(JsonUtility.ToJson(new LoginAccountRequest
        //     {
        //         email = email.value,
        //         password = password.value
        //     })));
        // });
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
public class ResetPasswordRequest
{
    public string action = "resetPassword";
    public string email;
} 
