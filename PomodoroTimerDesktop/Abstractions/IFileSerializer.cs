namespace PomodoroTimerDesktop.Abstractions
{
    public interface IFileSerializer
    {
        bool Serialize(object obj, string fileName);

        T Deserialize<T>(string fileName) where T : new();
    }
}
