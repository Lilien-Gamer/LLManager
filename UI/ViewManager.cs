﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameLayer
{
    BackRound = 0,
   
    Wnd,

    UI,
    
    Tips
}


public class ViewManager
{
    
    public static Dictionary<string, ViewBase> wndMap = new Dictionary<string, ViewBase>();
    public static Dictionary<string, Transform> layerMap = new Dictionary<string, Transform>();


    public static void Regist(ViewBase wnd)
    {
        if (wndMap.ContainsKey(wnd.name))
        {
            Debug.LogError("已经存有该窗口实例对象--" + wnd.name);
            return;
        }
        wndMap.Add(wnd.name, wnd);
        
    }

    public static void Init()
    {
        
        Transform uiParent = GameObject.Find("UI").transform;
        GameObject canvas = uiParent.Find("Canvas").gameObject;

        //创建窗口画布
        var enums = Enum.GetNames(typeof(GameLayer));
        for(int i=0;i<enums.GetLength(0);i++)
        {

            GameObject go = GameObject.Instantiate(canvas, uiParent);
            go.name = enums[i].ToString();
            layerMap.Add(enums[i], go.transform);
            
            
        }
       

        GameObject.Destroy(canvas);
    }

    public static ViewBase Get(string name)
    {
        
        return wndMap[name];
    }

    public static T Get<T>( ) where T:ViewBase
    {
        
        return wndMap[typeof(T).Name] as T;
    }



}
