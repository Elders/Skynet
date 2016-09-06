using Elders.Skynet.Core.Contracts.Packages;
using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Core.Handlers.Packages
{
    public class RegisterPackageHandler : IMessageHandler<RegisterPackage>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SkynetHost));

        public IPackageRepository PackagesRepository { get; set; }

        public void Handle(Message<RegisterPackage> message)
        {
            PackagesRepository.RegisterPackage(message.Payload.Location, message.Payload.Name);
        }
    }
}
