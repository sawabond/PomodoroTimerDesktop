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
        private readonly IFileSerializer _fileSerializer = new FileSerializer();
        private TimerConfiguration _configuration;
        private PomodoroTimer _timer;
        private IFileSerializer fileSerializer = new FileSerializer();

        public TimerMainWindow()
        {
            InitializeComponent();
            SetupProgramBeforeWork();
        }

        private void SetupProgramBeforeWork()
        {
            ReadConfiguration();

            _timer = new PomodoroTimer(_configuration);
            _timer.AddOnTick(OnTimeChanged);
            _timer.AddOnTimeFinished(OnTimeFinished);

            _soundPlayer.Stream = Properties.Resources.TimeFinishedNotification;

            UpdateTimerView();
        }
        
        private void ReadConfiguration()
        {
            _configuration = fileSerializer.Deserialize<TimerConfiguration>(FileConstants.SettingsName);
        }

        private bool IsRunning => _timer.IsRunning;

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
            _fileSerializer.Serialize(_configuration, "config");
        }
    }
}
