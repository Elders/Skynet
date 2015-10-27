using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Elders.Skynet.Core.Handlers.Packages;
using Elders.Skynet.Core.Output;
using Elders.Skynet.Core.Packages;
using Elders.Skynet.Core.Processes;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Core.Util;
using log4net;

namespace Elders.Skynet.Core
{
    public class SkynetHost : IObserver<IConnection>
    {
        private ConcurrentList<SkynetConnection> connectedClients;

        public string HostName { get; private set; }

        private BasicContainer container;

        private SkynetProcessRegistry processRegistry;

        private IPackageRepository packageRepo;

        public Guid ServerId;

        private IServer server;

        private int processId;

        private Timer timer;

        public IPackageExplorer packageExplorer;

        public SkynetHost(string hostName, IServer server, IPackageExplorer packageExplorer)
        {
            this.packageExplorer = packageExplorer;
            HostName = hostName;
            var currentProcess = Process.GetCurrentProcess();
            processId = currentProcess.Id;
            ServerId = Guid.NewGuid();
            this.server = server;
            connectedClients = new ConcurrentList<SkynetConnection>();
            container = new BasicContainer();
            packageRepo = new BasicFilePackageRepository();
            processRegistry = new SkynetProcessRegistry(packageRepo);
            processRegistry.Heartbeat(ServerId, processId);
            container.Register(() => packageRepo);
            container.Register(() => processRegistry);
            container.Register(() => this);
            container.Register(() => packageExplorer);
            var serverProcess = processRegistry.GetProcesss(processId);
            var repository = LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
            repository.Root.AddAppender(new OutputAppender(serverProcess.Output));
            container.Register(() => serverProcess.Output);
            timer = new Timer(x => { processRegistry.Heartbeat(ServerId, processId); }, null, TimeSpan.FromMilliseconds(1), TimeSpan.FromSeconds(10));
        }

        public void Send(Guid connectionId, IMessage message)
        {
            var connection = connectedClients.Where(x => x.Id == connectionId).SingleOrDefault();
            connection.SendMessage(message);
        }

        public void Start()
        {
            server.Start();
            server.Subscribe(this);
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(IConnection connection)
        {
            connectedClients.Add(new SkynetConnection(HostName, connection, new SkynetPublisher(container.Resolve), new JsonSerializer()));
        }
    }
}
