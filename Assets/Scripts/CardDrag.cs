using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 m_StartDragPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Запомним где начали тянуть
        m_StartDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Проверяем сколько протянули карту - в дюймах :-)
        float delta = (eventData.position.x - m_StartDragPosition.x) / Screen.dpi;
        if (delta <= -0.125f)
        {
            EventBus.SendEvent(EventBus.EventID.CardSwiped, -1);
        }
        else if (delta >= 0.125f)
        {
            EventBus.SendEvent(EventBus.EventID.CardSwiped, 1);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
