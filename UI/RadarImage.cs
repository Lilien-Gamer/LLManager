using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//逆时针读取数据n个点的数据 , point 也应逆时针顺序赋值

//通过调用OnDataUpdate方法传入数据更改显示

//使用时先拖拽赋值n个点，代表雷达的顶点，挂载该脚本的父物体下放置子物体作为顶点
public class RadarImage:Graphic
{
    //顶点位置
    
    private Vector3[] vertexes;

    //拖拽赋值 , 该 recttransform的中心点为 顶点位置
    public RectTransform[] points;

    //数据变化时引起顶点数据变化,传入顶点数据的百分比
    public void OnDataUpdate(float[] percents)
    {
        

        if(percents.Length != points.Length)
        {
            Debug.LogError("雷达图顶点数据数量与顶点数量不一致");
            return;
        }
        if(vertexes == null)
        vertexes = new Vector3[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            float percent = percents[i];
            vertexes[i] = points[i].anchoredPosition * percent;

        }

        Refresh();
    }
    //调用刷新则 刷新显示 雷达图， 应该在合适的时机调用
    public void Refresh()
    { 
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        //颜色为该组件上的纯色
        if (vertexes == null) return;

        foreach(var v in vertexes)
        {
            vh.AddVert(v, color, Vector2.zero);
        }

        for(int i = 1;i< points.Length - 1;i++)
        {
            vh.AddTriangle(0, i, i + 1);

        }
    }

}

