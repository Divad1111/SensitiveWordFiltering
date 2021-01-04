# 使用说明
1. Assets/SensitiveWordFiltering.cs 文件拷贝到自己工程即可
2. SensitiveWordFiltering.Build(string cvsFilePath,char splitChar = ',') 构建敏感词DFA树
3. SensitiveWordFiltering.Filter(string checkString, char replaceChar = '*', bool caseSensitive = false) 检查checkString是否是米敏感词

实例代码:
```csharp
//初始化调用一次
string[] sensitiveWords = new string[3] { "日", "日本", "日本人" }; //自测试的敏感词而已
SensitiveWordFiltering.Build(sensitiveWords);


//检查关键字
Debug.Log(SensitiveWordFiltering.Filter("日"));
Debug.Log(SensitiveWordFiltering.Filter("本人"));
Debug.Log(SensitiveWordFiltering.Filter("日本"));
Debug.Log(SensitiveWordFiltering.Filter("日本人"));
```
