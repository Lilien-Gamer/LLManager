﻿using System;
using System.Collections.Generic;


public class ModelManager
{
    public static Dictionary<string, ModelBase> modelMap = new Dictionary<string, ModelBase>();

    public static void Regist(ModelBase model )
    {
        if( modelMap.ContainsKey(model.name))
        {
            return;
        }
        else
        {
            modelMap.Add(model.name, model);
        }
    }

    public static ModelBase Get( string name)
    {
        if( !modelMap.ContainsKey(name))
        {
            UnityEngine.Debug.LogError("不含有该model" + name);
            return null;
        }

        return modelMap[name];
    }

    public static T Get<T>( ) where T:class
    {

        return modelMap[typeof(T).Name] as T;
    }
}


public  class ModelBase
{
    public string name;
    public ModelBase()
    {
        name = this.GetType().Name;
        ModelManager.Regist(this);
        Init();        
    }
    protected virtual void Init()
    {

    }
}

