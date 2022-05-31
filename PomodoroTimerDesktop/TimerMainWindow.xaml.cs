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
        private readonly Settings _settings;
        private TimerConfiguration _configuration;

        public TimerMainWindow(PomodoroTimer timer, TimerConfiguration configuration, IFileSerializer fileSerializer, Settings settings)
        {
            InitializeComponent();

            _timer = timer;
            _configuration = configuration;
            _fileSerializer = fileSerializer;
            _settings = settings;

            DataContext = this;

            SetupProgramBeforeWork();
        }

        private void UpdateMainVindowView()
        {
            UpdateTimerView();
            UpdateStartPauseButtonView();
        }

        private bool IsRunning => _timer.IsRunning;

        public void UpdateSettings(TimerConfiguration configuration)
        {
            _configuration = configuration;
            _timer.Configure(_configuration);

            _timer.Stop();
            UpdateMainVindowView();
        }

        private void SetupProgramBeforeWork()
        {
            ReadConfiguration();

            _settings.Configuration = _configuration;
            _settings.TimerMainWindow = this;

            _timer.Configure(_configuration);
            _timer.AddOnTick(OnTimeChanged);
            _timer.AddOnTimeFinished(OnTimeFinished);

            _soundPlayer.Stream = Properties.Resources.TimeFinishedNotification;

            UpdateMainVindowView();
        }

        private void ReadConfiguration()
        {
            _configuration = _fileSerializer.Deserialize<TimerConfiguration>(FileConstants.SettingsName);
        }

        private void OnTimeChanged(object sender, EventArgs e) => UpdateTimerView();

        private void OnTimeFinished(object sender, EventArgs e)
        {
            UpdateMainVindowView();
            PlaySoundWorkFinished();

            MessageBox.Show("Time finished!", "Attention!", MessageBoxButton.OK);
        }

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }

            UpdateMainVindowView();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Reset();

            UpdateMainVindowView();
        }

        private void UpdateTimerView() => Dispatcher.Invoke(() => Timer.Text = _timer.ToString());

        private void UpdateStartPauseButtonView() => Dispatcher.Invoke(() => StartPauseButton.Content = IsRunning ? "Pause" : "Start");

        private void PlaySoundWorkFinished() => Dispatcher.Invoke(() => _soundPlayer.Play());

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_settings.IsVisible)
            {
                _settings.Close();

                return;
            }

            _settings.Top = Top;
            _settings.Left = Left - ActualWidth;

            _settings.Show();
        }
    }
}
