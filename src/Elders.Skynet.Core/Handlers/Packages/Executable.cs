namespace Elders.Skynet.Core.Handlers.Packages
{
    public interface IExectuable
    {
        string ExecutableLocation { get; set; }

        string[] Args { get; set; }
    }
}