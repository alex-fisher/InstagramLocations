using System;
using System.Collections.Generic;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using InstagramLocations.Processor;

namespace InstagramLocations._Installers
{
    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(
                new ProcessingInstaller(),
                new DataAccessInstaller(),
                new ProvidersInstaller(),
                new FactoriesInstaller(),
                new RequestsInstaller());

            container.Register(Component.For<IApplicationRunner>().ImplementedBy<ApplicationRunner>());
        }
    }
}
