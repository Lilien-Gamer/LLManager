﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUtil
{


    public static GameObject GetGameObject(GameObject go , string path)
    {
        Transform t =  go.transform.Find(path);
        if( t==null)
        {
            Debug.LogError("该路径下不存在该物体:" + path);
            return null;
        }
        else
        {
            return t.gameObject;
        }

    }
    public static RectTransform GetRectTransform(GameObject go, string path)
    {
        var goo = GetGameObject(go, path);
        return goo?.GetComponent<RectTransform>();
    }
    public static Image GetImage(GameObject go, string path)
    {
        var goo = GetGameObject(go, path);
        return goo?.GetComponent<Image>();
    }
    public static Text GetText(GameObject go, string path)
    {
        var goo = GetGameObject(go, path);
        return goo?.GetComponent<Text>();
    }
    public static Transform GetTransform(GameObject go, string path)
    {
        return GetGameObject(go, path)?.transform;

    }

    public static UIListener SetUIOnClick( GameObject go, Action<GameObject> callBack )
    {
        var uiL = UIListener.GetUIListener(go);
        uiL.AddClick(callBack);
        return uiL;
    }

    //该ui的锚点位于屏幕左下角 与 屏幕坐标系一致
    public static Vector2 WorldToUGUI(Vector3 position)
    {
        Vector2 world2ScreenPos = Camera.main.WorldToScreenPoint(position);     
        return world2ScreenPos;

    }
    //让某个图变灰色 且不可点击
    public static void SetImageGray(Image image)
    {
        image.color = Color.gray;
        image.raycastTarget = false;

    }
    // 图片变白 还可以点击
    public static void SetImageWhite(Image image)
    {
        image.color = Color.white;
        image.raycastTarget = true;
    }

}