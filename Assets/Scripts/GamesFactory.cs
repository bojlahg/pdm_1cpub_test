using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GamesFactory : MonoBehaviour
{
    public void OnEnable()
    {
        // Подписываемся
        EventBus.onReceiveEvent += OnReceiveEvent;
    }

    public void OnDisable()
    {
        // Отписываемя
        EventBus.onReceiveEvent -= OnReceiveEvent;
    }

    public void CreateGame(Transform parent, string gameassetaddress)
    {
        // Тут тоже используем аддресуемые чтобы было универсальнее
        Addressables.LoadAssetAsync<GameObject>(gameassetaddress).Completed += (aoh)=> { OnGamePrefabLoadDone(parent, aoh); };
    }

    private void OnGamePrefabLoadDone(Transform parent, AsyncOperationHandle<GameObject> aoh)
    {
        // Загрузили префаб игры
        if (aoh.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject newgo = GameObject.Instantiate(aoh.Result, Vector3.zero, Quaternion.identity, parent);
            RectTransform rt = newgo.GetComponent<RectTransform>();
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;

            EventBus.SendEvent(EventBus.EventID.InitGame);
        }

        Addressables.Release(aoh);
    }

    private void OnReceiveEvent(EventBus.EventID id, object[] args)
    {
        // Обработка событий
        if(id == EventBus.EventID.CreateGame)
        {
            CreateGame((Transform)args[0], (string)args[1]);
        }
    }
}
