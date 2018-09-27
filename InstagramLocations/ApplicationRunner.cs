using System;
using Castle.Core.Logging;
using InstagramLocations.Processor;

namespace InstagramLocations
{
    class ApplicationRunner : IApplicationRunner
    {
        private readonly IInstagramProcessor _instagramProcessor;
        public ILogger Logger { get; set; } = NullLogger.Instance;

        public ApplicationRunner(IInstagramProcessor instagramProcessor)
        {
            _instagramProcessor = instagramProcessor;
        }

        public int Run(Options options)
        {
            if (String.IsNullOrWhiteSpace(options.City))
            {
                Logger.Error($"City not provided");
                return 3;
            }

            _instagramProcessor.Run(options.City);

            return 1;
        }
    }
}
