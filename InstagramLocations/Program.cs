using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Services.Logging.Log4netIntegration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using CommandLine;
using InstagramLocations._Installers;
using Microsoft.Extensions.DependencyInjection;

namespace InstagramLocations
{
    // TODO:
    // Migrate to ORM
    // Async processing - Task Parallel Library
    // Make processor more configurable
    // Harden exception handling
    class Program
    {
        private static IWindsorContainer _container;
        private static IServiceProvider _serviceProvider;

        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            _serviceProvider = CreateServiceProvider(serviceCollection);

            var logger = _serviceProvider.GetService<ILoggerFactory>().Create(typeof(Program));

            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts, logger))
                  .WithNotParsed<Options>((errs) => HandleParseError(errs, logger));
        }

        private static void HandleParseError(IEnumerable<Error> errs, ILogger logger)
        {
            logger.Error(errs.ToString());
            Environment.Exit(1);
        }

        private static void RunOptionsAndReturnExitCode(Options opts, ILogger logger)
        {
            logger.Debug(opts.ToString());

            var result = _serviceProvider.GetService<IApplicationRunner>().Run(opts);

            Console.ReadKey();
            Environment.Exit(result);
        }

        private static IServiceProvider CreateServiceProvider(IServiceCollection serviceCollection)
        {
            _container = new WindsorContainer();

            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));

            InstallFacilities();

            _container.Install(new ApplicationInstaller());

            return WindsorRegistrationHelper.CreateServiceProvider(_container, serviceCollection);
        }

        private static void InstallFacilities()
        {
            // Add logging to the container
            _container.AddFacility<LoggingFacility>(f => f.LogUsing<ExtendedLog4netFactory>().WithConfig("log4net.xml"));
        }
    }
}
