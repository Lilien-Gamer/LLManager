using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//点在该ui外时 禁用该ui 的 go
 public class OutRectClose: MonoBehaviour
{
    Action<GameObject> action;

    public void AddAction(Action<GameObject> action)
    {
        this.action = action;
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            bool b = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition);
            if (!b)
            {
                if(null == action)
                gameObject.SetActive(false);
                else
                {
                    action?.Invoke(gameObject);
                }
            }
        }
        
    }
}
