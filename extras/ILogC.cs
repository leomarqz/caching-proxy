
namespace caching_proxy.extras
{
    public interface ILogC
    {
        void Ok(string message);
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Error(string message);
    }
}