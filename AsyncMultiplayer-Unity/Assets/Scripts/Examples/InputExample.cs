using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputExample : MonoBehaviour
{
    private UIDocument uiDocument;
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        TextField textField = new TextField("Enter your name");
        root.Add(textField);
        Button button = new();
        button.text = "Submit";
        button.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log(textField.text);
        });

        // for (int i = 0; i < 10; i++)
        // {
        //     int index = i;
        //     Button button = new();
        //     button.text = "BUtton: " + i;
        //     button.RegisterCallback<ClickEvent>(evt =>
        //     {
        //         Debug.Log("Button " + index + " clicked");
        //     });
        //     root.Add(button);
        // }
    }
}
