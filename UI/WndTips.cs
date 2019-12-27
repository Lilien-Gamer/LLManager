using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class WndTips:ViewBase
{
    public static WndTips instance;

    public override void Init()
    {
        base.Init();
        layer = GameLayer.Tips;
        instance = this;
        SetVisible(true);
    }

    public override void OnFirstOpen() {
        base.OnFirstOpen();
    }

    public override void OnShow() {
        base.OnShow();
    }

    public override void OnHide() {
        base.OnHide();
    }

    public override void OnDispose() {
        base.OnDispose();
    }

    #region 显示提示信息
    

    public void ShowTips(string content)
    {
        var tipsText = GetChild<Text>("tipsText");
        childGoDic["tips"].gameObject.SetActive(true);
        tipsText.text = content;
        TimeManager.RegistOneTime((id) =>
        {
            childGoDic["tips"].gameObject.SetActive(false);
        }, 3f);
    }

    #endregion
}
