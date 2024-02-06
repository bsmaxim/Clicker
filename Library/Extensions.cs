using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
