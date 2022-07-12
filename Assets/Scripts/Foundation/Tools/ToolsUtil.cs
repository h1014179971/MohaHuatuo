using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public class ToolsUtil
{


    /// <summary> 
    /// 截取文本，区分中英文字符，中文算两个长度，英文算一个长度
    /// </summary>
    /// <param name="str">待截取的字符串</param>
    /// <param name="length">需计算长度的字符串</param>
    /// <returns>string</returns>
    public static string GetSubString(string str, int length)
    {
        string temp = str;
        int j = 0;
        int k = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
            {
                j += 2;
            }
            else
            {
                j += 1;
            }
            if (j <= length)
            {
                k += 1;
            }
            if (j > length)
            {
                return temp.Substring(0, k) + "..";
            }
        }
        return temp;
    }
}
