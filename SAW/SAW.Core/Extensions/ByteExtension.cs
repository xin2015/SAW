using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Extensions
{
    /// <summary>
    /// Byte扩展类
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        /// 将字节数组转换为Base64格式的字符串
        /// </summary>
        /// <param name="array">字节数组</param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] array)
        {
            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// 将字节数组转换为UTF8格式的字符串
        /// </summary>
        /// <param name="array">字节数组</param>
        /// <returns></returns>
        public static string ToUTF8String(this byte[] array)
        {
            return Encoding.UTF8.GetString(array);
        }

        /// <summary>
        /// 将字节数组转换为16进制2位大写的字符串
        /// </summary>
        /// <param name="array">字节数组</param>
        /// <returns></returns>
        public static string ToX2String(this byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte item in array)
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
