using System.Windows.Forms;

namespace AutoClicker.Library
{
    public class KeyEvent
    {
        public Keys KeyCode { get; set; }
        public long Timestamp { get; set; }
        public bool IsKeyUp { get; set; }
    }
}
