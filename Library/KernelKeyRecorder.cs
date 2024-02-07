using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Threading.Channels;
using System.Windows.Forms;

namespace AutoClicker.Library
{
    public class KernelKeyRecorder
    {
        // буфер записи, типа Dict<long, List<KeyEvent>
        public Dictionary<long, List<KeyEvent>> KeyPlaybackBuffer { get; set; } = [];
        private Stopwatch SW;
        private IKeyboardMouseEvents m_GlobalHook;
        private bool IsStarted = false;

        private Channel<string> KeyChannel { get; }
        public ChannelReader<string> KeyChannelReader { get; }
        private ChannelWriter<string> KeyChannelWriter { get; }

        public KernelKeyRecorder()
        {
            SW = new();
            m_GlobalHook = Hook.GlobalEvents();
            KeyChannel = Channel.CreateUnbounded<string>();
            KeyChannelReader = KeyChannel.Reader;
            KeyChannelWriter = KeyChannel.Writer;
        }

        public void Start()
        {
            if (IsStarted)
            {
                Console.WriteLine("Already started");
                return;
            }

            // TODO: Запись в канал записи (логи)

            KeyPlaybackBuffer = [];
            SW = new();

            m_GlobalHook.KeyDown += GlobalHookKeyDown;
            m_GlobalHook.KeyUp += GlobalHookKeyUp;

            SW.Start();
            IsStarted = true;
        }


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

                m_GlobalHook.KeyDown -= GlobalHookKeyDown;
                m_GlobalHook.KeyUp -= GlobalHookKeyUp;
                m_GlobalHook.Dispose();
            }
        }

        /// <summary>
        /// меняет оффсет таймингов для того чтобы начало было 0
        /// </summary>
        private void ReformatSequenceTimings()
        {
            var startTiming = KeyPlaybackBuffer.Keys.Min();
            var resultDict = KeyPlaybackBuffer.ToDictionary(
                kvp => kvp.Key - startTiming,
                kvp => kvp.Value
            );
            KeyPlaybackBuffer = resultDict;
        }

         private long? startTiming = null;

        private void GlobalHookKeyDown(object? sender, KeyEventArgs e)
        {
            var time = SW.ElapsedMicroseconds();
            // startTiming ??= time;
            // Console.WriteLine($"{time-startTiming} {time} {e.KeyCode}; is down");
            var keyEvent = new KeyEvent
            {
                KeyCode = e.KeyCode,
                Timestamp = time
            };
            if (KeyPlaybackBuffer.TryGetValue(time, out List<KeyEvent>? value))
            {
                value.Add(keyEvent);
            }
            else
            {
                KeyPlaybackBuffer[time] = [keyEvent];
            }
        }

        private void GlobalHookKeyUp(object? sender, KeyEventArgs e)
        {
            var time = SW.ElapsedMicroseconds();
            // startTiming ??= time;
            // Console.WriteLine($"{time-startTiming} {time} {e.KeyCode}; is up");
            var keyEvent = new KeyEvent
            {
                KeyCode = e.KeyCode,
                Timestamp = time
            };
            if (KeyPlaybackBuffer.TryGetValue(time, out List<KeyEvent>? value))
            {
                value.Add(keyEvent);
            }
            else
            {
                KeyPlaybackBuffer[time] = [keyEvent];
            }
        }
    }
}
