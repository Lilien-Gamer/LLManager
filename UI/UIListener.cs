﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class UIListener : MonoBehaviour, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler
{
    
    Action<GameObject> onClick;
    Action<GameObject> onEnter;
    Action<GameObject> onExit;
    public object param;
    public static UIListener GetUIListener(GameObject go)
    {
        var uiListener = go.GetComponent<UIListener>();
        if ( uiListener == null)
           uiListener = go.AddComponent<UIListener>();

        return uiListener;
    }
    public void AddClick(Action<GameObject> callBack)
    {
        
        onClick = callBack;
           
    }
    public void AddEnter(Action<GameObject> callBack)
    {
       
        onEnter = callBack;
    }
    public void AddExit(Action<GameObject> callBack)
    {
        
        onExit = callBack;
    }
    

    public void OnPointerClick(PointerEventData eventData)
    {

        onClick?.Invoke(gameObject);
        
    }

//     public void OnPointerEnter(PointerEventData eventData)
//     {
//         onEnter?.Invoke(gameObject);
//     }
// 
//     public void OnPointerExit(PointerEventData eventData)
//     {
//         onExit?.Invoke(gameObject);
//     }
}

