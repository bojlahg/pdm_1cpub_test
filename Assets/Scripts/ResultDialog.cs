using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDialog : MonoBehaviour
{
    public Text m_ScoreText;
    public GameObject m_Group;

    private void OnEnable()
    {
        EventBus.onReceiveEvent += OnReceiveEvent;
    }

    private void OnDisable()
    {
        EventBus.onReceiveEvent -= OnReceiveEvent;
    }

    private void OnReceiveEvent(EventBus.EventID id, object[] args)
    {
        if (id == EventBus.EventID.InitUI)
        {
            InitUI((EatableGame.EatableConfig)args[0]);
        }
    }

    private void InitUI(EatableGame.EatableConfig cfg)
    {
        m_Group.SetActive(false);
    }

    public void RetryButtonClick()
    {
        EventBus.SendEvent(EventBus.EventID.Result_RetryButtonClick);
    }
}
