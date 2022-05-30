using Domain.Abstractions;
using System.Timers;
using static Domain.Constants.TimeConstants;

namespace Domain
{
    public class PomodoroTimer : IRunnable
    {
        private readonly Timer _timer;
        private readonly TimerConfiguration _configuration;

        private int _minutes;
        private int _seconds;

        public PomodoroTimer()
        {
            _timer = new Timer(OneSecondInMiliseconds);
            _timer.Elapsed += ProceedTimerElapsed;
        }

        public PomodoroTimer(TimerConfiguration configuration) : this()
        {
            _configuration = configuration;

            _minutes = _configuration.Minutes;
            _seconds = _configuration.Seconds;
        }

        public bool IsRunning { get => _timer.Enabled; }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public override string ToString()
        {
            var minutes = _minutes.ToString().PadLeft(2, '0');
            var seconds = _seconds.ToString().PadLeft(2, '0');

            return $"{minutes}:{seconds}";
        }

        private void ProceedTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_seconds == SecondsToSwitch)
            {
                if (_minutes == MinutesToFinish)
                {
                    Stop();

                    return;
                }

                _minutes--;
                _seconds = SecondsInMinute - 1;

                return;
            }

            _seconds--;
        }
    }
}
