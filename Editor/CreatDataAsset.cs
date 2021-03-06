﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using Excel;
using UnityEngine;
using UnityEditor;
using System.IO;

using System;

using Object = System.Object;
using System.Text;

//excel文件请放置Assets下的Configs文件夹下
public class CreateDataWindow:EditorWindow
{

    [MenuItem("CreateDataAsset/StartCreat")]
    static void OpenWindow()
    {
        Rect wr = new Rect(0, 0, 500, 500);
        CreateDataWindow window = (CreateDataWindow)EditorWindow.GetWindowWithRect<CreateDataWindow>(wr, true,
            "创建数据资源");
        window.Show();
    }
    string text;
    private void OnGUI()
    {
        
        text = EditorGUILayout.TextField("输入文件名:",text);
        

        if (GUILayout.Button("生成数据类", GUILayout.Width(200)))
        {
            CreatDataAsset.Analysis(text);
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("生成数据资源", GUILayout.Width(200)))
        {
            CreatDataAsset.GetDataAsset(text);
            AssetDatabase.Refresh();
        }
    }
    
}


public class CreatDataAsset:Editor
{
    static List<string> configNames;

    static List<string> memberType = new List<string>();
    static List<string> names = new List<string>( );
    

    public static void Analysis(string fileName)
    {
        memberType.Clear();
        names.Clear();
        string path = Application.dataPath + "/Configs/" + fileName + ".xlsx";
        FileStream fileStream = File.Open(path , FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

        // 表格数据全部读取到result里
        DataSet result = excelDataReader.AsDataSet();
        var table = result.Tables[0];
        // 获取表格有多少列
        int columns = result.Tables[0].Columns.Count;
        // 获取表格有多少行
        int rows = result.Tables[0].Rows.Count;


        //取得表格中的数据 
        //取得table中所有的行
        var rowCollections = table.Rows;

        var rowCollection1 = rowCollections[1];//返回了第0行的集合

        var rowCollection2 = rowCollections[2];

        int columnLength = table.Columns.Count; //列数
        int rowLength = rowCollections.Count;
        
       
        for (int i = 0; i < columnLength; i++)
        {
            memberType.Add(rowCollection1[i].ToString());
        }

        for (int i = 0; i < columnLength; i++)
        {
            names.Add(rowCollection2[i].ToString());
        }

        BuildClass(fileName);
       
    }

    public static void GetDataAsset(string fileName)
    {
        Debug.Log(fileName);
        memberType.Clear();
        names.Clear();
        string path = Application.dataPath + "/Configs/" + fileName + ".xlsx";
        FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

        // 表格数据全部读取到result里
        DataSet result = excelDataReader.AsDataSet();
        var table = result.Tables[0];
        // 获取表格有多少列
        int columns = result.Tables[0].Columns.Count;
        // 获取表格有多少行
        int rows = result.Tables[0].Rows.Count;


        //取得表格中的数据
        //取得table中所有的行
        var rowCollections = table.Rows;

        var rowCollection1 = rowCollections[1];//返回了第1行的集合

        var rowCollection2 = rowCollections[2];

        int columnLength = table.Columns.Count; //列数
        int rowLength = rowCollections.Count;
    
        for (int i = 0; i < columnLength; i++)
        {
            names.Add(rowCollection2[i].ToString());
        }

        string dataCode = "";

        for(int i=3;i< rowLength;i++)
        {
            dataCode += $"\n    config = new {fileName}();";
            for (int j = 0; j < columnLength; j++)
            {
                dataCode += $"\n    config.{names[j]} = {ConvertMyExcel.ConvertString(rowCollection1[j].ToString(), rowCollections[i][j].ToString())};";
            }
            dataCode += "\n    allDatas.Add( config.id, config)\n;";
        }
        string code = "using System.Collections.Generic;\nusing System;\nnamespace DataClass\n{\n\n" +                                       
                      $"    public class {fileName}Manager : ConfigDataManager<{fileName}Manager,{fileName}>\n\n" +
                      "    {\n    "+//$"private static {fileName}Manager instance = new {fileName}Manager( );\n"+
                      //$"    public static {fileName}Manager Instance()"+"{ return instance; } "+
                      "\n    public override void Init( )\n    {\n    name = " + $"\"{fileName}\";\n" +
                      $"    {fileName} config = null;\n    {dataCode}\n"
                      + "    base.Init();\n    }\n}\n}";                          
        FileStream fs = new FileStream(Application.dataPath + "/Scripts/DataAsset/DataManager/" + fileName + "Manager.cs", FileMode.Create);//创建文件
        StreamWriter w = new StreamWriter(fs);

        w.Write(code);//追加数据
        w.Close();//释放资源,关闭文件  
        fs.Close();
     
        AssetDatabase.Refresh();

    }


    private static void BuildClass(string className)
    {

        string code = @"
using System;
using System.Collections;
using System.Collections.Generic;

namespace DataClass
{
    public class " + className+"{\n";

        string codeField = "";

        for(int i = 0;i< memberType.Count;i++)
        {
            codeField += $"        public {memberType[i]} {names[i]};\n";
        }

        code += codeField + "    }\n}";

        FileStream fs = new FileStream(Application.dataPath + "/Scripts/DataAsset/DataBaseClass/" + className + ".cs", FileMode.Create);//创建文件
        StreamWriter w = new StreamWriter(fs);

        w.Write(code);//追加数据
        w.Close();//释放资源,关闭文件  
        fs.Close();

        AssetDatabase.Refresh();
        return;

            
    }
    
}


class ConvertMyExcel
{
    

    public static string ConvertString(string stringType, string content)
    {
       
        string re = null;
        switch (stringType){
            case "int":
                re = ToInt(content);
                break;
            case "string":
                re = content;
                break;
            case "int[]":
                re = ToIntArray(content);
                break;
            case "int[][]":
                re = ToIntArrayArray(content);
                break;
            case "Dictionary<int,int[]>":
                re = ToDicIntIntArray(content);
                break;
            case "Dictionary<int,int>":
                re = ToDicIntInt(content);
                break;
            case "float":
                re =ToFloat(content);
                break;
        }
        return re;
    }
    static string ToFloat(string str)
    {
        return str+"f";
    }

    static string ToInt(string str)
    {

        return str;
    }

    static string ToIntArray(string str)
    {

        StringBuilder sb = new StringBuilder(str);
        sb.Remove(0,1);
        sb.Remove(sb.Length - 1, 1);

         
        string[] intArray = sb.ToString().Split(',');

        return $"new int[]{str}";
    }
    static string ToIntArrayArray( string str)
    {    
        StringBuilder sb = new StringBuilder(str);
        sb.Replace(" ", "");     
        List<int> index = new List<int>();
        for(int i=0;i<sb.Length;i++)
        {
            if (sb[i] == '{')
                index.Add(i);
        }
        for(int i=0;i<index.Count;i++)
        {
            sb.Insert(index[i] + i*(9), "new int[]");
        }
        sb.Insert(0, '{');
        sb.Insert(sb.Length - 1, '}');

        return $"new int[][]{sb.ToString()}";


    }
    //解析 dic<int,int[]>
    static string ToDicIntIntArray(string str )
    {
        StringBuilder sb = new StringBuilder(str);
        sb.Replace(" ", "");
        List<int> index = new List<int>();
        for (int i = 0; i < sb.Length; i++)
        {
            if (sb[i] == '{')
                index.Add(i);
        }
        for(int i=1;i<index.Count;i=i+2)
        {
            sb.Insert(index[i] + i/2 * (9), "new int[]");
        }

//         Dictionary<int, int[]> dic = new Dictionary<int, int[]>
//         {
//             {7,new int[] {500,300,400}},
//             {7,new int[] {500,300,400}}
//         };

        return "new Dictionary<int,int[]>{" + sb.ToString() + "}";
    }

    static string ToDicIntInt(string str)
    {
        
        return "new Dictionary<int,int>{" + str + "}";
    }


}
