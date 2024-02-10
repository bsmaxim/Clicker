using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library.Input
{
    public class InputSequence
    {
        public long Timestamp { get; set; }
        public INPUT[]? Value { get; set; }
    }
}
