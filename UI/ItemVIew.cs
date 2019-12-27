using System;
using System.Collections.Generic;

/// <summary>
/// 继承该类，重写updateview方法，实例化对象后调用Init方法
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public abstract class ItemView<T>
{
    protected T info;
    public int index;
    public ItemView( )
    {
        
    }

    //init里获得 view的各个组件
    public abstract void Init(UnityEngine.GameObject go, int index);


    public virtual void SetInfo(T info)
    {
        this.info = info;
        UpdateView();
    }

    public abstract void UpdateView();
    

    
}
    


