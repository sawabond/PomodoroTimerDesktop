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
            _configuration = configuration;
        }
    }
}
