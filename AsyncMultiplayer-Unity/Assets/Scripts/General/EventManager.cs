using UnityEngine.Events;

namespace Events {
    public static class EventManager
    {
        public static event UnityAction<UserData> MoneyUpdate;
        
        public static void OnUserDataUpdate(UserData data) => MoneyUpdate?.Invoke(data);
    }
}

