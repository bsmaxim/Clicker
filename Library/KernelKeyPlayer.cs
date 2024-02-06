using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AutoClicker.Library.InputStructs;

namespace AutoClicker.Library
{
    public class KernelKeyPlayer
    {
        private Stopwatch SW;

        private Channel<string> KeyEventMessageChannel { get; }
        public ChannelReader<string> KeyEventMessageChannelReader { get; }
        private ChannelWriter<string> KeyEventMessageChannelWriter { get; }

        private bool Playing = false;
        private Task KeyPlayingTask;

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
                    var currentTimestamp = 0L;
                    var keyInputSequences = KeyInputSequencer.BuildSequence(keyPlaybackBuffer);
                    var enumerator = keyInputSequences.GetEnumerator();

                    SW = new();
                    SW.Start();

                    while (enumerator.MoveNext())
                    {
                        try
                        {
                            await Task.Delay(interludeDelay);

                            currentTimestamp = SW.ElapsedMicroseconds();
                            //var millisecondsToSleep = (enumerator.Current.FrameTimestamp - currentTimestamp) / 1_000.0 - ThresholdAwaitTaskDelayInMilliseconds;
                            //if (millisecondsToSleep > SleepAccuracyAdjustmentInMicroseconds)
                            //{
                            //    await Task.Delay((int)millisecondsToSleep);
                            //}

                            while (SW.ElapsedMicroseconds() < enumerator.Current.FrameTimestamp - SleepAccuracyAdjustmentInMicroseconds) { }


                            var err = NativeMethods.SendInput(
                                    (uint)enumerator.Current.InputSequence.Length,
                                    enumerator.Current.InputSequence,
                                    Marshal.SizeOf(typeof(INPUT))
                                );

                            Console.WriteLine((Keys)enumerator.Current.InputSequence[0].data.ki.keyCode);

                            if (err > 1)
                            {
                                Console.WriteLine($"Error returned with SendInput. ErrorCode: {err}");
                            }
                            else
                            {
                                Console.WriteLine($"Key Sequence:\r\n{enumerator.Current.InputSequence}\r\nSimulated Timestamp: {currentTimestamp} μs");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }

                    Console.WriteLine("after while");

                    Playing = false;
                });
            }
        }
    }
}
