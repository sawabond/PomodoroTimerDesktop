namespace Domain
{
    public class TimerConfiguration
    {
        public int MinutesToWork { get; set; } = 25;

        public int SecondsToWork { get; set; } = 0;

        public int MinutesToRest { get; set; } = 5;

        public int SecondsToRest { get; set; } = 0;
    }
}
