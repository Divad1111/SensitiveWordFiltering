using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {   
        //初始化调用一次
        string[] sensitiveWords = new string[3] { "日", "日本", "日本人" }; //自测试的敏感词而已
        SensitiveWordFiltering.Build(sensitiveWords);
        

        //检查关键字
        Debug.Log(SensitiveWordFiltering.Filter("日"));
        Debug.Log(SensitiveWordFiltering.Filter("本人"));
        Debug.Log(SensitiveWordFiltering.Filter("日本"));
        Debug.Log(SensitiveWordFiltering.Filter("日本人"));


        //清除敏感词，重新构建从文件构建
        SensitiveWordFiltering.Clear();

        var filePath = Path.Combine(Application.dataPath, "SensitiveWord.txt");
        var fileContent = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(fileContent))
        {
            Debug.LogErrorFormat("文件：{0}内容为空。", filePath);
            return;
        }

        StringBuilder sb = new StringBuilder(fileContent);
        sb.Replace(System.Environment.NewLine, ",");

        var startTime = Time.realtimeSinceStartup;
        SensitiveWordFiltering.Build(sb.ToString().Split(','));
        Debug.LogFormat("构建的花费时间:{0}", Time.realtimeSinceStartup - startTime);

        //检查关键字
        startTime = Time.realtimeSinceStartup;
        Debug.Log(SensitiveWordFiltering.Filter("去氧麻黃堿或安非他命"));
        Debug.LogFormat("关键检查耗时:{0}", Time.realtimeSinceStartup - startTime);
    }

}
