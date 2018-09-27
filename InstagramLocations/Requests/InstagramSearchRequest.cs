using System.Collections.Specialized;
using System.Net.Http;
using InstagramLocations.Providers;

namespace InstagramLocations.Requests
{
    public class InstagramSearchRequest : InstagramRequest
    {
        private readonly string _searchUrl = $"{BaseUrl}/web/search/topsearch/";

        //TODO - Add csrf token / cookie headers
        public InstagramSearchRequest(IUserAgentProvider userAgentProvider, IWebProxyProvider webProxyProvider)
            : base(userAgentProvider, webProxyProvider)
        {
        }

        public InstagramSearchRequest(IUserAgentProvider userAgentProvider, HttpMessageHandler httpMessageHandler)
            : base(userAgentProvider, httpMessageHandler)
        {
        }

        public string Search(string query)
        {
            Logger.Info($"Executing search for query {query}");

            return DownloadString(
                _searchUrl,
                new NameValueCollection
                {
                    {"context", "blended"},
                    {"query", query}
                });
        }
    }
}
