using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Windows.Forms;
using static AutoClicker.Library.InputStructs;

namespace AutoClicker.Library
{
    public class KernelKeyPlayer
    {
        private Stopwatch? SW;

        private Channel<string> KeyEventMessageChannel { get; }
        public ChannelReader<string> KeyEventMessageChannelReader { get; }
        private ChannelWriter<string> KeyEventMessageChannelWriter { get; }

        private bool Playing = false;
        private Task? KeyPlayingTask;

        public KernelKeyPlayer()
        {
            KeyEventMessageChannel = Channel.CreateUnbounded<string>();
            KeyEventMessageChannelReader = KeyEventMessageChannel.Reader;
            KeyEventMessageChannelWriter = KeyEventMessageChannel.Writer;
        }

        private const long SleepAccuracyAdjustmentInMicroseconds = 1;
        private const int ThresholdAwaitTaskDelayInMilliseconds = 1;

        public void Play(Dictionary<long, List<KeyEvent>> keyPlaybackBuffer, int interludeDelay)
        {
            if (!Playing)
            {
                KeyPlayingTask = Task.Run(async () =>
                {
                    Console.WriteLine(keyPlaybackBuffer.Count);
                    var currentTimestamp = 0L;
                    // TODO: сделать сборку последовательности на этапе остановки записи последовательности
                    var keyInputSequences = KeyInputSequencer.BuildSequence(keyPlaybackBuffer);
                    var enumerator = keyInputSequences.GetEnumerator();

                    await Task.Delay(interludeDelay);

                    SW = new();
                    SW.Start();

                    while (enumerator.MoveNext())
                    {
                        try
                        {
                            currentTimestamp = SW.ElapsedMicroseconds();

                            while (currentTimestamp < enumerator.Current.FrameTimestamp)
                            {
                                var remainingMicroseconds = enumerator.Current.FrameTimestamp - currentTimestamp;
                                if (remainingMicroseconds > SleepAccuracyAdjustmentInMicroseconds)
                                {
                                    await Task.Delay(TimeSpan.FromTicks(remainingMicroseconds - SleepAccuracyAdjustmentInMicroseconds));
                                }
                                currentTimestamp = SW.ElapsedMicroseconds();
                            }



                            var err = NativeMethods.SendInput(
                                    (uint)enumerator.Current.InputSequence.Length,
                                    enumerator.Current.InputSequence,
                                    Marshal.SizeOf(typeof(INPUT))
                                );

                            // Console.WriteLine((Keys)enumerator.Current.InputSequence[0].data.ki.keyCode);

                            if (err > 1)
                            {
                                Console.WriteLine($"Error returned with SendInput. ErrorCode: {err}");
                            }
                            else
                            {
                                Console.WriteLine($"Simulated Timestamp: {currentTimestamp} us");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }

                    Playing = false;
                });
            }
        }
    }
}
