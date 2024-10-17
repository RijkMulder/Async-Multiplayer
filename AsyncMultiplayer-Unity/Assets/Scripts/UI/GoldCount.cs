using System;
using UnityEngine;
using Events;
using TMPro;
public class GoldCount : MonoBehaviour
{
    private TMP_Text goldText;

    private void OnEnable()
    {
        goldText = GetComponent<TMP_Text>();
        EventManager.MoneyUpdate += UpdateGold;
    }

    private void OnDisable()
    {
        EventManager.MoneyUpdate -= UpdateGold;
    }

    private void UpdateGold(UserData data)
    {
        goldText.text = data.gold.ToString();
    }
}
