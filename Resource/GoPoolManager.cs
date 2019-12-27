using System;
using System.Collections.Generic;
using GameObject = UnityEngine.GameObject;

//GameObject的池子管理
//自己调用 addpool添加池子
 public class GoPoolManager
{
    public Dictionary<string, GameObjectPool> GoPools;
    public GoPoolManager()
    {
        GoPools = new Dictionary<string, GameObjectPool>();
    }

    /// <summary>
    /// 添加资源池
    /// </summary>
    /// <param name="name">key，名字</param>
    /// <param name="go">资源池中的go对象</param>
    public void AddPool(string name, GameObject go)
    {
        GoPools.Add(name, new GameObjectPool(go));
    }

    public GameObject GetGo(string name)
    {
        GameObject go = null;
        if(GoPools.ContainsKey(name) )
        {
            go = GoPools[name].GetObject();
        }
        else
        {
            UnityEngine.Debug.Log("当前go对象池不存在该资源---" + name);
        }
        return go;
    }

    public void SaveGo(string name, GameObject go)
    {
        GoPools[name].SaveOject(go);
    }

    public void Clear()
    {
        foreach(var v in GoPools.Values)
        {
            v.Clear();
        }
    }
}

