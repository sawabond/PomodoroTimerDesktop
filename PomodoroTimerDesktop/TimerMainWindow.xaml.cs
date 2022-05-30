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
        private readonly TimerConfiguration _configuration = new TimerConfiguration();

        public TimerMainWindow()
        {
            InitializeComponent();

            _timer = new PomodoroTimer(_configuration);

            _timer.AddOnTick(OnTimeChanged);
            _timer.AddOnTimeFinished(OnTimeFinished);

            Timer.Text = _timer.ToString();
        }

        private bool IsRunning => _timer.IsRunning;

        private void OnTimeChanged(object sender, EventArgs e) => UpdateTimerView();

        private void OnTimeFinished(object sender, EventArgs e)
        {
            UpdateStartPauseButton();
            UpdateTimerView();

            MessageBox.Show("Time finished!", "Attention!", MessageBoxButton.OK);
        }

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                _timer.Stop();

                return;
            }

            _timer.Start();
            UpdateStartPauseButton();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e) => Close();

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Reset();

            UpdateTimerView();
            UpdateStartPauseButton();
        }

        private void UpdateTimerView() => Dispatcher.Invoke(() => Timer.Text = _timer.ToString());

        private void UpdateStartPauseButton() => Dispatcher.Invoke(() => StartPauseButton.Content = IsRunning ? "Pause" : "Start");
    }
}
