using System;
using UnityEngine;
using TMPro;
public class CropUI : MonoBehaviour, IBuildingData
{
    private TMP_Text cropIndicator;
    private string lastUpdateTime;
    
    public int growIntervalTime;
    public int growAmntPerInterval;
    public int maxCropAmnt;

    private void Awake()
    {
        cropIndicator = GetComponent<TMP_Text>();
    }

    public void SetCurrentAmount(int currentAmount)
    {
        currentAmount = Mathf.Clamp(currentAmount, 0, maxCropAmnt);
        cropIndicator.text = $"Crop: {currentAmount}/{maxCropAmnt}";
    }

    public void GetInterval<t>(t lastUpdate)
    {
        // get time passed since last update
        DateTime oldTime = DateTime.ParseExact(lastUpdate.ToString(), "yyyy-MM-dd HH:mm:ss", null);
        DateTime currentTime = DateTime.Now;
        TimeSpan timePassed = currentTime - oldTime;
        
        // get number of intervals passed
        int intervalsPassed = (int)timePassed.TotalSeconds / growIntervalTime;
        int secondsRemaining = (int)timePassed.TotalSeconds % growIntervalTime;
        int secondsPassed = intervalsPassed * growIntervalTime;
        
        // set new time
        DateTime newTime = oldTime.AddSeconds(secondsPassed);
        lastUpdateTime = newTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
