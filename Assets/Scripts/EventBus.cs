using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    // Доступные события
    public enum EventID
    {
        CreateGame,
        InitGame,
        InitUI,
        ShowCard,
        CardAppeared,
        CardSwiped,
        CardMove,
        DropCard,
        GameFinished,
        CardMoved,
        LiveCountChanged,
        StarCountChanged,
        TimerChanged,
        RestartGame,
    }

    public delegate void OnReceiveEvent(EventBus.EventID id, object[] args);
    public static OnReceiveEvent onReceiveEvent;

    public static void SendEvent(EventBus.EventID id, params object[] args)
    {
        // Послать событие
        /*
        string log = string.Format("Event: {0}", id);
        foreach (object arg in args)
        {
            log += string.Format(", {0}", arg.ToString());
        }
        Debug.Log(log);
        */
        if (onReceiveEvent != null)
        {
            onReceiveEvent.Invoke(id, args);
        }
    }
}
