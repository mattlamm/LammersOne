using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace LammersOne.Utils
{
    public class Registry
    {
        //-------------------------------------------------------------------------------------------------------------------------

        #region Public

        public static bool IsRegisteredForCurrentUserStartup(string applicationName)
        {
            RegistryKey runKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            return runKey.GetValueNames().Contains(applicationName);
        }

        public static bool IsRegisteredForLocalMachineStartup(string applicationName)
        {
            RegistryKey runKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            return runKey.GetValueNames().Contains(applicationName);
        }

        public static void RegisterForCurrentUserStartup(string applicationName, string executablePath)
        {
            RegistryKey runKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            runKey.SetValue(applicationName, String.Format("\"{0}\"", executablePath));
        }

        public static void RegisterForLocalMachineStartup(string applicationName, string executablePath)
        {
            RegistryKey runKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            runKey.SetValue(applicationName, String.Format("\"{0}\"", executablePath));
        }

        public static void UnRegisterForCurrentUserStartup(string applicationName, string executablePath)
        {
            RegistryKey runKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            runKey.DeleteValue(applicationName);
        }

        public static void UnRegisterForLocalMachineStartup(string applicationName, string executablePath)
        {
            RegistryKey runKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            runKey.DeleteValue(applicationName);
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------
    }
}
