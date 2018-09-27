using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using InstagramLocations.Requests;

namespace InstagramLocations._Installers
{
    public class RequestsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IInstagramRequest>().ImplementedBy<InstagramRequest>());
        }
    }
}
