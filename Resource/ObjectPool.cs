using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//对象池， 需要时自个儿new一个, 仅支持 能直接new出来的ob
public class ObjectPool<T> where T: class,new()
{

    Action<T> OnGet;
    Action<T> OnSave;

    public ObjectPool(Action<T> onGet = null, Action<T> onSave = null   )
    {
        OnGet = onGet;
        OnSave = onSave;
    }

    public Stack<T> pool = new Stack<T>();
    //go
    public static Dictionary<string, List<T>> goMap = new Dictionary<string, List<T>>();

    
    public T GetObject( )
    {
        T ob = null;
        if(pool.Count > 0 )
        {
            ob= pool.Pop();
        }
        else
        {
            ob= new T();
        }
        /*OnGet(ob);*/
        return ob;
    }

//     public T GetObject(Action<T> action)
//     {
//         T ob = GetObject();
//         action(ob);
//         return ob;
//     }

    public  void SaveOject(T ob)
    {
        /*OnSave(ob);*/
        pool.Push(ob);
    }



    public  void Clear()
    {
        pool.Clear();
    }
}
