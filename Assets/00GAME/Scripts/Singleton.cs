using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance => _instance;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this.GetComponent<T>();
        }
        else if (instance.GetInstanceID() != this.GetInstanceID())
        {
            Destroy(this);
        }
    }
}
