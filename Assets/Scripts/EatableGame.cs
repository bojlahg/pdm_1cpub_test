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

    private int m_LiveCount = 0, m_CardIndex = 0, m_ScoreCount = 0, m_TimerInt = 0;
    private float m_Timer = 0;
    private CardConfig[] m_CardArray;
    private bool m_GameStarted = false, m_TimerEnabled = false;

    private void OnEnable()
    {
        // Подписываемся
        EventBus.onReceiveEvent += OnReceiveEvent;
    }

    private void OnDisable()
    {
        // Отписываемя
        EventBus.onReceiveEvent -= OnReceiveEvent;
    }

    private void OnReceiveEvent(EventBus.EventID id, object[] args)
    {
        // Обрабатываем события
        switch(id)
        {
            case EventBus.EventID.InitGame: InitGame(); break;
            case EventBus.EventID.CardAppeared: CardAppeared(); break;
            case EventBus.EventID.CardSwiped: CardSwiped((int)args[0]); break;
            case EventBus.EventID.CardMoved: CardMoved(); break;
            case EventBus.EventID.RestartGame: StartGame(); break;
        }
    }

    private void InitGame()
    {
        // Загрузим конфигурацию
        Addressables.LoadAssetAsync<TextAsset>("Assets/Config/eatable.json").Completed += OnGameConfigLoadDone;
    }

    private void OnGameConfigLoadDone(AsyncOperationHandle<TextAsset> aoh)
    {
        // Загрузили конфигурацию
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

    private void StartGame()
    {
        // Начинаем игру
        m_LiveCount = m_EatableConfig.lives;
        m_CardIndex = 0;
        m_ScoreCount = 0;
        m_TimerEnabled = false;
        m_Timer = m_EatableConfig.timeout;
        m_TimerInt = Mathf.CeilToInt(m_Timer);

        m_CardArray = new CardConfig[m_EatableConfig.cards.Length];

        System.Array.Copy(m_EatableConfig.cards, m_CardArray, m_EatableConfig.cards.Length);
        m_CardArray.Shuffle();

        m_GameStarted = true;

        EventBus.SendEvent(EventBus.EventID.StarCountChanged, m_ScoreCount);
        EventBus.SendEvent(EventBus.EventID.LiveCountChanged, m_LiveCount);
        EventBus.SendEvent(EventBus.EventID.TimerChanged, m_TimerInt);
        EventBus.SendEvent(EventBus.EventID.ShowCard, m_CardArray[m_CardIndex]);
    }

    private void CardAppeared()
    {
        // Карта появилась в своем рабочем месте
        m_TimerEnabled = true;
        m_Timer = m_EatableConfig.timeout;
        m_TimerInt = Mathf.CeilToInt(m_Timer);
        EventBus.SendEvent(EventBus.EventID.TimerChanged, m_TimerInt);
    }

    private void CardSwiped(int dir)
    {
        // Карту свайпнули
        m_TimerEnabled = false;
        bool correct = false;
        if ((m_CardArray[m_CardIndex].eatable && dir == 1) || (!m_CardArray[m_CardIndex].eatable && dir == -1))
        {
            ++m_ScoreCount;
            EventBus.SendEvent(EventBus.EventID.StarCountChanged, m_ScoreCount);
            correct = true;
        }
        else
        {
            --m_LiveCount;
            EventBus.SendEvent(EventBus.EventID.LiveCountChanged, m_LiveCount);
        }

        EventBus.SendEvent(EventBus.EventID.CardMove, dir, correct);
    }

    private void CardMoved()
    {
        // Карта закончила движение (анимацию)
        ++m_CardIndex;
        if (m_LiveCount == 0 || m_CardIndex == m_CardArray.Length)
        {
            // конец игры - жизни кончились либо карты в колоде
            EventBus.SendEvent(EventBus.EventID.GameFinished, m_ScoreCount);
        }
        else
        {
            EventBus.SendEvent(EventBus.EventID.ShowCard, m_CardArray[m_CardIndex]);
        }
    }

    private void Update()
    {
        // Обновления таймера
        if(m_GameStarted && m_TimerEnabled)
        {
            m_Timer -= Time.deltaTime;

            int timerint = Mathf.CeilToInt(m_Timer);

            if(timerint < 0)
            {
                timerint = 0;
            }

            if(m_TimerInt != timerint)
            {
                m_TimerInt = timerint;
                EventBus.SendEvent(EventBus.EventID.TimerChanged, m_TimerInt);
            }

            if (m_Timer <= 0)
            {
                m_TimerEnabled = false;
                --m_LiveCount;
                EventBus.SendEvent(EventBus.EventID.LiveCountChanged, m_LiveCount);
                EventBus.SendEvent(EventBus.EventID.DropCard);
            }
        }
    }
}
