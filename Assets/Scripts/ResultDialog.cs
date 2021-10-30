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
        // �������������
        EventBus.onReceiveEvent += OnEventReceived;
    }

    private void OnDisable()
    {
        // �����������
        EventBus.onReceiveEvent -= OnEventReceived;
    }

    private void OnEventReceived(EventBus.EventID id, object[] args)
    {
        // ��������� �������
        switch (id)
        {
            case EventBus.EventID.InitUI: InitUI((EatableGame.EatableConfig)args[0]); break;
            case EventBus.EventID.GameFinished: GameFinished((int)args[0]); break;
        }
    }

    private void InitUI(EatableGame.EatableConfig cfg)
    {
        // �������������
        m_Group.SetActive(false);
    }

    private void GameFinished(int score)
    {
        // ���� ��������� ������ ���� � ���������� ����
        m_ScoreText.text = string.Format("����: {0}", score);
        m_Group.SetActive(true);
    }

    public void RetryButtonClick()
    {
        // ������ ������ ����������� ����
        m_Group.SetActive(false);
        EventBus.SendEvent(EventBus.EventID.RestartGame);
    }
}
