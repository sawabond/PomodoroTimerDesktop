using Domain;
using System;
using System.Windows;

namespace PomodoroTimerDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TimerMainWindow : Window
    {
        private readonly PomodoroTimer _timer;
        private readonly TimerConfiguration _configuration = new TimerConfiguration { Minutes = 0, Seconds = 02 };

        public TimerMainWindow()
        {
            InitializeComponent();

            _timer = new PomodoroTimer(_configuration);

            _timer.AddOnTick(OnTimeChanged);
            _timer.AddOnTimeFinished(OnTimeFinished);

            Timer.Text = _timer.ToString();
        }

        private bool IsRunning => _timer.IsRunning;

        private void OnTimeChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Timer.Text = _timer.ToString();
            });
        }

        private void OnTimeFinished(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StartPauseButton.Content = "Start";
            });

            MessageBox.Show("time finished!");
        }

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                _timer.Stop();

                return;
            }

            _timer.Start();

            StartPauseButton.Content = IsRunning ? "Pause" : "Start";
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
