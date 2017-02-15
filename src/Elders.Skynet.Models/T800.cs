namespace Elders.Skynet.Models
{
    /// <summary> 
    /// Hasta la vista
    /// </summary>
    public interface T800
    {
        void PowerUp(params string[] args);
        void Command(params string[] args);
        void Shutdown(params string[] args);
    }
}
