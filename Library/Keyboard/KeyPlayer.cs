using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using AutoClicker.Library.Input;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library.Keyboard
{
    public class KeyPlayer
    {
        private Stopwatch SW;

        private bool Playing = false;
        private Task? KeyPlayingTask;

        public KeyPlayer()
        {
            SW = new();
        }

        private const long SleepAccuracyAdjustmentInMicroseconds = 1;
        public void Play(Dictionary<long, List<KeyEvent>> keyPlaybackBuffer, int interludeDelay)
        {
            if (!Playing)
            {
                KeyPlayingTask = Task.Run(async () =>
                {
                    Console.WriteLine(keyPlaybackBuffer.Count);
                    var currentTimestamp = 0L;
                    // TODO: сделать сборку последовательности на этапе остановки записи последовательности
                    var keyInputSequences = InputSequencer.BuildSequence(keyPlaybackBuffer);
                    var enumerator = keyInputSequences.GetEnumerator();

                    await Task.Delay(interludeDelay);

                    SW = new();
                    SW.Start();

                    while (enumerator.MoveNext())
                    {
                        try
                        {
                            currentTimestamp = SW.ElapsedMicroseconds();

                            while (currentTimestamp < enumerator.Current.Timestamp)
                            {
                                var remainingMicroseconds = enumerator.Current.Timestamp - currentTimestamp;
                                if (remainingMicroseconds > SleepAccuracyAdjustmentInMicroseconds)
                                {
                                    await Task.Delay(TimeSpan.FromTicks(remainingMicroseconds - SleepAccuracyAdjustmentInMicroseconds));
                                }
                                currentTimestamp = SW.ElapsedMicroseconds();
                            }

                            if (enumerator.Current.Sequence != null)
                            {
                                var err = NativeMethods.SendInput(
                                    (uint)enumerator.Current.Sequence.Length,
                                    enumerator.Current.Sequence,
                                    Marshal.SizeOf(typeof(INPUT))
                                );

                                if (err > 1)
                                {
                                    Console.WriteLine($"Error returned with SendInput. ErrorCode: {err}");
                                }
                                else
                                {
                                    Console.WriteLine($"Simulated Timestamp: {currentTimestamp} us");
                                }
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
