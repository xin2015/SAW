using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace SAW.Core.Helpers
{
    /// <summary>
    /// IP工具类
    /// </summary>
    public class IPHelper
    {
        /// <summary>
        /// 通过访问IP地址查询网站获取外网IP
        /// </summary>
        /// <returns>外网IP</returns>
        public static string GetExtranetIPAddress()
        {
            using (WebClient wc = new WebClient())
            {
                string html = wc.DownloadString("http://2017.ip138.com/ic.asp");
                Regex regex = new Regex("(\\d+.\\d+.\\d+.\\d+)");
                return regex.Match(html).Value;
            }
        }

        /// <summary>
        /// 获取本机IPv4地址
        /// </summary>
        /// <returns>本机IPv4地址</returns>
        public static List<string> GetHostInterNetworkAddresses()
        {
            return Dns.GetHostAddresses(Dns.GetHostName()).Where(o => o.AddressFamily == AddressFamily.InterNetwork).Select(o => o.ToString()).ToList();
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP地址</returns>
        public static List<string> GetHostAddresses()
        {
            return Dns.GetHostAddresses(Dns.GetHostName()).Select(o => o.ToString()).ToList();
        }
    }
}
