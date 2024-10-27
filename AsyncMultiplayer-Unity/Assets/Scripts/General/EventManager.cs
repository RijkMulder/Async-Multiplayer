using UnityEngine.Events;

namespace Events {
    public static class EventManager
    {
        public static event UnityAction<UserData> MoneyUpdate;
        public static event UnityAction<TileData> CropUpdate;
        
        public static void OnUserDataUpdate(UserData data) => MoneyUpdate?.Invoke(data);
        public static void OnCropUpdate(TileData data) => CropUpdate?.Invoke(data);
    }
}

