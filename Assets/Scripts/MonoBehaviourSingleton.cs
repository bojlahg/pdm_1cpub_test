using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;
    private static bool m_Destroyed = false;
    protected virtual bool isPersistent { get { return false; } }


    public static T instance
    {
        get
        {
            if(m_Destroyed)
            {
                return null;
            }
            if(m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType<T>();
                if (m_Instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    m_Instance = go.AddComponent<T>();                    
                }

                MonoBehaviourSingleton<T> mbs = m_Instance.GetComponent<MonoBehaviourSingleton<T>>();
                if (mbs.isPersistent)
                {
                    GameObject.DontDestroyOnLoad(m_Instance);
                }
            }
            return m_Instance;
        }
    }

    public virtual void OnDestroy()
    {
        m_Destroyed = true;
    }
}