using System;
using System.IO;

namespace InstagramLocations.Providers
{
    // Consider creating web based proxy provider vs static file.
    public class FileBasedWebProxyProvider : IWebProxyProvider
    {
        private const string UserAgentFile = "Providers/Resources/ProxyStrings.txt";
        private readonly string[] _proxyStrings;

        public int ProxyCount => _proxyStrings.Length;

        public string[] ProxyStrings => _proxyStrings;

        public FileBasedWebProxyProvider()
        {
            try
            {
                _proxyStrings = File.ReadAllLines(
                    UserAgentFile);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Opening proxy File : {e.Message}");
                throw e;
            }
        }

        public string GetWebProxyUrl()
        {
            var proxyIp = _proxyStrings[new Random().Next(0, _proxyStrings.Length)];
            return string.Format("https://{0}/", proxyIp);
        }
    }
}
