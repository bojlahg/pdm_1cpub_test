using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameScreen : MonoBehaviour
{
    public Image m_EatableImage;
    public Text m_CardText, m_TimerText, m_StarCountText;
    public GameObject m_HeartPrefab, m_Group;
    public Animator m_CardAnimator, m_TimerAnimator, m_ScoreAnimator;
    public CardDrag m_CardDrag;

    private GameObject[] m_Hearts;
    private EatableGame.CardConfig m_CardConfig;


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
        // Обработка событий
        switch (id)
        {
            case EventBus.EventID.InitUI: InitUI((EatableGame.EatableConfig)args[0]); break;
            case EventBus.EventID.ShowCard: ShowCard((EatableGame.CardConfig)args[0]); break;
            case EventBus.EventID.DropCard: DropCard(); break;
            case EventBus.EventID.CardAppeared: CardAppeared(); break;
            case EventBus.EventID.CardSwiped: CardSwiped(); break;
            case EventBus.EventID.CardMove: CardMove((int)args[0], (bool)args[1]); break;
            case EventBus.EventID.LiveCountChanged: LiveCountChanged((int)args[0]); break;
            case EventBus.EventID.StarCountChanged: ScoreCountChanged((int)args[0]); break;
            case EventBus.EventID.TimerChanged: TimerChanged((int)args[0]); break;
        }
    }

    private void InitUI(EatableGame.EatableConfig cfg)
    {
        // Инициализация
        m_Hearts = new GameObject[cfg.lives];

        for(int i = 0; i < m_Hearts.Length; ++i)
        {
            m_Hearts[i] = GameObject.Instantiate(m_HeartPrefab, m_HeartPrefab.transform.parent);
        }        

        m_HeartPrefab.SetActive(false);

        m_CardDrag.enabled = false;

        m_Group.SetActive(true);
    }

    private void LiveCountChanged(int lc)
    {
        // Изменение кол-ва жизней
        for (int i = 0; i < m_Hearts.Length; ++i)
        {
            bool act = i < lc;
            if (m_Hearts[i].activeSelf != act)
            {
                if (act)
                {
                    RectTransform rt = m_Hearts[i].GetComponent<RectTransform>();
                    rt.localScale = Vector3.one;
                    m_Hearts[i].SetActive(true);
                }
                else
                {
                    StartCoroutine(HeartAnim(m_Hearts[i]));
                }
            }
        }
    }

    private IEnumerator HeartAnim(GameObject go)
    {
        // Процедурная анимация сердец
        RectTransform rt = go.GetComponent<RectTransform>();
        float anim = 0;
        while (anim < 0.5f)
        {
            yield return null;
            anim += Time.deltaTime;
            float t = anim * 2;
            rt.localScale = Vector3.one * (1 - t);
        }
        rt.localScale = Vector3.zero;
        rt.gameObject.SetActive(false);
    }

    private void ScoreCountChanged(int sc)
    {
        // Изменение количества очков
        m_StarCountText.text = sc.ToString();
        m_ScoreAnimator.SetTrigger("Ping");
    }

    private void TimerChanged(int t)
    {
        // Изменение значения таймера
        m_TimerText.text = t.ToString();
        m_TimerAnimator.SetTrigger("Tick");
    }

    private void ShowCard(EatableGame.CardConfig cc)
    {
        // Начинаем загрузку картинки
        m_CardConfig = cc;
        Addressables.LoadAssetAsync<Sprite>(cc.address).Completed += OnCardLoadDone;
    }

    private void OnCardLoadDone(AsyncOperationHandle<Sprite> aoh)
    {
        // Загрузка картинки завершена
        if(aoh.Status == AsyncOperationStatus.Succeeded)
        {
            m_EatableImage.sprite = aoh.Result;
            m_CardText.text = m_CardConfig.name;
            m_CardAnimator.SetInteger("State", 0);

            // Очищаем память от использованной картинки
            Resources.UnloadUnusedAssets();
        }
        Addressables.Release(aoh);
    }

    private void CardAppeared()
    {
        // Включаем обработчик Drag
        m_CardDrag.enabled = true;
    }   
    
    private void CardSwiped()
    {
        // Отключаем обработчик Drag
        m_CardDrag.enabled = false;
    }

    private void DropCard()
    {
        // Запускаем анимацию - время вышло
        m_CardAnimator.SetInteger("State", 10);
    }

    private void CardMove(int dir, bool correct)
    {
        // Запускаем анимацию движения карты с подсветкой правильного ответа
        int state = 0;

        if(dir == 1)
        {
            state = correct ? 1 : 2;
        }
        else if (dir == -1)
        {
            state = correct ? -1 : -2;
        }
        m_CardAnimator.SetInteger("State", state);
    }
}
