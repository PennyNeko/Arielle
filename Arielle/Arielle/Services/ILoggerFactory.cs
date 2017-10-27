namespace Arielle
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string v);
        void AddConsole();
    }
}