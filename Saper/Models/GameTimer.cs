using System;
using System.Diagnostics;

namespace Saper.Models
{
    public class GameTimer
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public void Start() => _stopwatch.Restart();
        public void Stop()  => _stopwatch.Stop();
        public TimeSpan Elapsed => _stopwatch.Elapsed;
    }
}
