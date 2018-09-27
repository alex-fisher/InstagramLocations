using System;
using InstagramLocations.Constants;
using Microsoft.Extensions.Configuration;

namespace InstagramLocations.Providers
{
    public class AppSettingsConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AppSettingsConnectionStringProvider(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public string GetConnectionString()
        {
            return _configurationRoot.GetConnectionString(ApplicationConstants.Database);
        }
    }
}
