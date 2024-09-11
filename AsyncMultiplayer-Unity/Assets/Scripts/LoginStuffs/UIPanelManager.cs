using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance { get; private set; }
    [SerializeField] private UIDocument uiDocument;
    public VisualTreeAsset loginWindowAsset;
    public VisualTreeAsset registerWindowAsset;
    
    [SerializeField] private LoginWindow loginWindow;
    [SerializeField] private CreateAccountWindow createAccountWindow;
    private void Awake()
    {
        Instance = this;
        uiDocument = GetComponent<UIDocument>();
    }

    public void ChangeSourceAsset(VisualTreeAsset newAsset)
    {
        uiDocument.visualTreeAsset = newAsset;
        if (newAsset == loginWindowAsset)
        {
            loginWindow.enabled = true;
            createAccountWindow.enabled = false;
        }
        else
        {
            loginWindow.enabled = false;
            createAccountWindow.enabled = true;
        }
    }
}
