using System.Windows.Forms;

namespace AutoClicker.Library.Keyboard
{
    public class KeyRecorder : Recorder
    {
        public KeyRecorder() : base() { }

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
            var keyEvent = CreateKeyEvent(e.KeyCode, timestamp, false);
            AddInputEventToBuffer(keyEvent, timestamp);
        }

        private void HookKeyUp(object? sender, KeyEventArgs e)
        {
            var timestamp = SW.ElapsedMicroseconds();
            var keyEvent = CreateKeyEvent(e.KeyCode, timestamp, true);
            AddInputEventToBuffer(keyEvent, timestamp);
        }

        private static KeyEvent CreateKeyEvent(Keys keyCode, long timestamp, bool isKeyUp)
        {
            return new KeyEvent
            {
                KeyCode = keyCode,
                Timestamp = timestamp,
                IsKeyUp = isKeyUp
            };
        }
    }
}
