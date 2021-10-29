using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EatableGame : MonoBehaviour
{
    [System.Serializable]
    public class CardConfig
    {
        public string name, address;
        public bool eatable;
    }

    [System.Serializable]
    public class EatableConfig
    {
        public int lives = 3;
        public float timeout = 3;
        public CardConfig[] cards;
    }

    private EatableConfig m_EatableConfig;

    private int m_Lives = 0, m_CardIndex = 0, m_StarCount = 0;
    private float m_Timer = 0;
    private CardConfig[] m_CardArray;
    private bool m_GameStarted = false, m_TimerEnabled = false;


    private void Start()
    {
        Addressables.LoadAssetAsync<TextAsset>("Assets/Config/eatable.json").Completed += OnGameConfigLoadDone;
    }

    private void OnEnable()
    {
        EventBus.onReceiveEvent += OnReceiveEvent;
    }

    private void OnDisable()
    {
        EventBus.onReceiveEvent -= OnReceiveEvent;
    }

    private void OnGameConfigLoadDone(AsyncOperationHandle<TextAsset> aoh)
    {
        if (aoh.Status == AsyncOperationStatus.Succeeded)
        {
            m_EatableConfig = JsonUtility.FromJson<EatableConfig>(aoh.Result.text);
        }
        else
        {
            m_EatableConfig = new EatableConfig();
        }

        EventBus.SendEvent(EventBus.EventID.InitUI, m_EatableConfig);

        StartGame();

        Addressables.Release(aoh);
    }

    private void OnReceiveEvent(EventBus.EventID id, object[] args)
    {
        switch(id)
        {
            case EventBus.EventID.CardAppeared:
                CardAppeared();
                break;

            case EventBus.EventID.CardSwiped:
                CardSwiped((int)args[0]);
                break;

            case EventBus.EventID.CardMoved:
                CardFinishedAnim();
                break;
        }
    }

    private void StartGame()
    {
        m_Lives = m_EatableConfig.lives;
        m_CardIndex = 0;
        m_StarCount = 0;
        m_TimerEnabled = false;
        m_Timer = m_EatableConfig.timeout;

        m_CardArray = new CardConfig[m_EatableConfig.cards.Length];

        System.Array.Copy(m_EatableConfig.cards, m_CardArray, m_EatableConfig.cards.Length);
        m_CardArray.Shuffle();

        m_GameStarted = true;

        EventBus.SendEvent(EventBus.EventID.ShowCard, m_CardArray[m_CardIndex]);
    }

    private void CardAppeared()
    {
        m_TimerEnabled = true;
    }

    private void CardSwiped(int dir)
    {
        bool correct = false;
        if ((m_CardArray[m_CardIndex].eatable && dir == 1) || (!m_CardArray[m_CardIndex].eatable && dir == -1))
        {
            ++m_StarCount;
            correct = true;
        }
        else
        {
            --m_Lives;
        }

        EventBus.SendEvent(EventBus.EventID.CardMove, dir, correct);
    }

    private void CardFinishedAnim()
    {
        m_TimerEnabled = true;
        m_Timer = 0;
        ++m_CardIndex;
        if (m_Lives == 0 || m_CardIndex == m_CardArray.Length)
        {
            // конец игры - жизни кончились либо карты в колоде
            EventBus.SendEvent(EventBus.EventID.GameFinished);
        }
        else
        {
            EventBus.SendEvent(EventBus.EventID.ShowCard, m_CardArray[m_CardIndex]);
        }
    }

    private void Update()
    {
        if(m_GameStarted && m_TimerEnabled)
        {
            m_Timer -= Time.deltaTime;
            if(m_Timer <= 0)
            {
                m_TimerEnabled = false;
                EventBus.SendEvent(EventBus.EventID.DropCard);
            }
        }
    }
}
