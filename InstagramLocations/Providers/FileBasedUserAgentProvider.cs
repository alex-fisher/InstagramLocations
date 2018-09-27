using System;
using System.IO;

namespace InstagramLocations.Providers
{
    public class FileBasedUserAgentProvider : IUserAgentProvider
    {
        private const string UserAgentFile = "Providers/Resources/UserAgentStrings.txt";
        private readonly string[] _userAgentStrings;

        public int UserAgentCount => _userAgentStrings.Length;

        public string[] UserAgentStrings => _userAgentStrings;

        public FileBasedUserAgentProvider()
        {
            try
            {
                _userAgentStrings = File.ReadAllLines(
                    UserAgentFile);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Opening Useragent File : {e.Message}");
                throw e;
            }
        }

        public string GetUserAgentString()
        {
            return _userAgentStrings[new Random().Next(0, UserAgentCount)];
        }
    }
}
