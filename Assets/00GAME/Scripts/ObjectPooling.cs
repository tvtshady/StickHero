using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> : MonoBehaviour where T: Component
{
    private static ObjectPooling<T> _instance;

    public static ObjectPooling<T> instance => _instance;


    [SerializeField] T bullet;
    [SerializeField] int size,count;

    public T getT() { return bullet; }

    Stack<T> pools = new Stack<T>();

    T clone;

    private void Awake()
    {
        count = 0;
        if (_instance == null)
            _instance = this;
        else if (this.GetInstanceID() != instance.GetInstanceID())
            Destroy(this.gameObject);
    }



    void CreatePool()
    {
        for (int i = 0; i < size; i++)
        {
            clone = Instantiate(bullet, transform);
            clone.gameObject.SetActive(false);
            pools.Push(clone);
        }
    }


    public T GetObjectFromPool()
    {
        if (pools.Count > 0)
        {
            clone = pools.Pop();
        }
        else
        {
            clone = Instantiate(bullet, transform);
            clone.gameObject.SetActive(false);
        }
        return clone;

    }


    public void ReturnToPool(T b)
    {
        b.gameObject.SetActive(false);
        pools.Push(b);

    }

    public virtual void ReturnAllPooling()
    {
        foreach (Transform g in this.transform)
        {
            if(g.gameObject.activeSelf)
            {
                ReturnToPool(g.GetComponent<T>());
                count++;
            }
        }
    }

}
