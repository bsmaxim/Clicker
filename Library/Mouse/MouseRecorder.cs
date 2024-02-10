using System.Windows.Forms;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace AutoClicker.Library.Mouse
{
    public class MouseRecorder : Recorder
    {
        public MouseRecorder() : base() { }

        protected override void ConnectHooks()
        {
            m_GlobalHook.MouseDown += HookMouseDown;
            m_GlobalHook.MouseUp += HookMouseUp;
            m_GlobalHook.MouseMove += HookMouseEvent;
            m_GlobalHook.MouseWheel += HookMouseWheel;
        }
        protected override void DisconnectHooks()
        {
            m_GlobalHook.MouseDown -= HookMouseDown;
            m_GlobalHook.MouseUp -= HookMouseUp;
            m_GlobalHook.MouseMove -= HookMouseEvent;
            m_GlobalHook.MouseWheel -= HookMouseWheel;
        }

        private enum MouseEventType
        {
            MouseDown,
            MouseUp,
            MouseMove,
            MouseWheel
        }

        private void HookEvent(MouseEventArgs e, MouseEventType eventType)
        {
            var currentTimestamp = SW.ElapsedMicroseconds();
            // Console.WriteLine($"{e.Delta} {e.Button} {e.Clicks} {e.Location} {currentTimestamp} {eventType}");

            var me = new MouseEvent()
            {
                X = e.X,
                Y = e.Y,
                MouseData = (uint)e.Delta,
                Flags = GetFlags(e.Button, eventType),
                Timestamp = currentTimestamp,
                ExtraInfo = e.Clicks
            };

            if (KeyPlaybackBuffer.TryGetValue(currentTimestamp, out List<IInputEvent>? value))
            {
                value.Add(me);
            }
            else
            {
                KeyPlaybackBuffer[currentTimestamp] = [me];
            }

        }

        private void HookMouseWheel(object? sender, MouseEventArgs e)
        {
            HookEvent(e, MouseEventType.MouseWheel);
        }
        private void HookMouseEvent(object? sender, MouseEventArgs e)
        {
            HookEvent(e, MouseEventType.MouseMove);
        }
        private void HookMouseUp(object? sender, MouseEventArgs e)
        {
            HookEvent(e, MouseEventType.MouseUp);
        }
        private void HookMouseDown(object? sender, MouseEventArgs e)
        {
            HookEvent(e, MouseEventType.MouseDown);
        }

        private const uint MOUSE_MOVE = 0x0001;
        private const uint MOUSE_LEFTDOWN = 0x0002;
        private const uint MOUSE_LEFTUP = 0x0004;
        private const uint MOUSE_RIGHTDOWN = 0x0008;
        private const uint MOUSE_RIGHTUP = 0x0010;
        private const uint MOUSE_MIDDLEDOWN = 0x0020;
        private const uint MOUSE_MIDDLEUP = 0x0040;
        private const uint MOUSE_XDOWN = 0x0080;
        private const uint MOUSE_XUP = 0x0100;
        private const uint MOUSE_WHEEL = 0x0800;
        private const uint MOUSE_ABSOLUTE = 0x8000;

        private static uint GetFlags(MouseButtons button, MouseEventType eventType)
        {
            if (eventType == MouseEventType.MouseWheel)
            {
                return MOUSE_WHEEL;
            }

            if (eventType == MouseEventType.MouseDown)
            {
                return button switch
                {
                    MouseButtons.None => MOUSE_MOVE | MOUSE_ABSOLUTE,
                    MouseButtons.Left => MOUSE_LEFTDOWN,
                    MouseButtons.Right => MOUSE_RIGHTDOWN,
                    MouseButtons.Middle => MOUSE_MIDDLEDOWN,
                    MouseButtons.XButton1 => MOUSE_XDOWN,
                    MouseButtons.XButton2 => MOUSE_XDOWN,
                    _ => 0x0000
                };
            }

            return button switch
            {
                MouseButtons.None => MOUSE_MOVE | MOUSE_ABSOLUTE,
                MouseButtons.Left => MOUSE_LEFTUP,
                MouseButtons.Right => MOUSE_RIGHTUP,
                MouseButtons.Middle => MOUSE_MIDDLEUP,
                MouseButtons.XButton1 => MOUSE_XUP,
                MouseButtons.XButton2 => MOUSE_XUP,
                _ => 0x0000
            };
        }
    }

}
