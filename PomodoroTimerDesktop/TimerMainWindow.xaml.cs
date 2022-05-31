using Domain;
using PomodoroTimerDesktop.Abstractions;
using PomodoroTimerDesktop.Constants;
using PomodoroTimerDesktop.Models;
using System;
using System.Media;
using System.Windows;

namespace PomodoroTimerDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TimerMainWindow : Window
    {
        private readonly SoundPlayer _soundPlayer = new SoundPlayer();
        private readonly PomodoroTimer _timer;
        private readonly IFileSerializer _fileSerializer;
        private TimerConfiguration _configuration;

        public TimerMainWindow()
        {

        }

        public TimerMainWindow(PomodoroTimer timer, TimerConfiguration configuration, IFileSerializer fileSerializer)
        {
            InitializeComponent();

            _soundPlayer = new SoundPlayer();

            _timer = timer;
            _configuration = configuration;
            _fileSerializer = fileSerializer;

            DataContext = this;

            SetupProgramBeforeWork();
        }

        private bool IsRunning => _timer.IsRunning;

        private void SetupProgramBeforeWork()
        {
            ReadConfiguration();

            _timer.Configure(_configuration);
            _timer.AddOnTick(OnTimeChanged);
            _timer.AddOnTimeFinished(OnTimeFinished);

            _soundPlayer.Stream = Properties.Resources.TimeFinishedNotification;

            UpdateTimerView();
        }

        private void ReadConfiguration()
        {
            _configuration = _fileSerializer.Deserialize<TimerConfiguration>(FileConstants.SettingsName);
        }

        private void OnTimeChanged(object sender, EventArgs e) => UpdateTimerView();

        private void OnTimeFinished(object sender, EventArgs e)
        {
            UpdateStartPauseButton();
            UpdateTimerView();
            PlaySoundWorkFinished();

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

        private void PlaySoundWorkFinished() => Dispatcher.Invoke(() => _soundPlayer.Play());

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new Settings(_configuration);

            settingsWindow.Top = Top;
            settingsWindow.Left = Left - ActualWidth;

            settingsWindow.Show();
        }
    }
}
