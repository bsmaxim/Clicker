using System.Windows.Forms;

namespace AutoClicker.Library.Recorder
{
    public class KeyRecorder : InputRecorder
    {
        public KeyRecorder()
            : base() { }

        protected override void ConnectHooks()
        {
            m_GlobalHook.KeyDown += HookKeyDown;
            m_GlobalHook.KeyUp += HookKeyUp;
        }

        protected override void DisconnectHooks()
        {
            m_GlobalHook.KeyDown -= HookKeyDown;
            m_GlobalHook.KeyUp -= HookKeyUp;
        }

        private void HookKeyDown(object? sender, KeyEventArgs e)
        {
            var timestamp = SW.ElapsedMicroseconds();
            HookEvent(e, timestamp, false);
        }

        private void HookKeyUp(object? sender, KeyEventArgs e)
        {
            var timestamp = SW.ElapsedMicroseconds();
            HookEvent(e, timestamp, true);
        }

        private void HookEvent(KeyEventArgs e, long timestamp, bool isKeyUp)
        {
            Console.WriteLine(
                $"${e.KeyCode} {timestamp} {e.KeyCode} {e.KeyValue} {e.KeyData} {e.KeyData} {(isKeyUp ? "up" : "down")}"
            );
            var keyEvent = CreateKeyEvent(e.KeyCode, timestamp, isKeyUp);
            AddInputEventToBuffer(keyEvent, timestamp);
        }

        private static KeyEvent CreateKeyEvent(Keys keyCode, long timestamp, bool isKeyUp)
        {
            return new KeyEvent
            {
                KeyCode = keyCode,
                Timestamp = timestamp,
                IsKeyUp = isKeyUp,
            };
        }
    }
}
