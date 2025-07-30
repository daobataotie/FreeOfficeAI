using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Setup.Library
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        /// <summary>
        /// 程序名称，用作注册表的键名
        /// </summary>
        private string programName = "FreeOfficeAI";

        public Installer()
        {
            InitializeComponent();

            AfterInstall += Installer_AfterInstall;

            AfterUninstall += Installer_AfterUninstall;
        }

        private void Installer_AfterInstall(object sender, InstallEventArgs e)
        {
            //安装目录
            string installerPath = Context.Parameters["targetdir"].Remove(Context.Parameters["targetdir"].Length - 2);

            EditRegistry(RegistryHive.CurrentUser, installerPath);

            //bool allUsers = !string.IsNullOrWhiteSpace(Context.Parameters["ALLUSERS"]);
            //if (allUsers)
            //    EditRegistry(RegistryHive.LocalMachine, installerPath);
        }

        private void Installer_AfterUninstall(object sender, InstallEventArgs e)
        {
            //安装目录
            string installerPath = Context.Parameters["targetdir"].Remove(Context.Parameters["targetdir"].Length - 2);

            EditRegistry(RegistryHive.CurrentUser, installerPath, false);

            //bool allUsers = !string.IsNullOrWhiteSpace(Context.Parameters["ALLUSERS"]);
            //if (allUsers)
            //    EditRegistry(RegistryHive.LocalMachine, installerPath, false);
        }

        private void EditRegistry(RegistryHive root, string installPath, bool isInstall = true)
        {
            RegistryKey rootKey = null;
            try
            {
                if (Environment.Is64BitOperatingSystem)
                    rootKey = RegistryKey.OpenBaseKey(root, RegistryView.Registry64);
                else
                    rootKey = RegistryKey.OpenBaseKey(root, RegistryView.Registry32);


                //注册Word加载项-office
                string officeWordPath = $@"SOFTWARE\Microsoft\Office\Word\Addins\{programName}";
                var officeWordKey = rootKey.CreateSubKey(officeWordPath);
                if (officeWordKey != null)
                {
                    MessageBox.Show(officeWordKey.Name);
                    if (isInstall)
                    {
                        officeWordKey.SetValue("Description", programName, RegistryValueKind.String);
                        officeWordKey.SetValue("FriendlyName", $"{programName}助手", RegistryValueKind.String);    //显示在COM加载项中的名称
                        officeWordKey.SetValue("LoadBehavior", 3, RegistryValueKind.DWord); //启动时加载

                        //以Path.Combine方式拼接路径会将“|”误认为路径分隔符，导致路径格式错误
                        string assemblyPath = $"file:///{installPath.Replace("\\", "/")}/FreeOfficeAI.Word.vsto|vstolocal";
                        officeWordKey.SetValue("Manifest", assemblyPath, RegistryValueKind.String);
                    }
                    else
                    {
                        string officeWordPathRoot = @"SOFTWARE\Microsoft\Office\Word\Addins";
                        var officeWordRoot = rootKey.CreateSubKey(officeWordPathRoot);
                        officeWordRoot.DeleteSubKeyTree(programName, false);
                    }
                }


                //注册Word加载项-WPS
                var wpsWordKey = rootKey.CreateSubKey(@"SOFTWARE\Kingsoft\Office\WPS\AddinsWL");
                if (wpsWordKey != null)
                {
                    if (isInstall)
                        wpsWordKey.SetValue(programName, "", RegistryValueKind.String);
                    else
                        wpsWordKey.DeleteValue(programName, false);
                }


                //注册Excel加载项-office
                string officeExcelPath = $@"SOFTWARE\Microsoft\Office\Excel\Addins\{programName}";
                var officeExcelKey = rootKey.CreateSubKey(officeExcelPath);
                if (officeExcelKey != null)
                {
                    if (isInstall)
                    {
                        officeExcelKey.SetValue("Description", programName, RegistryValueKind.String);
                        officeExcelKey.SetValue("FriendlyName", $"{programName}助手", RegistryValueKind.String);    //显示在COM加载项中的名称
                        officeExcelKey.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);  //启动时加载

                        string assemblyPath = $"file:///{installPath.Replace("\\", "/")}/FreeOfficeAI.Excel.vsto|vstolocal";
                        officeExcelKey.SetValue("Manifest", assemblyPath, RegistryValueKind.String);
                    }
                    else
                    {
                        string officeExcelPathRoot = @"SOFTWARE\Microsoft\Office\Excel\Addins";
                        var officeExcelRoot = rootKey.CreateSubKey(officeExcelPathRoot);
                        officeExcelRoot.DeleteSubKeyTree(programName, false);
                    }
                }

                //注册Excel加载项-WPS
                var wpsExcelKey = rootKey.CreateSubKey(@"SOFTWARE\Kingsoft\Office\ET\AddinsWL");
                if (wpsExcelKey != null)
                {
                    if (isInstall)
                        wpsExcelKey.SetValue(programName, "", RegistryValueKind.String);
                    else
                        wpsExcelKey.DeleteValue(programName, false);
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            finally
            {
                rootKey.Dispose();
            }
        }
    }
}
