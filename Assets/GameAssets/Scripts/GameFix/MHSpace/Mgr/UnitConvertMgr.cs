using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;
using System;
using System.Linq;
using libx;

namespace MHSpace
{
    public class UnitConvertMgr : Singleton<UnitConvertMgr>
    {
        private Dictionary<int, UnitConvert> _unitConvertDic = new Dictionary<int, UnitConvert>();
        public override void Init()
        {
            ReadFile();
        }

        private void ReadFile()
        {
            TextAsset textAsset = AssetLoader.Load<TextAsset>(Files.unitConvert);
            string jsonStr = textAsset.text;
            List<UnitConvert> props = FullSerializerAPI.Deserialize(typeof(List<UnitConvert>), jsonStr) as List<UnitConvert>;
            _unitConvertDic = props.ToDictionary(key => key.x, value => value);
        }

        public string GetUnit(int key)
        {
            if (_unitConvertDic.ContainsKey(key))
            {
                if (String.IsNullOrEmpty(_unitConvertDic[key].y))
                    return "";
                else
                    return _unitConvertDic[key].y;
            }
            return "";
        }

        public string GetValue(Long2 lhs, bool integer = false)
        {
            if (lhs.x == 0)
                return "0";
            int unit = (int)((lhs.y + Math.Log10(Mathf.Abs(lhs.x))) / 3);
            string uni = GetUnit(unit);
            int b = ((int)((lhs.y + Math.Log10(Mathf.Abs(lhs.x))) / 3)) * 3;
            double a = lhs.x * Math.Pow(10, lhs.y - b);
            if (!integer)
                return a + uni;
            string aa = Mathf.RoundToInt((float)a).ToString();
            return aa + uni;
        }

        public string GetValue(Long2 lhs, int length)
        {
            if (lhs.x == 0)
                return "0";
            int unit = (int)((lhs.y + Math.Log10(Mathf.Abs(lhs.x))) / 3);
            string uni = GetUnit(unit);
            int b = ((int)((lhs.y + Math.Log10(Mathf.Abs(lhs.x))) / 3)) * 3;
            double a = lhs.x * Math.Pow(10, lhs.y - b);
            string aa = a.ToString();
            length = Math.Min(length, aa.Length);
            //���ȡֵ�����һλ��С���㣬������ȡһλ
            int index = aa.IndexOf('.');
            if (index == length - 1)
                length++;
            aa = aa.Substring(0, length);
            return aa + uni;
        }

        public string GetIntValue(Long2 lhs, int length)
        {
            if (lhs.x == 0)
                return "0";
            double a = lhs.x * Math.Pow(10, lhs.y * 3);
            string aa = a.ToString();
            length = Math.Min(length, aa.Length);
            //���ȡֵ�����һλ��С���㣬������ȡһλ
            int index = aa.IndexOf('.');
            if (index != -1)
            {
                index += length;
                aa = aa.Substring(0, index);
            }
            return aa;
        }

        private string _uni;
        private string _aa;
        /// <summary>
        /// ȡС�����countλ��ֵ
        /// </summary>
        /// <returns></returns>
        public string GetFloatValue(Long2 lhs, int count)
        {
            if (lhs.x == 0)
                return "0";
            int unit = (int)((lhs.y + Math.Log10(Mathf.Abs(lhs.x))) / 3);
            _uni = GetUnit(unit);
            int b = ((int)((lhs.y + Math.Log10(Mathf.Abs(lhs.x))) / 3)) * 3;
            double a = lhs.x * Math.Pow(10, lhs.y - b);
            a *= Math.Pow(10, count);
            a = Mathf.RoundToInt((float)a);
            a /= Math.Pow(10, count);
            //���ȡֵ�����һλ��С���㣬������ȡһλ
            _aa = a.ToString();
            string[] valueStr = _aa.Split('.');
            if (valueStr.Length <= 1)
                return _aa + _uni;
            int length = valueStr[0].Length + Math.Min(count, valueStr[1].Length) + 1;
            _aa = _aa.Substring(0, length);
            return _aa + _uni;
        }

    }
}

