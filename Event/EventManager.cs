﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventManager
{
    private static Dictionary<EventType, Action<dynamic>> eventMap = new Dictionary<EventType, Action<dynamic>>();
    static public void RegistEvent(EventType type, Action<dynamic> action)
    {
        if (!eventMap.ContainsKey(type))
            eventMap.Add(type, action);
        else
        {   
            eventMap[type] -= action;
            eventMap[type] += action;
        }
        
    }

    static public void UnRegistEvent(EventType type, Action<dynamic> action)
    {
        if (!eventMap.ContainsKey(type))
        {
            Debug.LogError("注销不存在的事件类型.");
            return;
        }
              

        eventMap[type] -= action;
    }


    static public void ExecuteEvent(EventType type,object param = null)
    {
        if( !eventMap.ContainsKey(type))
        {
            return;
        }
        else
        {
            eventMap[type]?.Invoke(param);
        }
// 
        

    }
    static public void ExecuteEvent(EventType type, int param)
    {
        ExecuteEvent<int>(type, param);
    }

    static public void ExecuteEvent<T>(EventType type, T param) where T:struct
    {
        if (!eventMap.ContainsKey(type))
        {
            return;
        }
        eventMap[type]?.Invoke(param);
    }


}
