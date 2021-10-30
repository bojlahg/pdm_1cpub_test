using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimEvents : MonoBehaviour
{
    public void CardAppeared()
    {
        // Карта появилась и готова
        EventBus.SendEvent(EventBus.EventID.CardAppeared);
    }

    public void MoveFinished()
    {
        // Карта закончила движение
        EventBus.SendEvent(EventBus.EventID.CardMoved);
    }
}
