using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AutoClicker.Library.InputStructs;

namespace AutoClicker.Library
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
                    inputs.Add(ConvertKeyToInput(key.KeyCode));
                }

                sequence.InputSequence = inputs.ToArray();
                sequences.Add(sequence);
            }

            return sequences;
        }

        private const int KEYBOARDEVENT = 1;
        private const int SCANVALUE = 0;

        private static INPUT ConvertKeyToInput(Keys key)
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
                        flags = 0,
                        time = 0,
                        extraInfo = IntPtr.Zero
                    }
                }
            };
        }
    }
}
