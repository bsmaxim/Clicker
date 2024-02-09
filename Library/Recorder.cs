using Gma.System.MouseKeyHook;
using System.Diagnostics;

namespace AutoClicker.Library
{
    public class Recorder
    {
        protected Stopwatch SW;
        protected IKeyboardMouseEvents m_GlobalHook;
        protected bool IsStarted = false;
        public Dictionary<long, List<IInputEvent>> KeyPlaybackBuffer { get; set; } = [];

        public Recorder()
        {
            SW = new();
            m_GlobalHook = Hook.GlobalEvents();
        }

        public void Start()
        {
            if (IsStarted)
            {
                Console.WriteLine("Already started");
                return;
            }

            KeyPlaybackBuffer = [];
            SW = new();

            ConnectHooks();

            SW.Start();
            IsStarted = true;
        }

        protected virtual void ConnectHooks() { }

        public void Clear()
        {
            if (IsStarted)
            {
                Stop();
            }

            KeyPlaybackBuffer = [];
            SW = new();
        }
        public void Stop()
        {
            if (IsStarted)
            {
                SW.Stop();
                ReformatSequenceTimings();
                IsStarted = false;

                DisconnectHooks();
                m_GlobalHook.Dispose();
            }
        }

        private void ReformatSequenceTimings()
        {
            var startTiming = KeyPlaybackBuffer.Keys.Min();
            var resultDict = KeyPlaybackBuffer.ToDictionary(
                kvp => kvp.Key - startTiming,
                kvp => kvp.Value
            );
            KeyPlaybackBuffer = resultDict;
        }

        protected virtual void DisconnectHooks() { }

        protected void AddInputEventToBuffer(IInputEvent inputEvent, long timestamp)
        {
            if (KeyPlaybackBuffer.TryGetValue(timestamp, out List<IInputEvent>? value))
            {
                value.Add(inputEvent);
            }
            else
            {
                KeyPlaybackBuffer[timestamp] = [inputEvent];
            }
        }
    }
}
