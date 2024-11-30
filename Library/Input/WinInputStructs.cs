using System.Runtime.InteropServices;

namespace AutoClicker.Library.Input
{
    public static class WinInputStructs
    {
        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 1;
        public const int INPUT_HARDWARE = 2; // Для аппаратного ввода.
        public const uint KEY_UP = 0x0002;

        public struct UserInput
        {
            public uint Type;
            public InputDataUnion Data;
        }

        public struct MouseInputData(int x, int y, uint mouseData, uint flags)
        {
            public int X = x;
            public int Y = y;
            public uint MouseData = mouseData;
            public uint Flags = flags;
            public uint Time = 0;
            public nint ExtraInfo = nint.Zero;
        }

        public struct KeyboardInputData(ushort keyCode, uint flags)
        {
            public ushort KeyCode = keyCode;
            public ushort Scan = 0;
            public uint Flags = flags;
            public uint Time = 0;
            public nint ExtraInfo = nint.Zero;
        }

        public struct HardwareInputData(uint msg, ushort paramL, ushort paramH)
        {
            public uint Msg = msg;
            public ushort ParamL = paramL;
            public ushort ParamH = paramH;
        }

        [StructLayout(LayoutKind.Explicit)] // Явное размещение полей
        public struct InputDataUnion
        {
            [FieldOffset(0)] // Все поля размещаются в одном и том же месте. В начале структуры
            public MouseInputData MouseInput;

            [FieldOffset(0)]
            public KeyboardInputData KeyboardInput;

            [FieldOffset(0)]
            public HardwareInputData HardwareInput;
        }
    }
}
