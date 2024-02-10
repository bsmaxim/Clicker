using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoClicker.Library
{
    public static class Extensions
    {
        private const long Million = 1_000_000L;

        // Прощедшее время в микросекундах
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ElapsedMicroseconds(this Stopwatch watch)
        {
            return (long)((double)watch.ElapsedTicks / Stopwatch.Frequency * Million);
        }
        public static int GetScreenX()
        {
            return NativeMethods.GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        }
        public static int GetScreenY()
        {
            return NativeMethods.GetSystemMetrics(SystemMetric.SM_CYSCREEN);
        }

        public static int CalculateAbsoluteX(int x)
        {
            return x * 65536 / GetScreenX();
        }

        public static int CalculateAbsoluteY(int y)
        {
            return y * 65536 / GetScreenY();
        }
    }
}
