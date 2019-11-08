using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    /// <summary>
    /// FTP工具类
    /// </summary>
    public class FtpHelper
    {
        /// <summary>
        /// 基URI
        /// </summary>
        private Uri _baseUri;
        /// <summary>
        /// 凭据
        /// </summary>
        private NetworkCredential _networkCredential;

        /// <summary>
        /// FTP工具类构造函数
        /// </summary>
        /// <param name="baseUriString">基URI</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public FtpHelper(string baseUriString, string userName, string password)
        {
            _baseUri = new Uri(baseUriString);
            _networkCredential = new NetworkCredential(userName, password);
        }

        private FtpWebRequest GetRequest(string relativeUriString, string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(_baseUri, relativeUriString));
            request.Credentials = _networkCredential;
            request.Method = method;
            return request;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="relativeUriString">相对URI</param>
        /// <param name="path">文件保存路径</param>
        public void DownloadFile(string relativeUriString, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Append);
            FtpWebRequest request = GetRequest(relativeUriString, WebRequestMethods.Ftp.DownloadFile);
            request.ContentOffset = fs.Length;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            stream.CopyTo(fs);
            stream.Close();
            response.Close();
            fs.Flush();
            fs.Close();
        }


        public static void Demo()
        {
            FtpHelper helper = new FtpHelper("ftp://202.104.69.206", "admin", "suncereltd@2017");
            try
            {
                helper.DownloadFile("Data/Suncere/EnvCrownStationDB_zhuhai.rar", "D:\\Data\\预报预警小组\\DataBaseBackup\\EnvCrownStationDB_zhuhai.rar");
            }
            catch (Exception ex)
            {
                if (ex.Message != "远程服务器返回错误: (550) 文件不可用(例如，未找到文件，无法访问文件)。")
                {
                    throw ex;
                }
            }
        }
    }
}
