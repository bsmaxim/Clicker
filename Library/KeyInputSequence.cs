using static AutoClicker.Library.InputStructs;

namespace AutoClicker.Library
{
    public class KeyInputSequence
    {
        public long FrameTimestamp { get; set; }
        public INPUT[] InputSequence { get; set; }
    }
}
