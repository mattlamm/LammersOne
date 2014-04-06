using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LammersOne.Utils
{
    public class WindowsSystem
    {
        //-------------------------------------------------------------------------------------------------------------------------

        #region Enums

        [Flags]
        public enum ExitWindows : uint
        {
            // ONE of the following five:
            LogOff = 0x00,
            ShutDown = 0x01,
            Reboot = 0x02,
            PowerOff = 0x08,
            RestartApps = 0x40,
            // plus AT MOST ONE of the following two:
            Force = 0x04,
            ForceIfHung = 0x10,
        }

        [Flags]
        enum ShutdownReason : uint
        {
            MajorApplication = 0x00040000,
            MajorHardware = 0x00010000,
            MajorLegacyApi = 0x00070000,
            MajorOperatingSystem = 0x00020000,
            MajorOther = 0x00000000,
            MajorPower = 0x00060000,
            MajorSoftware = 0x00030000,
            MajorSystem = 0x00050000,

            MinorBlueScreen = 0x0000000F,
            MinorCordUnplugged = 0x0000000b,
            MinorDisk = 0x00000007,
            MinorEnvironment = 0x0000000c,
            MinorHardwareDriver = 0x0000000d,
            MinorHotfix = 0x00000011,
            MinorHung = 0x00000005,
            MinorInstallation = 0x00000002,
            MinorMaintenance = 0x00000001,
            MinorMMC = 0x00000019,
            MinorNetworkConnectivity = 0x00000014,
            MinorNetworkCard = 0x00000009,
            MinorOther = 0x00000000,
            MinorOtherDriver = 0x0000000e,
            MinorPowerSupply = 0x0000000a,
            MinorProcessor = 0x00000008,
            MinorReconfig = 0x00000004,
            MinorSecurity = 0x00000013,
            MinorSecurityFix = 0x00000012,
            MinorSecurityFixUninstall = 0x00000018,
            MinorServicePack = 0x00000010,
            MinorServicePackUninstall = 0x00000016,
            MinorTermSrv = 0x00000020,
            MinorUnstable = 0x00000006,
            MinorUpgrade = 0x00000003,
            MinorWMI = 0x00000015,

            FlagUserDefined = 0x40000000,
            FlagPlanned = 0x80000000
        }


        #endregion

        //-------------------------------------------------------------------------------------------------------------------------

        #region Structs

        /// <summary>
        /// SYSTEMTIME structure with some useful methods
        /// </summary>
        public struct SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Milliseconds;

            public void FromDateTime(DateTime time)
            {
                Year = (ushort)time.Year;
                Month = (ushort)time.Month;
                DayOfWeek = (ushort)time.DayOfWeek;
                Day = (ushort)time.Day;
                Hour = (ushort)time.Hour;
                Minute = (ushort)time.Minute;
                Second = (ushort)time.Second;
                Milliseconds = (ushort)time.Millisecond;
            }

            public DateTime ToDateTime()
            {
                return new DateTime(Year, Month, Day, Hour, Minute, Second, Milliseconds);
            }

            public static DateTime ToDateTime(SystemTime time)
            {
                return time.ToDateTime();
            }
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------

        #region DLLImports

        [DllImport("user32.dll")]
        static extern bool ExitWindowsEx(ExitWindows uFlags, ShutdownReason dwReason);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveOutGetNumDevs();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool LockWorkStation();

        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime Time);

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        public extern static void Win32GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SystemTime sysTime);

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------

        #region Public

        /// <summary>
        /// Locks a workstation.
        /// </summary>
        public static void LockPC()
        {
            LockWorkStation();
        }

        /// <summary>
        /// Checks the count of the sound cards installed on a PC.
        /// </summary>
        /// <returns></returns>
        public static uint GetSoundCardCount()
        {
            return waveOutGetNumDevs();
        }

        /// <summary>
        /// Gets the system time expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSystemDateTime()
        {
            SystemTime currTime = new SystemTime();
            Win32GetSystemTime(ref currTime);

            return new DateTime(currTime.Year, currTime.Month, currTime.Day, currTime.Hour, currTime.Minute, currTime.Second);
        }

        /// <summary>
        /// Hibernates a PC.
        /// </summary>
        /// <returns></returns>
        public static bool HibernatePC()
        {
            return Application.SetSuspendState(PowerState.Hibernate, true, false);
        }

        /// <summary>
        /// If the function succeeds, the return value is nonzero. Because the function executes asynchronously, 
        /// a nonzero return value indicates that the shutdown has been initiated. It does not indicate whether 
        /// the shutdown will succeed. It is possible that the system, the user, or another application 
        /// will abort the shutdown. 
        /// </summary>
        /// <returns>0 for failure otherwise non zero.</returns>
        public static bool LogOffPC(bool forceProcessToTerminate)
        {
            if (forceProcessToTerminate)
            {
                return ExitWindowsEx(ExitWindows.Force, ShutdownReason.MajorOther);
            }
            else
            {
                return ExitWindowsEx(ExitWindows.LogOff, ShutdownReason.MajorOther);
            }
        }

        /// <summary>
        /// If the function succeeds, the return value is nonzero. Because the function executes asynchronously, 
        /// a nonzero return value indicates that the shutdown has been initiated. It does not indicate whether 
        /// the shutdown will succeed. It is possible that the system, the user, or another application 
        /// will abort the shutdown. 
        /// </summary>
        /// <returns>0 for failure otherwise non zero.</returns>
        public static void RebootPC()
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
        }

        /// <summary>
        /// Sets the local date and time.
        /// </summary>
        /// <param name="dateTime"></param>
        public static void SetLocalDateTime(DateTime dateTime)
        {
            SystemTime sysTime = new SystemTime();
            sysTime.FromDateTime(dateTime);
            SetLocalTime(ref sysTime);
        }

        /// <summary>
        /// Sets the system time expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="dateTime">The DateTime to set.</param>
        public static void SetSystemDateTime(DateTime dateTime)
        {
            SystemTime updatedTime = new SystemTime();
            updatedTime.Year = (ushort)dateTime.Year;
            updatedTime.Month = (ushort)dateTime.Month;
            updatedTime.Day = (ushort)dateTime.Day;

            // UTC time; it will be modified according to the regional settings of the target computer so the actual hour might differ
            updatedTime.Hour = (ushort)dateTime.TimeOfDay.Hours;
            updatedTime.Minute = (ushort)dateTime.TimeOfDay.Minutes;
            updatedTime.Second = (ushort)dateTime.TimeOfDay.Seconds;

            Win32SetSystemTime(ref updatedTime);
        }

        /// <summary>
        /// If the function succeeds, the return value is nonzero. Because the function executes asynchronously, 
        /// a nonzero return value indicates that the shutdown has been initiated. It does not indicate whether 
        /// the shutdown will succeed. It is possible that the system, the user, or another application 
        /// will abort the shutdown. 
        /// </summary>
        /// <returns>0 for failure otherwise non zero.</returns>
        public static bool ShutdownPC()
        {
            return ExitWindowsEx(ExitWindows.ShutDown, ShutdownReason.MajorOther);
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------
    }
}