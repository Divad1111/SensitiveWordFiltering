using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        BuildFromCode();

        BuildFromFile();

        SerializeTest();
    }

    void BuildFromCode()
    {
        //清除敏感词，重新构建
        SensitiveWordFiltering.Clear();

        //初始化调用一次
        string[] sensitiveWords = new string[3] { "日", "日本", "日本人" }; //自测试的敏感词而已
        SensitiveWordFiltering.Add(sensitiveWords);


        //检查关键字
        Debug.Log(SensitiveWordFiltering.Filter("日"));
        Debug.Log(SensitiveWordFiltering.Filter("本人"));
        Debug.Log(SensitiveWordFiltering.Filter("日本"));
        Debug.Log(SensitiveWordFiltering.Filter("日本人"));
    }

    void BuildFromFile()
    {
        //清除敏感词，重新构建
        SensitiveWordFiltering.Clear();

        var startTime = Time.realtimeSinceStartup;

        var filePath = Path.Combine(Application.dataPath, "SensitiveWord.txt");
        var fileContent = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(fileContent))
        {
            Debug.LogErrorFormat("文件：{0}内容为空。", filePath);
            return;
        }

        StringBuilder sb = new StringBuilder(fileContent);
        sb.Replace(System.Environment.NewLine, ",");
        var splitedSensitiveWords = sb.ToString().Split(',');
        
        SensitiveWordFiltering.Add(splitedSensitiveWords);
        Debug.LogFormat("构建的花费时间:{0}", Time.realtimeSinceStartup - startTime);

        //检查关键字
        startTime = Time.realtimeSinceStartup;
        Debug.Log(SensitiveWordFiltering.Filter("去氧麻黃堿或安非他命"));
        Debug.LogFormat("关键检查耗时:{0}", Time.realtimeSinceStartup - startTime);
    }

    void SerializeTest()
    {
        //保存路径
        var savedFilePath = Path.Combine(Application.dataPath, "offlineSensitiveWord.dat");

        //将当前敏感词串行化
        SensitiveWordFiltering.Serialize(savedFilePath);

        //清除敏感词，重新构建
        SensitiveWordFiltering.Clear();

        var startTime = Time.realtimeSinceStartup;
        //从文件反向串行化
        SensitiveWordFiltering.Deserialize(savedFilePath);
        Debug.LogFormat("反向串行化用时{0}", Time.realtimeSinceStartup - startTime);

        //检查关键字
        startTime = Time.realtimeSinceStartup;
        Debug.Log(SensitiveWordFiltering.Filter("去氧麻黃堿或安非他命"));
        Debug.LogFormat("关键检查耗时:{0}", Time.realtimeSinceStartup - startTime);
    }

}
