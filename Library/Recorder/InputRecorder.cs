using Gma.System.MouseKeyHook;
using System.Diagnostics;

namespace AutoClicker.Library.Recorder
{
    public class InputRecorder
    {
        protected Stopwatch SW;
        public readonly static IKeyboardMouseEvents m_GlobalHook = Hook.GlobalEvents();
        protected bool IsStarted = false;
        public Dictionary<long, List<IInputEvent>> KeyPlaybackBuffer { get; set; } = [];

        public InputRecorder()
        {
            SW = new();
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
            }
        }

        private void ReformatSequenceTimings()
        {
            if (KeyPlaybackBuffer.Count == 0)
            {
                Console.WriteLine("No items in buffer");
                return;
            }

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
