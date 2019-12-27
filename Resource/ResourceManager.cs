using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Object = UnityEngine.Object;


//资源存储时 以名字为唯一标识， 所以不同的资源 名字不可以相同！！
public class ResourceManager
{

    public static ResourceManager instance = new ResourceManager();
    private ResourceManager()
    {
        
    } 
    //加载过的资源都存在这个字典里,注意资源的名字一定不能相同
    public static Dictionary<string,Object> assetMap = new Dictionary<string, UnityEngine.Object>();

    //path是非完整路径
    public static T LoadAsset<T>(string path, string name) where T: UnityEngine.Object
    {
        T asset = null;
        if (!assetMap.ContainsKey(name))
        {
            asset = Resources.Load<T>(path+name);
            assetMap.Add(name, asset);
        }
        else
        {
            asset = assetMap[name] as T;
        }

            
        if (asset == null) Debug.LogError($"加载的资源为空——{path+name}");
      
        return asset;
    }


}
