using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonXample : MonoBehaviour
{
    private void Start()
    {
        MakeJson();
    }

    private void MakeJson()
    {
        string newHighscore = JsonUtility.ToJson(new HighScore { name = "John", score = 40 }, true);
    }
}

[System.Serializable]
public class HighScore
{
    public string name;
    public int score;
}
