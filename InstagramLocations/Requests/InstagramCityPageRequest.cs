using System.Collections.Specialized;
using System.Net.Http;
using InstagramLocations.Providers;

namespace InstagramLocations.Requests
{
    public class InstagramCityPageRequest : InstagramRequest
    {
        public InstagramCityPageRequest() : base()
        {
        }

        public InstagramCityPageRequest(
            IUserAgentProvider userAgentProvider,
            IWebProxyProvider webProxyProvider)
            : base(userAgentProvider, webProxyProvider)
        {
        }

        public InstagramCityPageRequest(IUserAgentProvider userAgentProvider, HttpMessageHandler httpMessageHandler)
            : base(userAgentProvider, httpMessageHandler)
        {
        }

        public string GetCityPage(string cityName)
        {
            Logger.Info($"Executing city page request for user {cityName}");

            return DownloadString(
                $"{BaseUrl}/{cityName}",
                new NameValueCollection()
                {
                    {"__a", "1"}
                });
        }
    }
}
