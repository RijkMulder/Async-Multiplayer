using System;
using UnityEngine;
using Events;
using TMPro;
public class StatCount : MonoBehaviour
{
    private TMP_Text statText;
    [SerializeField] private string statName;

    private void OnEnable()
    {
        statText = GetComponent<TMP_Text>();
        EventManager.MoneyUpdate += UpdateGold;
    }

    private void OnDisable()
    {
        EventManager.MoneyUpdate -= UpdateGold;
    }

    private void UpdateGold(UserData data)
    {
        // get value of tracked stat in data by name
        int value = (int)data.GetType().GetField(statName).GetValue(data);
        string newText = $"{statName}: {value}";
        
        // set text to new value
        statText.text = newText;
    }
}
