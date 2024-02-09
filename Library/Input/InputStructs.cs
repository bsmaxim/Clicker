using System.Runtime.InteropServices;

namespace AutoClicker.Library.Input
{

    public static class InputStructs
    {
        public const uint KEY_UP = 0x0002;

        public struct INPUT
        {
            public uint type;
            public InputUnion data;
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
