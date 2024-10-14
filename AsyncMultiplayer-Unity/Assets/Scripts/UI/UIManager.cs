using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject[] panels;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            foreach (GameObject p in panels)
            {
                if (p != pauseMenu)
                {
                    p.SetActive(false);
                }
            }
            if (!pauseMenu.activeSelf) gameUI.SetActive(true);
        }
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        foreach (GameObject p in panels)
        {
            if (p != panel)
            {
                p.SetActive(false);
            }
        }
    }
}
