using System.Diagnostics;
using System.Windows.Forms;
using AutoClicker.Library.Input;
using Gma.System.MouseKeyHook;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace AutoClicker.Library.Mouse
{
    public class MouseRecorder : Recorder
    {
        public MouseRecorder() : base() { }

        protected override void ConnectHooks()
        {
            m_GlobalHook.MouseDown += GlobalHookMouseDown;
            m_GlobalHook.MouseUp += GlobalHookMouseUp;
            m_GlobalHook.MouseMove += GlobalHookMouseEvent;
            m_GlobalHook.MouseWheel += GlobalHookMouseWheel;
        }
        protected override void DisconnectHooks()
        {
                m_GlobalHook.MouseDown -= GlobalHookMouseDown;
                m_GlobalHook.MouseUp -= GlobalHookMouseUp;
                m_GlobalHook.MouseMove -= GlobalHookMouseEvent;
                m_GlobalHook.MouseWheel -= GlobalHookMouseWheel;
        }

        private enum MouseEventType
        {
            MouseDown,
            MouseUp,
            MouseMove,
            MouseWheel
        }

        private void HookEvent(object? sender, MouseEventArgs e, MouseEventType eventType)
        {
            var currentTimestamp = SW.ElapsedMicroseconds();
            Console.WriteLine($"{e.Delta} {e.Button} {e.Clicks} {e.Location} {currentTimestamp} {eventType}");
            var isDown = eventType == MouseEventType.MouseDown;
            var me = CreateMouseEvent(e, currentTimestamp, isDown);
            AddMouseEventToBuffer(me, currentTimestamp);
        
        }


        private static MouseEvent CreateMouseEvent(MouseEventArgs e, long timestamp, bool isDown)
        {
            return new()
            {
                X = e.X,
                Y = e.Y,
                MouseData = (uint)e.Delta,
                Flags = WinInputStructs.MouseButtonToFlag(e.Button, isDown),
                Timestamp = timestamp,
                ExtraInfo = e.Clicks
            };
        }
        private void AddMouseEventToBuffer(MouseEvent me, long timestamp)
        {
            if (KeyPlaybackBuffer.TryGetValue(timestamp, out List<IInputEvent>? value))
            {
                value.Add(me);
            }
            else
            {
                KeyPlaybackBuffer[timestamp] = [me];
            }
        }

        private void GlobalHookMouseWheel(object? sender, MouseEventArgs e)
        {
            HookEvent(sender, e, MouseEventType.MouseWheel);
        }
        private void GlobalHookMouseEvent(object? sender, MouseEventArgs e)
        {
            HookEvent(sender, e, MouseEventType.MouseMove);
        }
        private void GlobalHookMouseUp(object? sender, MouseEventArgs e)
        {
            HookEvent(sender, e, MouseEventType.MouseUp);
        }
        private void GlobalHookMouseDown(object? sender, MouseEventArgs e)
        {
            HookEvent(sender, e, MouseEventType.MouseDown);
        }
    }

}
