using System;
using System.Runtime.InteropServices;

namespace AutoClicker.Library
{
    public static class InputStructs
    {
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
            public IntPtr extraInfo;
        }

        public struct KEYBDINPUT
        {
            public ushort keyCode;
            public ushort scan;
            public uint flags;
            public uint time;
            public IntPtr extraInfo;
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
