using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviourSingleton<App>
{
    public Transform m_GameContainer;

    private void Start()
    {
        // Стартуем игру
        EventBus.SendEvent(EventBus.EventID.CreateGame, m_GameContainer, "Assets/Prefabs/EatableGame.prefab");
    }
}
