using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public class BatHelper
    {
        /// <summary>
        /// 执行bat文件
        /// </summary>
        /// <param name="batName"></param>
        public static void LaunchBat(string batName)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.FileName = batName;
            psi.Verb = "runas";
            Process process = Process.Start(psi);
            process.WaitForExit();
        }
    }
}
