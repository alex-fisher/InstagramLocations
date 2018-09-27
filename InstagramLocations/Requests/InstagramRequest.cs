using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using Castle.Core.Logging;
using DotNetTor;
using InstagramLocations.Providers;

namespace InstagramLocations.Requests
{
    public abstract class InstagramRequest : IInstagramRequest
    {
        protected const string BaseUrl = @"https://www.instagram.com";

        protected const int MaxProxyRequests = 50;
        protected const int MaxFailedProxyRequests = 3;
        protected const int MaxFailedRequests = 5;

        private readonly IUserAgentProvider _userAgentProvider;
        private readonly IWebProxyProvider _webProxyProvider;
        private readonly HttpMessageHandler _httpMessageHandler;

        private WebProxy _currentProxy;
        private int _proxyUseCount = 0;
        private int _failedProxyRequestCount = 0;
        private int _failedRequestCount = 0;

        public ILogger Logger { get; set; } = NullLogger.Instance;

        protected InstagramRequest()
        {
        }

        protected InstagramRequest(IUserAgentProvider userAgentProvider, IWebProxyProvider webProxyProvider)
        {
            _userAgentProvider = userAgentProvider;
            _webProxyProvider = webProxyProvider;
        }

        protected InstagramRequest(IUserAgentProvider userAgentProvider, HttpMessageHandler httpMessageHandler)
        {
            _userAgentProvider = userAgentProvider;
            _httpMessageHandler = httpMessageHandler;
        }

        public bool UsingProxy => _webProxyProvider != null;

        private HttpMessageHandler HttpClientHandler(WebProxy proxy) =>
            _httpMessageHandler ??
            new HttpClientHandler()
            {
                Proxy = proxy,
                UseProxy = proxy != null
            };

        public virtual string DownloadString(string uri, NameValueCollection queryCollection)
        {
            using (var client = GetClient(queryCollection))
            {
                try
                {
                    var requestUri = $"{uri}/{BuildQueryString(queryCollection)}";

                    Logger.Info($"Executing request for uri {requestUri}");

                    if (UsingProxy)
                    {
                        Logger.Info($"Incrementing proxyUseCount to {_proxyUseCount + 1}");

                        _proxyUseCount++;
                    }

                    if (_proxyUseCount >= MaxProxyRequests)
                    {
                        Logger.Info("Proxy count exceeded maximum, refreshing");

                        _currentProxy = null;
                    }

                    var message = client.GetAsync(requestUri).Result;

                    if (message.StatusCode == HttpStatusCode.MovedPermanently)
                    {
                        if (ShouldRefreshIp())
                            RefreshIp();
                    }

                    var content = message.Content.ReadAsStringAsync();

                    return content.Result;
                }
                catch (Exception e)
                {
                    Logger.Info($"Error connecting to host {uri} : {e.Message}");

                    if (UsingProxy && _currentProxy != null
                        && _failedProxyRequestCount++ >= MaxFailedProxyRequests)
                    {
                        Logger.Info($"Maxium proxy failed request amount reached, refreshing");

                        _currentProxy = null;
                        _failedProxyRequestCount = 0;
                    }

                    else if (ShouldRefreshIp())
                        RefreshIp();

                    return null;
                }
            }
        }

        public bool ShouldRefreshIp()
        {
            if (!UsingProxy && _failedRequestCount++ >= MaxFailedRequests)
            {
                Logger.Info($"Maxium failed request amount reached, refreshing IP.");
                return true;
            }

            return false;
        }

        public void RefreshIp()
        {
            var controlPortClient = new TorControlClient("127.0.0.1", controlPort: 9051, password: "InstagramLocations");
            controlPortClient.ChangeCircuitAsync().Wait();
        }
        
        private HttpClient GetClient(NameValueCollection queryCollection)
        {
            Logger.Info($"Acquiring request client with queryCollection {queryCollection}");

            HttpClient client = new HttpClient();
            if (UsingProxy && _currentProxy == null)
            {
                _currentProxy = new WebProxy(_webProxyProvider.GetWebProxyUrl());
                _currentProxy.UseDefaultCredentials = true;
                client = new HttpClient(HttpClientHandler(_currentProxy));

                Logger.Info($"Proxy enabled and using value {_currentProxy.Address}");
            }

            var userAgent = GetUserAgent();

            if (!string.IsNullOrWhiteSpace(userAgent))
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

            Logger.Info("Request client initialized");

            return client;
        }

        private string GetUserAgent()
        {
            if (_userAgentProvider != null)
            {
                var userAgent = _userAgentProvider.GetUserAgentString();

                Logger.Info($"User-Agent enabled and using value {userAgent}");

                return userAgent;
            }

            return null;
        }

        private string BuildQueryString(NameValueCollection queryCollection)
        {
            if (queryCollection.Keys.Count == 0)
                return string.Empty;

            var queryParameters = string.Join(
                "&",
                queryCollection.AllKeys.Select(k => $"{k}={queryCollection[k]}"));

            return $"?{queryParameters}".TrimEnd('&');
        }
    }
}
