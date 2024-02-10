using System.Windows.Forms;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library.Input
{
    public static class InputSequencer
    {
        public static List<InputSequence> BuildSequence(Dictionary<long, List<KeyEvent>> KeyPlaybackBuffer)
        {
            var sequences = new List<InputSequence>();

            foreach (var kvp in KeyPlaybackBuffer)
            {
                var timestamp = kvp.Key;
                var keyEvents = kvp.Value;
                var inputs = new List<INPUT>();

                foreach (var keyEvent in keyEvents)
                {
                    inputs.Add(ConvertKeyToInput(keyEvent.KeyCode, keyEvent.IsKeyUp));
                }

                sequences.Add(new InputSequence
                {
                    Timestamp = timestamp,
                    Value = [.. inputs]
                });
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
                    ki = new KEYBDINPUT
                    {
                        keyCode = (ushort)key,
                        scan = 0,
                        flags = IsKeyUp ? KEY_UP : 0,
                        time = 0,
                        extraInfo = nint.Zero
                    }
                }
            };
        }

        public static List<InputSequence> BuildSequence(Dictionary<long, List<MouseEvent>> KeyPlaybackBuffer)
        {
            var sequences = new List<InputSequence>();

            foreach (var kvp in KeyPlaybackBuffer)
            {
                var timestamp = kvp.Key;
                var mouseEvents = kvp.Value;
                var inputs = new List<INPUT>();

                foreach (var mouseEvent in mouseEvents)
                {
                    inputs.Add(ConvertMouseToInput(mouseEvent));
                }

                sequences.Add(new InputSequence
                {
                    Timestamp = timestamp,
                    Value = [.. inputs]
                });
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
                    mi = new MOUSEINPUT
                    {
                        x = mouseEvent.X,
                        y = mouseEvent.Y,
                        mouseData = 0,
                        flags = mouseEvent.Flags,
                        time = 0,
                        extraInfo = nint.Zero
                    }
                }
            };
        }
    }
}
