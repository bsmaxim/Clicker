using System.Windows.Forms;
using static AutoClicker.Library.Input.InputStructs;

namespace AutoClicker.Library.Input
{
    public class KeyInputSequencer
    {
        public static List<KeyInputSequence> BuildSequence(Dictionary<long, List<KeyEvent>> KeyPlaybackBuffer)
        {
            var sequences = new List<KeyInputSequence>();

            // проход по словарю и добавление в sequences
            foreach (var kvp in KeyPlaybackBuffer)
            {
                var sequence = new KeyInputSequence();
                var inputs = new List<INPUT>();
                sequence.FrameTimestamp = kvp.Key;

                foreach (var key in kvp.Value)
                {
                    inputs.Add(ConvertKeyToInput(key.KeyCode, key.IsKeyUp));
                }

                sequence.InputSequence = inputs.ToArray();
                sequences.Add(sequence);
            }

            return sequences;
        }

        private const int KEYBOARDEVENT = 1;
        private const int SCANVALUE = 0;

        private static INPUT ConvertKeyToInput(Keys key, bool IsKeyUp)
        {
            return new INPUT
            {
                type = KEYBOARDEVENT,
                data =
                {
                    ki = new KEYBDINPUT
                    {
                        keyCode = (ushort)key,
                        scan = SCANVALUE,
                        flags = IsKeyUp ? KEY_UP : 0,
                        time = 0,
                        extraInfo = nint.Zero
                    }
                }
            };
        }
    }
}
