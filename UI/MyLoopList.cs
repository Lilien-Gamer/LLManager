using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//loopList中单独处理单个数据的view载体，继承后重写updateview方法，重写init方法
public class ICustomGridItem<T> where T : class
{
    protected T info;
    protected GameObject go;
    

    public virtual void Init(GameObject go)
    {
        this.go = go;
    }

    public virtual void UpdateView()
    {

    }
    public void SetInfo(T info)
    {
        this.info = info;
        UpdateView();
    }

}

//使用时new出该对象，调用init方法
//dataType为loopList处理的单个数据类型，itemType为 负责单独处理每个数据项的view对象

public class MyLoopList<DataType, ItemType> where ItemType : ICustomGridItem<DataType>, new() where DataType:class 
{
    RectTransform scrollRect;

    RectTransform contentTR;
    public GameObject ItemGo;
    
    public int GridHeight;          //
    public int GridWidth;
    public int wholeItemCount;     //格子总数

    public int lineCount;           //每一行的格子数
    public Vector2 space;

    int line;                       //面板里的行数
    float contentHeight;            //content根据grid数量初始化后的高度

    float lastContentRTy;

    int itemCountInView;            //面板里可容纳的最大Item数量
    float scrollViewHeight;         //面板的初始高度

    int firstLineIndex;             //当前面板中的第一个Item的index

    LinkedList<RectTransform> itemList = new LinkedList<RectTransform>(); 
    int itemDataIndex;

    Dictionary<GameObject, ICustomGridItem<DataType>> gridList = new Dictionary<GameObject, ICustomGridItem<DataType>>();

    DataType[] datas;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="datas">该loopList中显示的所有数据</param>
    /// <param name="space">间隔大小</param>
    /// <param name="gridHeight">每个格子的高度</param>
    /// <param name="gridWidth">每个格子的宽度</param>
    /// <param name="lineCount">每一行的格子数量</param>
    /// <param name="wholeItemCount">总的格子数量</param>
    /// <param name="itemGo">用来显示子项的go对象，要求锚点为左上角，中心点为左上角</param>
    /// <param name="scrollRect">unity的scrollRect组件</param>
    public void Init(DataType[] datas, Vector2 space, int gridHeight,
    int gridWidth, int lineCount, int wholeItemCount, GameObject itemGo, RectTransform scrollRect)
    {
        this.scrollRect = scrollRect;

        this.datas = datas;
        GridHeight = gridHeight;
        GridWidth = gridWidth;
        this.lineCount = lineCount;
        this.wholeItemCount = wholeItemCount;
        this.space = space;
        ItemGo = itemGo;
        
        contentTR = scrollRect.Find("content").GetComponent<RectTransform>();
        if (contentTR == null)
        {
            Debug.LogError("scrollRect应该有叫'content'的子物体");
            return;
        }
        
        var rect = scrollRect.GetComponent<RectTransform>().rect;
        scrollViewHeight = rect.height;
        Vector2 size = Vector2.zero;
        size.x = rect.width;
        contentHeight = (space.y + GridHeight) * wholeItemCount / lineCount; //设置content的高度
        size.y = contentHeight;

        contentTR.sizeDelta = size; //content的总大小被设置
        //上一帧的 content y轴位置
        lastContentRTy = GetContentPositionY();

       
        //将预设的宽高设置
        (ItemGo.transform as RectTransform).sizeDelta = new Vector2(GridWidth, GridHeight);
        
        //计算面板里能显示多少行  +2 防止穿帮
        line = (int)(scrollViewHeight /(GridHeight+space.x)) + 2;

        itemCountInView = line * lineCount; // view窗口里显示的item数量
        for (int i = 1; i < itemCountInView + 1; i++)
        {
            GameObject go = GameObject.Instantiate(ItemGo, contentTR);
            var item = new ItemType();
            item.Init(go);
            gridList.Add(go,item );

            Vector3 pos = Vector3.zero;

            int count = (i % lineCount);
            if (count == 0) count = lineCount;

            pos.x = (i - 1) % lineCount * GridWidth + count * space.x;
            int hang = (int)(Mathf.Ceil(i * 1.0f / lineCount));

            pos.y = -((hang - 1) * GridHeight + hang * space.y);
            pos.z = 0;
            var rectt = (go.transform as RectTransform);
            rectt.anchoredPosition = pos;
            itemList.AddLast(rectt);
            
        }
        //第一个go的索引  最后一个go的索引（对应的数据索引）
        firstLineIndex = 0;
       
        scrollRect.GetComponent<ScrollRect>().onValueChanged.AddListener(OnValueChanged);

        ItemGo.SetActive(false);

        Refresh();
    }

    
    public void OnValueChanged(Vector2 pos)
    {
        float nowContentRTy = GetContentPositionY();
        //往上移
        bool isUp = false;

        if (nowContentRTy - lastContentRTy > 0)
        {
            if (scrollViewHeight + GetItemPositionY(false) + nowContentRTy > GridHeight
            && contentHeight - nowContentRTy > scrollViewHeight)
            {
                isUp = true;
                MoveItemGo(isUp);
                firstLineIndex += lineCount;
            }

        }
        else
        {
            if (-GetItemPositionY(true) - nowContentRTy > space.y
            && nowContentRTy > 0)
            {
                isUp = false;
                MoveItemGo(isUp);
                firstLineIndex -= lineCount;
            }
        }
              
        lastContentRTy = nowContentRTy;
    }
    //true 表示是处理第一个
    float GetItemPositionY(bool first)
    {
        float y;
        if (first)
            y = itemList.First.Value.anchoredPosition.y;
        else
            y = itemList.Last.Value.anchoredPosition.y;

        return y;
    }
    float GetContentPositionY()
    {
        return contentTR.anchoredPosition.y;
    }

    void SetInfo(ICustomGridItem<DataType> item, int index)
    {
        if(index >=0 && index < datas.Length)
        {
            item.SetInfo(datas[index]);
        }
        else
        {
            item.SetInfo(null);
        }
    }

    void MoveItemGo(bool up)
    {
        if (up)
        {//tou
            for (int i = 0; i < lineCount; i++)
            {
                var item = itemList.First.Value;

                Vector3 pos = item.anchoredPosition;

                pos.y -= line * (GridHeight + space.y);

                item.anchoredPosition = pos;

                itemList.RemoveFirst();

                itemList.AddLast(item);

                //注入数据
                int index = firstLineIndex +(line-1)*lineCount + lineCount + i;
                

                SetInfo(gridList[item.gameObject], index);
                
            }
        }
        else
        {
            for (int i = 0; i < lineCount; i++)
            {
                var item = itemList.Last.Value;

                Vector3 pos = item.anchoredPosition;

                pos.y += line * (GridHeight + space.y);

                item.anchoredPosition = pos;

                itemList.RemoveLast();
                itemList.AddFirst(item);
                int index = firstLineIndex - i - 1;

                SetInfo(gridList[item.gameObject], index);

            }
        }
    }
    void Refresh()
    {
        var value = itemList.First;
        for (int i = firstLineIndex; i < firstLineIndex + itemCountInView; i++)
        {
            if (i >= datas.Length) return;
            if (value == null) return;
            gridList[value.Value.gameObject].SetInfo(datas[i]);
            gridList[value.Value.gameObject].UpdateView();
            value = value.Next;
        }

    }
} 
