using System.Windows.Forms;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library.Input
{
    public static class InputSequencer
    {
        public static List<InputUnit> BuildSequence(
            Dictionary<long, List<KeyEvent>> KeyPlaybackBuffer
        )
        {
            var sequences = new List<InputUnit>();

            foreach (var kvp in KeyPlaybackBuffer)
            {
                var timestamp = kvp.Key;
                var keyEvents = kvp.Value;
                var inputs = new List<UserInput>();

                foreach (var keyEvent in keyEvents)
                {
                    inputs.Add(CreateKeyboardInput(keyEvent.KeyCode, keyEvent.IsKeyUp));
                }

                sequences.Add(new InputUnit(timestamp, [.. inputs]));
            }

            return sequences;
        }

        public static List<InputUnit> BuildSequence(
            Dictionary<long, List<MouseEvent>> KeyPlaybackBuffer
        )
        {
            var sequences = new List<InputUnit>();

            foreach (var kvp in KeyPlaybackBuffer)
            {
                var timestamp = kvp.Key;
                var mouseEvents = kvp.Value;
                var inputs = new List<UserInput>();

                foreach (var mouseEvent in mouseEvents)
                {
                    inputs.Add(CreateMouseInput(mouseEvent));
                }

                sequences.Add(new InputUnit(timestamp, [.. inputs]));
            }

            return sequences;
        }

        public static UserInput CreateMouseInput(MouseEvent mouseEvent)
        {
            return new UserInput
            {
                Type = INPUT_MOUSE,
                Data =
                {
                    MouseInput = new MouseInputData(
                        mouseEvent.X,
                        mouseEvent.Y,
                        mouseEvent.MouseData,
                        mouseEvent.Flags
                    ),
                },
            };
        }

        public static UserInput CreateKeyboardInput(Keys key, bool IsKeyUp)
        {
            return new UserInput
            {
                Type = INPUT_KEYBOARD,
                Data = { KeyboardInput = new KeyboardInputData((ushort)key, IsKeyUp ? KEY_UP : 0) },
            };
        }

        public static UserInput CreateHardwareInput(uint msg, ushort paramL, ushort paramH)
        {
            return new UserInput
            {
                Type = INPUT_HARDWARE,
                Data = new InputDataUnion
                {
                    HardwareInput = new HardwareInputData(msg, paramL, paramH),
                },
            };
        }
    }
}
