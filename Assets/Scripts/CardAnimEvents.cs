using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimEvents : MonoBehaviour
{
    public void CardAppeared()
    {

    }

    public void MoveFinished()
    {
        EventBus.SendEvent(EventBus.EventID.CardMoved);
    }
}
