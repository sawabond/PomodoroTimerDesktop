using Domain;
using PomodoroTimerDesktop.Abstractions;
using System.ComponentModel;
using System.Windows;

namespace PomodoroTimerDesktop
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private TimerConfiguration _configuration;
        private TimerMainWindow _mainWindow;
        private readonly IFileSerializer _fileSerializer;

        public Settings(IFileSerializer fileSerializer)
        {
            InitializeComponent();

            DataContext = this;

            _fileSerializer = fileSerializer;
        }

        public TimerMainWindow TimerMainWindow
        {
            get => _mainWindow;
            set
            {
                if (_mainWindow is null)
                {
                    _mainWindow = value;
                    return;
                }

                throw new System.Exception();
            }
        }

        public TimerConfiguration Configuration
        {
            get => _configuration;
            set => _configuration = value;
        }

        public int MinutesToWork
        {
            get => _configuration.MinutesToWork;
            private set
            {
                _configuration.MinutesToWork = value;
                _fileSerializer.Serialize(_configuration, "settings");
            }
        }

        public int MinutesToRest
        {
            get => _configuration.MinutesToRest;
            private set
            {
                _configuration.MinutesToRest = value;
                _fileSerializer.Serialize(_configuration, "settings");
            }
        }

        private void ResetToDefaults_Click(object sender, RoutedEventArgs e)
        {
            _configuration = new TimerConfiguration();

            MinutesToWork = _configuration.MinutesToWork;
            MinutesToRest = _configuration.MinutesToRest;

            WorkMinutesTextBox.Text = MinutesToWork.ToString();
            RestMinutesTextBox.Text = MinutesToRest.ToString();

            _mainWindow.UpdateSettings(_configuration);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(WorkMinutesTextBox.Text, out int minutesToWork)
                && int.TryParse(RestMinutesTextBox.Text, out int minutesToRest))
            {
                MinutesToWork = minutesToWork;
                MinutesToRest = minutesToRest;

                _mainWindow.UpdateSettings(_configuration);

                return;
            }

            MessageBox.Show("Wrong values has been provided for inputs");
        }
    }
}
