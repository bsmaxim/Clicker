using System.Diagnostics;
using AutoClicker.Library.Input;

namespace AutoClicker.Library.Mouse
{
    public class MousePlayer
    {
        private Stopwatch SW;
        private bool Playing = false;
        private Task? KeyPlayingTask;

        public MousePlayer()
        {
            SW = new();
        }

        public void Play(Dictionary<long, List<MouseEvent>> keyPlaybackBuffer, int startCooldown)
        {
            if (!Playing)
            {
                KeyPlayingTask = Task.Run(async () =>
                {
                    //var currentTimestamp = 0L;

                    await Task.Delay(startCooldown);
                    var mouseInputSequences = InputSequencer.BuildSequence(keyPlaybackBuffer);

                    SW.Start();
                    foreach (var kvp in keyPlaybackBuffer)
                    {
                        while (SW.ElapsedMilliseconds < kvp.Key + startCooldown)
                        {
                            Thread.Sleep(1);
                        }
                        // foreach (var mouseEvent in kvp.Value)
                        // {
                        //     if (mouseEvent == MouseEventTypes.Move)
                        //     {

                        //     }
                        //     else if (mouseEvent.Type == MouseEventTypes.Down)
                        //     {
                        //         MouseSimulator.MouseDown(mouseEvent.Button);
                        //     }
                        //     else if (mouseEvent.Type == MouseEventTypes.Up)
                        //     {
                        //         MouseSimulator.MouseUp(mouseEvent.Button);
                        //     }
                        // }
                    }
                });

            }
        }
    }
}
