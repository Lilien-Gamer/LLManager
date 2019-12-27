using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//一个模板子类，直接复制
// class ViewModel:ViewBase
// {
//     public override void Init()
//     {
//         base.Init();
//     }
// 
//     public override void OnFirstOpen() {
//         base.OnFirstOpen();
//     }
// 
//     public override void OnShow() {
//         base.OnShow();
//     }
// 
//     public override void OnHide() {
//         base.OnHide();
//     }
// 
//     public override void OnDispose() {
//         base.OnDispose();
//     }
// }

/// <summary>
/// 在wndbase中必须设置wnd的层级
/// </summary>
public class ViewBase
{
    #region 静态显示遮罩方法（还没有实现)
    public static void  ShowBlackMask()
    {

    }
    public static void ShowMask()
    {

    }
    #endregion

    public Dictionary<string, Component> childGoDic ;


    protected enum FalseType
    {
        hide,
        destroy
    }

    protected FalseType falseType = FalseType.hide;

    public string name;
    protected GameObject go;
    protected RectTransform rect;
    protected GameLayer layer;
    protected bool isActive;
    //go是否在scene中
    protected bool isGoExcit;
    public ViewBase()
    {
        
        Init();
    }
    public virtual void Init()
    {
        isActive = false;
        isGoExcit = false;
        name = this.GetType().Name;
        ViewManager.Regist(this);

        this.layer = GameLayer.Wnd;
    }
    //测试
    void InitChildGoDic()
    {
        childGoDic = new Dictionary<string, Component>();

        ForChild(go.transform);

//         foreach(var v in childGoDic)
//         {
//             Debug.Log(v.Key);
//         }
    }

    protected T GetChild<T>(string name) where T:Component
    {
        return childGoDic[name] as T;
    }

    void ForChild(Transform trans)
    {
        if (trans.childCount == 0) return;
        else
        {
            for(int i = 0;i<trans.childCount;i++)
            {
                var child = trans.GetChild(i);
                ForChild(child);
               
                
                string name = child.name;

                if(name[0] == '#')
                {
                    //需要被记住
                    if(name.Contains("_"))
                    {
                        string[] nameArray = name.Split('_');
                        childGoDic.Add(nameArray[0].Substring(1), child.GetComponent(nameArray[1]));
                    }
                    else
                    {
                        childGoDic.Add(name.Substring(1), child);
                    }
                }
                else
                {
                    continue;
                }
  
            }
        }
    }


    public virtual void OnFirstOpen() {
        InitChildGoDic();

    }

    public virtual void OnShow() { }

    public virtual void OnHide() { }

    public virtual void OnDispose() { }

    public virtual void SetVisible(bool value)
    {
        if (isActive == value) return;
        
        else if(value == true&&!isActive)
        {
            if(!isGoExcit)
            {
                
                go = GameObject.Instantiate(ResourceManager.LoadAsset<GameObject>(ConstConfig.WndPath , name),
                    ViewManager.layerMap[layer.ToString()]);
                rect = go.GetComponent<RectTransform>();
                go.SetActive(value);
                OnFirstOpen();
                OnShow();
                isGoExcit = true;
                /*isActive = true;*/
            }
            else
            {
                go.SetActive(value);
                /*isActive = true;*/
                OnShow();
            }

        }
        else if(value == false && isActive)
        {
            //
            if(falseType == FalseType.hide)
            go.SetActive(false);
            else if( falseType == FalseType.destroy)
            {
                GameObject.Destroy(go);
                isGoExcit = false;
            }

            /*isActive = false;*/
            OnHide();
        }

        isActive = value;
    }
    public GameObject GetGameObject(string path)
    {
        return go.transform.Find(path).gameObject;
    }

    public Transform GetTransform(string path)
    {
        return go.transform.Find(path);
    }
    public Image GetImage(string path)
    {
        return go.transform.Find(path).GetComponent<Image>();
    }
    public Text GetText(string path)
    {
        return go.transform.Find(path).GetComponent<Text>();
    }
    public RectTransform GetRectTransform(string path)
    {
        return go.transform.Find(path).GetComponent<RectTransform>();
    }
    public T GetComponent<T>(string path)
    {
        return go.transform.Find(path).GetComponent<T>();
    }

    public bool GetActive()
    {
        return isActive;
    }
    public void AddUIButton(string path, Action<GameObject> callBack)
    {
        UIListener uiListener =  UIListener.GetUIListener(go.transform.Find(path).gameObject);
        uiListener.AddClick(callBack);
    }
}