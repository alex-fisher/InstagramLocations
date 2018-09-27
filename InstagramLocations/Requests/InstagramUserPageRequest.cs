using System.Collections.Specialized;
using System.Net.Http;
using InstagramLocations.Providers;

namespace InstagramLocations.Requests
{
    public class InstagramUserPageRequest : InstagramRequest
    {
        public InstagramUserPageRequest(IUserAgentProvider userAgentProvider, IWebProxyProvider webProxyProvider)
            : base(userAgentProvider, webProxyProvider)
        {
        }

        public InstagramUserPageRequest(IUserAgentProvider userAgentProvider, HttpMessageHandler httpMessageHandler)
            : base(userAgentProvider, httpMessageHandler)
        {
        }

        public string GetUserPage(string profileUrl, string maxMediaId = null)
        {
            Logger.Info($"Executing user page request for user {profileUrl}");

            var queryCollection = new NameValueCollection()
                                  {
                                      {"__a", "1"}
                                  };

            if (maxMediaId != null)
                queryCollection.Add("max_id", maxMediaId);

            return DownloadString(
                profileUrl, queryCollection);
        }
    }
}
