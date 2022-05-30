using Domain.Abstractions;
using Domain.Enums;
using System;
using System.Timers;
using static Domain.Constants.TimeConstants;

namespace Domain
{
    public class PomodoroTimer : IRunnable
    {
        private readonly Timer _timer;
        private readonly TimerConfiguration _configuration;

        private EventHandler _timeFinished;
        private int _minutes;
        private int _seconds;
        private TimerMode _mode = TimerMode.Work;

        public PomodoroTimer()
        {
            _timer = new Timer(OneSecondInMiliseconds);
            _timer.Elapsed += ProceedTimerElapsed;
            _timeFinished += TimeFinished;
        }

        public PomodoroTimer(TimerConfiguration configuration) : this()
        {
            _configuration = configuration;

            _minutes = _configuration.Minutes;
            _seconds = _configuration.Seconds;
        }
        public bool IsRunning { get => _timer.Enabled; }

        public void AddOnTick(ElapsedEventHandler handler)
        {
            _timer.Elapsed += handler;
        }

        public void AddOnTimeFinished(EventHandler handler)
        {
            _timeFinished += handler;
        }

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

        private void TimeFinished(object sender, EventArgs e)
        {
            switch (_mode)
            {
                case TimerMode.Work:
                    _mode = TimerMode.Rest;
                    break;
                case TimerMode.Rest:
                    _mode = TimerMode.Work;
                    break;
                default:
                    _mode = TimerMode.Work;
                    break;
            }

            Stop();
        }

        private void ProceedTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_seconds == SecondsToSwitch)
            {
                if (_minutes == MinutesToFinish)
                {
                    _timeFinished?.Invoke(sender, e);

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
