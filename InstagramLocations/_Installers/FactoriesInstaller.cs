using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using InstagramLocations.Factories;

namespace InstagramLocations._Installers
{
    public class FactoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IQueryFactory>().ImplementedBy<QueryFactory>());
        }
    }
}
