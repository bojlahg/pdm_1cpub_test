using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GamesFactory : MonoBehaviour
{
    public enum GameNames
    {
        Eatable
    }

    public void OnEnable()
    {
        EventBus.onReceiveEvent += OnReceiveEvent;
    }

    public void OnDisable()
    {
        EventBus.onReceiveEvent -= OnReceiveEvent;
    }

    public void CreateGame(Transform parent, GameNames gn)
    {
        switch(gn)
        {
            case GameNames.Eatable:
                Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/EatableGame.prefab").Completed += (aoh)=> { OnGamePrefabLoadDone(parent, aoh); };
                break;
        }
    }

    private void OnGamePrefabLoadDone(Transform parent, AsyncOperationHandle<GameObject> aoh)
    {
        if (aoh.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject newgo = GameObject.Instantiate(aoh.Result, Vector3.zero, Quaternion.identity, parent);
            RectTransform rt = newgo.GetComponent<RectTransform>();
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
        }

        Addressables.Release(aoh);
    }

    private void OnReceiveEvent(EventBus.EventID id, object[] args)
    {
        if(id == EventBus.EventID.CreateGame)
        {
            CreateGame((Transform)args[0], (GameNames)args[1]);
        }
    }
}
