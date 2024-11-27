using System.Diagnostics;
using System.Runtime.InteropServices;
using AutoClicker.Library.Input;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library
{

	public class InputPlayer
	{
		protected const long SleepAccuracyAdjustmentInMicroseconds = 1;
		protected Stopwatch SW;
		protected bool Playing = false;
		protected Task? KeyPlayingTask;
		public InputPlayer()
		{
			SW = new();
		}

		public void Play(Dictionary<long, List<IInputEvent>> keyPlaybackBuffer, int interludeDelay)
		{
			if (!Playing)
			{
				if (keyPlaybackBuffer.Count == 0)
				{
					return;
				}

				KeyPlayingTask = Task.Run(async () =>
				{
					var currentTimestamp = 0L;
					// TODO: сделать сборку последовательности на этапе остановки записи последовательности
					var inputSequences = GetInputSequences(keyPlaybackBuffer);
					var enumerator = inputSequences.GetEnumerator();

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

							if (enumerator.Current.Value != null)
							{
								var err = MySendInput(enumerator.Current.Value);


								if (err > 1)
								{
									Console.WriteLine($"Error returned with SendInput. ErrorCode: {err}");
								}
								else // всё норм
								{
									// Console.WriteLine($"Simulated Timestamp: {currentTimestamp} us");
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

		public virtual List<InputUnit> GetInputSequences(Dictionary<long, List<IInputEvent>> keyPlaybackBuffer)
		{
			throw new NotImplementedException();
		}

		protected virtual uint MySendInput(INPUT[] inputs)
		{
			return WinApi.SendInput(
									(uint)inputs.Length,
									inputs,
									Marshal.SizeOf(typeof(INPUT))
								);
		}

	}
}
