using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoClicker.Library
{
    public static class Extensions
    {
        private const long Million = 1_000_000L;

        // Прощедшее время в наносекундах

        // Прощедшее время в микросекундах
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ElapsedMicroseconds(this Stopwatch watch)
        {
            return (long)((double)watch.ElapsedTicks / Stopwatch.Frequency * Million);
        }
    }
}
