using Gma.System.MouseKeyHook;
using System.Diagnostics;

namespace AutoClicker.Library
{
    public class InputRecorder
    {
        protected Stopwatch SW;
        public readonly static IKeyboardMouseEvents m_GlobalHook = Hook.GlobalEvents();
        protected bool IsStarted = false;
        public Dictionary<long, List<IInputEvent>> KeyPlaybackBuffer { get; set; } = [];

        #region изменение оффсета на самый ранний тайминг между всеми инстансами
        protected static List<InputRecorder> Instances { get; set; } = [];
        protected static int RunningInstancesCount { get; set; } = 0;
        protected static object LockObject { get; set; } = new();
        protected static long EarliestTiming { get; set; } = long.MaxValue;
        #endregion

        public InputRecorder()
        {
            SW = new();
            Instances.Add(this);
        }

        ~InputRecorder()
        {
            if (IsStarted)
            {
                Stop();
            }
            Instances.Remove(this);
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

            lock (LockObject)
            {
                RunningInstancesCount++;
            }

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
                IsStarted = false;

                DisconnectHooks();

                lock (LockObject)
                {
                    RunningInstancesCount--;
                    if (RunningInstancesCount == 0)
                    {
                        Instances.ForEach(i =>
                        {
                            if (i.KeyPlaybackBuffer.Count > 0)
                            {
                                EarliestTiming = Math.Min(EarliestTiming, i.KeyPlaybackBuffer.Keys.Min());
                            }
                        });

                        Instances.ForEach(i => i.ReformatSequenceTimings());

                        EarliestTiming = long.MaxValue;
                    }
                }
            }
        }

        private void ReformatSequenceTimings()
        {
            if (KeyPlaybackBuffer.Count == 0)
            {
                Console.WriteLine("No items in buffer");
                return;
            }

            var resultDict = KeyPlaybackBuffer.ToDictionary(
                kvp => kvp.Key - EarliestTiming,
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
