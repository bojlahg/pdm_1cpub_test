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
    public Animator m_CardAnimator;

    private GameObject[] m_Hearts;
    private EatableGame.CardConfig m_CardConfig;


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
        switch (id)
        {
            case EventBus.EventID.InitUI: InitUI((EatableGame.EatableConfig)args[0]); break;
            case EventBus.EventID.ShowCard: ShowCard((EatableGame.CardConfig)args[0]); break;
            case EventBus.EventID.DropCard: DropCard(); break;
            case EventBus.EventID.CardMove: CardMove((int)args[0], (bool)args[1]); break;
        }
    }

    private void InitUI(EatableGame.EatableConfig cfg)
    {
        m_Hearts = new GameObject[cfg.lives];

        for(int i = 0; i < m_Hearts.Length; ++i)
        {
            m_Hearts[i] = GameObject.Instantiate(m_HeartPrefab, m_HeartPrefab.transform.parent);
        }        

        m_HeartPrefab.SetActive(false);

        m_Group.SetActive(true);
    }

    private void ShowCard(EatableGame.CardConfig cc)
    {
        m_CardConfig = cc;
        Addressables.LoadAssetAsync<Sprite>(cc.address).Completed += OnCardLoadDone;
    }

    private void OnCardLoadDone(AsyncOperationHandle<Sprite> aoh)
    {
        if(aoh.Status == AsyncOperationStatus.Succeeded)
        {
            m_EatableImage.sprite = aoh.Result;
            m_CardText.text = m_CardConfig.name;
            m_CardAnimator.SetInteger("State", 0);
        }
        Addressables.Release(aoh);
    }

    private void DropCard()
    {
        m_CardAnimator.SetInteger("State", 10);
    }

    private void CardMove(int dir, bool correct)
    {
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
