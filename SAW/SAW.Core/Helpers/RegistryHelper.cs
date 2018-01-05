using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    /// <summary>
    /// 注册表操作工具类
    /// </summary>
    public static class RegistryHelper
    {
        public static void Demo()
        {
            using (RegistryKey software = Registry.LocalMachine.OpenSubKey("Software", true))
            {
                RegistryKey company = software.OpenSubKey("Suncere", true);
                if (company == null)
                {
                    company = software.CreateSubKey("Suncere", true);
                }
                RegistryKey project = company.CreateSubKey("ContentTransfer", true);
                project.SetValue("Name", "产品交换系统");
                project.SetValue("Version", "1.0.0.0");
                project.Dispose();
                company.Dispose();
            }
        }
    }
}
