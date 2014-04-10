using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace LammersOne.Windows
{
    public class AssemblyHelper
    {
        //-------------------------------------------------------------------------------------------------------------------------

        #region Public

        public static string GetEntryAssemblyProductVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion;
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------
    }
}
