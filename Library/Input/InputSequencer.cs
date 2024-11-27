using System.Windows.Forms;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library.Input
{
    public static class InputSequencer
    {
        public static List<InputUnit> BuildSequence(Dictionary<long, List<KeyEvent>> KeyPlaybackBuffer)
        {
            var sequences = new List<InputUnit>();

            foreach (var kvp in KeyPlaybackBuffer)
            {
                var timestamp = kvp.Key;
                var keyEvents = kvp.Value;
                var inputs = new List<INPUT>();

                foreach (var keyEvent in keyEvents)
                {
                    inputs.Add(ConvertKeyToInput(keyEvent.KeyCode, keyEvent.IsKeyUp));
                }

                sequences.Add(new InputUnit(timestamp, [.. inputs]));
            }

            return sequences;
        }

        private static INPUT ConvertKeyToInput(Keys key, bool IsKeyUp)
        {
            return new INPUT
            {
                type = INPUT_KEYBOARD,
                data =
                {
                    ki = new KEYBDINPUT(
                        (ushort)key,
                        IsKeyUp ? KEY_UP : 0
                    )
                }
            };
        }

        public static List<InputUnit> BuildSequence(Dictionary<long, List<MouseEvent>> KeyPlaybackBuffer)
        {
            var sequences = new List<InputUnit>();

            foreach (var kvp in KeyPlaybackBuffer)
            {
                var timestamp = kvp.Key;
                var mouseEvents = kvp.Value;
                var inputs = new List<INPUT>();

                foreach (var mouseEvent in mouseEvents)
                {
                    inputs.Add(ConvertMouseToInput(mouseEvent));
                }

                sequences.Add(new InputUnit(timestamp, [.. inputs]));
            }

            return sequences;
        }
        public static INPUT ConvertMouseToInput(MouseEvent mouseEvent)
        {
            return new INPUT
            {
                type = INPUT_MOUSE,
                data =
                {
                    mi = new MOUSEINPUT(
                        mouseEvent.X,
                        mouseEvent.Y,
                        mouseEvent.MouseData,
                        mouseEvent.Flags)
                }
            };
        }
    }
}
