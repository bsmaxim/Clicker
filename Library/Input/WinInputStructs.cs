using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoClicker.Library.Input
{

    public static class WinInputStructs
    {
        public const uint KEY_UP = 0x0002;

        public struct INPUT
        {
            public uint type;
            public InputUnion data;
        }
        private const uint MOUSE_MOVE = 0x0001;
        private const uint MOUSE_LEFTDOWN = 0x0002;
        private const uint MOUSE_LEFTUP = 0x0004;
        private const uint MOUSE_RIGHTDOWN = 0x0008;
        private const uint MOUSE_RIGHTUP = 0x0010;
        private const uint MOUSE_MIDDLEDOWN = 0x0020;
        private const uint MOUSE_MIDDLEUP = 0x0040;
        private const uint MOUSE_XDOWN = 0x0080;
        private const uint MOUSE_XUP = 0x0100;
        private const uint MOUSE_WHEEL = 0x0800;
        public static uint MouseButtonToFlag(MouseButtons button, bool isDown)
        {
            if (isDown)
            {
                return button switch
                {
                    MouseButtons.Left => MOUSE_LEFTDOWN,
                    MouseButtons.Right => MOUSE_RIGHTDOWN,
                    MouseButtons.Middle => MOUSE_MIDDLEDOWN,
                    MouseButtons.XButton1 => MOUSE_XDOWN,
                    MouseButtons.XButton2 => MOUSE_XDOWN,
                    _ => 0x0000
                };
            }
            else
            {
                return button switch
                {
                    MouseButtons.Left => MOUSE_LEFTUP,
                    MouseButtons.Right => MOUSE_RIGHTUP,
                    MouseButtons.Middle => MOUSE_MIDDLEUP,
                    MouseButtons.XButton1 => MOUSE_XUP,
                    MouseButtons.XButton2 => MOUSE_XUP,
                    _ => 0x0000
                };
            }
        }

        public struct MOUSEINPUT
        {
            public int x;
            public int y;
            public uint mouseData;
            public uint flags;
            public uint time;
            public nint extraInfo;
        }

        public struct KEYBDINPUT
        {
            public ushort keyCode;
            public ushort scan;
            public uint flags;
            public uint time;
            public nint extraInfo;
        }

        public struct HARDWAREINPUT
        {
            public uint msg;
            public ushort paramL;
            public ushort paramH;
        }

        [StructLayout(LayoutKind.Explicit)] // Явное размещение полей
        public struct InputUnion
        {
            [FieldOffset(0)] // Все поля размещаются в одном и том же месте. В начале структуры
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
    }
}
