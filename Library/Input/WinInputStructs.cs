using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoClicker.Library.Input
{

    public static class WinInputStructs
    {
        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 1;
        public const uint KEY_UP = 0x0002;

        public struct INPUT
        {
            public uint type;
            public InputUnion data;
        }

        public struct MOUSEINPUT(int x, int y, uint mouseData, uint flags)
        {
            public int x = x;
            public int y = y;
            public uint mouseData = mouseData;
            public uint flags = flags;
            public uint time = 0;
            public nint extraInfo = nint.Zero;
        }

        public struct KEYBDINPUT(ushort keyCode, uint flags)
        {
            public ushort keyCode = keyCode;
            public ushort scan = 0;
            public uint flags = flags;
            public uint time = 0;
            public nint extraInfo = nint.Zero;
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
