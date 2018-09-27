using System.Collections.Specialized;
using System.Net.Http;
using InstagramLocations.Models;
using InstagramLocations.Providers;

namespace InstagramLocations.Requests
{
    public class InstagramMediaRequest : InstagramRequest
    {
        public InstagramMediaRequest(IUserAgentProvider userAgentProvider, IWebProxyProvider webProxyProvider)
            : base(userAgentProvider, webProxyProvider)
        {
        }

        public InstagramMediaRequest(IUserAgentProvider userAgentProvider, HttpMessageHandler httpMessageHandler)
            : base(userAgentProvider, httpMessageHandler)
        {
        }

        public string GetMediaPage(Node mediaNode)
        {
            Logger.Info($"Executing user page request for user {mediaNode.media_url}");

            return DownloadString(
                mediaNode.media_url,
                new NameValueCollection()
                {
                    {"__a", "1"}
                });
        }
    }
}
