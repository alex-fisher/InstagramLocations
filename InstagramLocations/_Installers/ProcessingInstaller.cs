using System;
using System.Net;
using System.Net.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DotNetTor;
using InstagramLocations.Processor;

namespace InstagramLocations._Installers
{
    public class ProcessingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<HttpMessageHandler>()
                         .Instance(new TorSocks5Handler(new IPEndPoint(new IPAddress(BitConverter.ToInt32(IPAddress.Parse("127.0.0.1").GetAddressBytes(), 0)), 9050))));

            container.Register(Component.For<IInstagramProcessor>().ImplementedBy<InstagramProcessor>());
        }
    }
}
