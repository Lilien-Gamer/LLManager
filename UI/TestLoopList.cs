using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//测试loopList的代码
public class TestData
{
    public string name;
    public TestData(string n)
    {
        name = n;
    }
}

public class TestView:ICustomGridItem<TestData>
{
    Text text;

    public override void Init(GameObject go)
    {
        base.Init(go);
        text = go.transform.Find("Text").GetComponent<Text>();
    }

    public override void UpdateView()
    {
        base.UpdateView();
        if(info == null)
        {
            text.text = "数据为空";
            return;
        }
        text.text = info.name;
    }
}

public class TestLoopList:MonoBehaviour
{
    //这俩都是拖拽赋值
    public RectTransform rect;
    public GameObject go;

    private void Start()
    {
        TestData[] datas = new TestData[]
        {
            new TestData("111"), new TestData("222"),
            new TestData("333"), new TestData("444"),
            new TestData("555"), new TestData("666"),
            new TestData("777"), new TestData("888"),
            new TestData("999"), new TestData("101010"),
            new TestData("111111"), new TestData("121212"),
            new TestData("131313"), new TestData("14141414"),
        };
        MyLoopList<TestData, TestView> v = new MyLoopList<TestData, TestView>();
        v.Init(datas, new Vector2(10, 10), 200, 150, 2, 30, go, rect);
    }
}
