using System;
using UnityEngine;
using TMPro;
using Events;
public class CropUI : MonoBehaviour, IBuildingData
{
    [SerializeField]private TMP_Text cropIndicator;
    private string lastUpdateTime;
    private int currentCropAmnt;
    
    public int growIntervalTime;
    public int growAmntPerInterval;
    public int maxCropAmnt;

    private void OnEnable()
    {
        EventManager.CropUpdate += UpdateCrop;
    }

    private void OnDisable()
    {
        EventManager.CropUpdate -= UpdateCrop;
    }

    private void Update()
    {
        GetInterval(lastUpdateTime);
    }
    public void SetCurrentAmount(int currentAmount)
    {
        currentAmount = Mathf.Clamp(currentAmount + currentCropAmnt, 0, maxCropAmnt);
        currentCropAmnt = currentAmount;
        cropIndicator.text = $"{currentAmount}/{maxCropAmnt}";
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
        
        // update crop amount
        if (intervalsPassed > 0)SetCurrentAmount(intervalsPassed * growAmntPerInterval);
    }
    private void UpdateCrop(TileData data)
    {
        // check if this is the tile to update
        Vector2 positionToCheck = new Vector2(data.posX, data.posY);
        if (transform.position.x == positionToCheck.x && transform.position.z == positionToCheck.y) 
        {
            currentCropAmnt = 0;
            SetCurrentAmount(0);
        }
    }
}
