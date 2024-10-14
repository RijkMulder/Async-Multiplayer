using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Asset
{
    public VisualTreeAsset asset;
    public MonoScript script;
}

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance { get; private set; }
    public Asset[] assets;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private Dictionary<VisualTreeAsset, MonoScript> assetsDic = new();
    private void Awake()
    {
        Instance = this;
        uiDocument = GetComponent<UIDocument>();
        CreateAssetsDic();
    }

    public void ChangeSourceAsset(VisualTreeAsset newAsset)
    {
        uiDocument.visualTreeAsset = newAsset;
        foreach (KeyValuePair<VisualTreeAsset, MonoScript> asset in assetsDic)
        {
            // set everything false that isnt new asset
            Type componentType = asset.Value.GetClass();
            Component assetScript = GetComponent(componentType);
            if (asset.Key != newAsset && assetScript is MonoBehaviour monoBehaviour)
            {
                monoBehaviour.enabled = false;
            }
            
            // set correct asset true
            else if (assetScript is MonoBehaviour monoBehaviour2)
            {
                monoBehaviour2.enabled = true;
            }
        }
    }

    public void IsLoggedIn()
    {
        uiDocument.visualTreeAsset = null;
        foreach (KeyValuePair<VisualTreeAsset, MonoScript> asset in assetsDic)
        {
            Type componentType = asset.Value.GetClass();
            Component assetScript = GetComponent(componentType);
            if (assetScript is MonoBehaviour monoBehaviour)
            {
                monoBehaviour.enabled = false;
            }
        }
        SceneManager.LoadScene("GameScene");
    }

    private void CreateAssetsDic()
    {
        foreach (Asset asset in assets)
        {
            assetsDic.Add(asset.asset, asset.script);
        }
    }
}