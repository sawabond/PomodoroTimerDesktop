using Domain;
using System.Windows;

namespace PomodoroTimerDesktop
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private readonly TimerConfiguration _configuration;

        public Settings(TimerConfiguration configuration)
        {
            InitializeComponent();

            DataContext = this;

            _configuration = configuration;
        }

        public int MinutesToWork
        {
            get => _configuration.MinutesToWork;
            set => _configuration.MinutesToWork = value;
        }

        public int MinutesToRest
        {
            get => _configuration.MinutesToRest;
            set => _configuration.MinutesToRest = value;
        }
    }
}
