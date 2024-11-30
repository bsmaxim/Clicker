using System.Windows.Input;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library.Input
{
    public class InputUnit(
        long timestamp,
        UserInput[]? value,
        ModifierKeys modifierKeys = ModifierKeys.None
    )
    {
        public long Timestamp { get; set; } = timestamp;
        public UserInput[]? Value { get; set; } = value;
        public ModifierKeys ModifierKeys { get; set; } = modifierKeys;
    }
}
