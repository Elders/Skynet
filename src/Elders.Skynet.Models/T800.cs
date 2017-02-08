namespace Elders.Skynet.Models
{
    public interface T800
    {
        void PowerUp(params string[] args);
        void Command(params string[] args);
        void Shutdown(params string[] args);
    }
}
