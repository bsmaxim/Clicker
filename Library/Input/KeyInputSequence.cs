using static AutoClicker.Library.Input.InputStructs;

namespace AutoClicker.Library.Input
{
    public class KeyInputSequence
    {
        public long FrameTimestamp { get; set; }
        public INPUT[]? InputSequence { get; set; }
    }
}
