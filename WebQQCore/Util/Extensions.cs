﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace iQQ.Net.WebQQCore.Util
{

    /// <summary>DataList的Add方法的选项</summary>
    public enum AddChoice
    {
        /// <summary>直接添加，有重复时忽略</summary>
        IgnoreDuplication,
        /// <summary>若存在，则更新数值</summary>
        Update,
        /// <summary>直接添加，有重复时异常</summary>
        NotIgnoreDuplication,

    }

    public static class Extensions
    {
        public static void Add<TK, TV>(this Dictionary<TK, List<TV>> dic, TK key, TV value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(value);
            }
            else
            {
                dic.Add(key, new List<TV>() { value });
            }
        }


        /// <summary>更新：如果不存在，则添加；存在，则替换。
        /// <para>不更新：如果不存在，则添加；存在，则忽略。</para></summary>
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value, AddChoice addChoice)
        {
            if (dict.ContainsKey(key))
            {
                if (addChoice == AddChoice.Update)
                {
                    dict[key] = value;
                    return;
                }
                else if (addChoice == AddChoice.IgnoreDuplication)
                {
                    return;
                }
                else if (addChoice == AddChoice.NotIgnoreDuplication)
                {
                    throw new ArgumentException("已存在具有相同键的元素");
                }
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public static IEnumerable<Cookie> GetAllCookies(this CookieContainer cc)
        {
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
            System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (var pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                {
                    foreach (var c in colCookies.OfType<Cookie>())
                    {
                        yield return c;
                    }
                }
            }
        }

        public static IEnumerable<Cookie> GetCookies(this CookieContainer cc, string name)
        {
            return GetAllCookies(cc).Where(item => string.Compare(item.Name,name, StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        /// 将字符串MD5加密为字节数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Md5ToArray(this string input)
        {
            return MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// 将字符串MD5加密为字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5ToString(this string input)
        {
            var buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            var builder = new StringBuilder();
            for (var i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("X2"));
            }
            return builder.ToString();
        }
    }
}
