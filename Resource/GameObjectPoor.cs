using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    Action<GameObject> OnGet;
    Action<GameObject> OnSave;
    Action<GameObject> OnInstiante;
    GameObject goPrefab;
    public GameObjectPool(GameObject goPrefab,Action<GameObject> onGet = null, Action<GameObject> onSave = null)
    {
        OnGet = onGet;
        OnSave = onSave;
        this.goPrefab = goPrefab;
    }
    public void SetOnInst(Action<GameObject> OnInstiante)
    {
        this.OnInstiante = OnInstiante;
    }

    public Stack<GameObject> pool = new Stack<GameObject>();

    public void Clear()
    {
        foreach(var g in pool)
        {
            GameObject.Destroy(g);
        }
        pool.Clear();
    }

    int count = 0;

    public GameObject GetObject()
    {
        GameObject ob = null;
        if (pool.Count > 0)
        { 
            ob = pool.Pop();
        }
        else
        {

            ob = GameObject.Instantiate(goPrefab);
            OnInstiante?.Invoke(ob);
        }
        /*        OnGet(ob);*/
        count++;

        
        return ob;
    }

    public void SaveOject(GameObject ob)
    {
        /*OnSave(ob);*/
        pool.Push(ob);
    }

}
