using System.Windows.Forms;

namespace AutoClicker.Library
{
    public interface IInputEvent
    {
        public long Timestamp { get; set; }
    }

    public class KeyEvent : IInputEvent
    {
        public long Timestamp { get; set; }
        public Keys KeyCode { get; set; }
        public bool IsKeyUp { get; set; }
    }

    public class MouseEvent : IInputEvent
    {
        public long Timestamp { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public uint MouseData { get; set; }
        public uint Flags { get; set; }
        public nint ExtraInfo { get; set; }
    }
}
