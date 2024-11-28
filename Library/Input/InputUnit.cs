using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library.Input
{
    public class InputUnit(long timestamp, INPUT[]? value)
    {
        public long Timestamp { get; set; } = timestamp;
        public INPUT[]? Value { get; set; } = value;
    }
}
