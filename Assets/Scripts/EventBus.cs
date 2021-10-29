using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    public enum EventID
    {
        CreateGame,

        InitUI,
        ShowCard,
        CardAppeared,
        CardSwiped,
        CardMove,
        DropCard,
        GameFinished,
        CardMoved,
        Result_RetryButtonClick,
    }

    public delegate void OnReceiveEvent(EventBus.EventID id, object[] args);
    public static OnReceiveEvent onReceiveEvent;


    public static void SendEvent(EventBus.EventID id, params object[] args)
    {
        if(onReceiveEvent != null)
        {
            onReceiveEvent.Invoke(id, args);
        }
    }
}
