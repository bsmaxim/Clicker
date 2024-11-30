using System.Runtime.InteropServices;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library
{
    public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);

    public static class WinApi
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        public static int ShowNotification(
            string message,
            string title,
            NotificationType type = NotificationType.Ok
        )
        {
            return MessageBox(IntPtr.Zero, message, title, (uint)type);
        }

        // Типы уведомлений
        public enum NotificationType : uint
        {
            Ok = 0x00000000,
            OkCancel = 0x00000001,
            YesNo = 0x00000004,
            Information = 0x00000040,
            Warning = 0x00000030,
            Error = 0x00000010,
        }

        #region Ввод

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(
            uint numberOfInputs,
            UserInput[] inputs,
            int sizeOfInputStructure
        );

        [DllImport("kernel32.dll")]
        internal static extern uint GetLastError();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetSystemMetrics(SystemMetric smIndex);

        #endregion

        #region Хоткеи

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        #endregion

        #region Уведомления
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        #endregion
    }

    public enum SystemMetric
    {
        SM_CXSCREEN = 0,
        SM_CYSCREEN = 1,
    };
}
