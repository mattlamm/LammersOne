using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace LammersOne.Utils
{
    public class AssemblyHelper
    {
        //-------------------------------------------------------------------------------------------------------------------------

        #region Public

        public static string GetEntryAssemblyProductVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion;
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------
    }
}
