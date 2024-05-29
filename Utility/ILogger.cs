namespace SQMS.Utility
{
    public interface ILogger<T> where T : class
    {
        void LogWrite(T message);
    }
}
