using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class WordNode
{   
    public char word;
    public WordNode parent;
    public List<WordNode> children = new List<WordNode>();
    public bool isBreakUpNode;

    public WordNode AddChildWorkNode(char word, bool isBreakUpNode = false)
    {
        var newWordNode = new WordNode();
        newWordNode.word = word;
        newWordNode.parent = this;
        newWordNode.isBreakUpNode = isBreakUpNode;

        children.Add(newWordNode);

        return newWordNode;
    }


    public WordNode GetChildWordNode(char word, bool caseSensitive = false)
    {
        if (children == null)
            return null;

        var upperCaseWord = char.ToUpper(word);
        for (var i = 0; i < children.Count; i++)
        {
            var cwn = children[i];
            if (cwn == null)
                continue;

            if (caseSensitive)
            {
                if (cwn.word == word)
                    return cwn;
            }
            else
            {
                if (char.ToUpper(cwn.word) == upperCaseWord)
                    return cwn;
            }
        }

        return null;
    }

    public bool IsLeafNode()
    {
        return children == null || children.Count <= 0;
    }
}

public static class SensitiveWordFiltering
{
    private static WordNode _root = new WordNode();

    public static void Build(string cvsFilePath, char splitChar = ',')
    {
        var fileContent = File.ReadAllText(cvsFilePath);
        if (string.IsNullOrEmpty(fileContent))
        {
            Debug.LogErrorFormat("文件：{0}内容为空。", cvsFilePath);
            return;
        }
        Build(fileContent.Split(splitChar));
    }

    public static void Build(string[] sensitiveWords)
    {
        if (sensitiveWords == null || sensitiveWords.Length <= 0)
        {
            Debug.LogError("输出参数错误：sensitiveWords为空。");
            return;
        }

        for (var swsIndex = 0; swsIndex < sensitiveWords.Length; swsIndex++)
        {
            var words = sensitiveWords[swsIndex];
            if (string.IsNullOrEmpty(words))
                continue;

            var curWN = _root;
            var wordsLength = words.Length;
            for (var wIndex = 0; wIndex < wordsLength; wIndex++)
            {
                bool isLastWord = wIndex + 1 == wordsLength;
                var word = words[wIndex];
                var childWN = curWN.GetChildWordNode(word);
                if (childWN != null)
                {
                    if (!childWN.isBreakUpNode)
                        childWN.isBreakUpNode = isLastWord;

                    curWN = childWN;
                }
                else
                {   
                    curWN = curWN.AddChildWorkNode(word, isLastWord);
                }
            }
        }
    }

    public static void Clear()
    {
        _root.children.Clear();
    }

    public static string Filter(string checkString, char replaceChar = '*', bool caseSensitive = false)
    {   
        if (string.IsNullOrEmpty(checkString))
        {
            Debug.LogError("checkString 是空。");
            return string.Empty;
        }

        var rst = new StringBuilder(checkString);
        var curWN = _root;
        var startIndex = -1;
        for(var csIndex = 0; csIndex < checkString.Length; csIndex++)
        {
            var checkWord = checkString[csIndex];
            var childWN = curWN.GetChildWordNode(checkWord, caseSensitive);
            var isFound = childWN != null && (childWN.isBreakUpNode || childWN.IsLeafNode());
            if (isFound)
            {
                if (startIndex < 0)
                    startIndex = csIndex;

                for (var i = startIndex; i <= csIndex; i++)
                    rst[i] = replaceChar;

                startIndex = csIndex + 1;

                if (childWN.IsLeafNode())
                {
                    curWN = _root;
                    continue;
                }
            }

            if (childWN == null)
            {
                curWN = _root;
                startIndex = -1;
                continue;
            }

            if (startIndex < 0)
                startIndex = csIndex;

            curWN = childWN;
        }

        return rst.ToString();
    }
}
