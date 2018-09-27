using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using InstagramLocations.Providers;

namespace InstagramLocations._Installers
{
    public class ProvidersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUserAgentProvider>().ImplementedBy<FileBasedUserAgentProvider>());
            container.Register(Component.For<IConnectionStringProvider>().ImplementedBy<AppSettingsConnectionStringProvider>());
        }
    }
}
