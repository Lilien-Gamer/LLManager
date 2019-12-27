using System;
using System.Collections.Generic;
using UnityEngine;

//给粒子系统的go挂上,用于粒子执行完后 回归对象池的回调
public class OnEffectStop:MonoBehaviour
{
    

    public void OnParticleSystemStopped()
    {

        //Main.instance.effectManager.SaveEffect(name, gameObject);
    }
}
