using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//背景遮罩组件，挂载后，不可显示图片，没有颜色，rebuild为空，没有overDraw
public class MyMask : Graphic,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        print("点击");
    }

    public override void Rebuild(CanvasUpdate update)
    {
        
    }
}
