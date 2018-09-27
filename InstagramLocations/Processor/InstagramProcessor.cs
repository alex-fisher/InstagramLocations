using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Castle.Core.Logging;
using DotNetTor;
using InstagramLocations.Models;
using InstagramLocations.Providers;
using InstagramLocations.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Media = Instacram.Models.MediaModels.Media;

namespace InstagramLocations.Processor
{
    // TODO - Refactor this class into new async classes
    public class InstagramProcessor : IInstagramProcessor
    {
        public ILogger Logger { get; set; } = NullLogger.Instance;

        private readonly InstagramSearchRequest _instagramSearchRequest;
        private readonly InstagramUserPageRequest _instagramUserPageRequest;
        private readonly InstagramMediaRequest _instagramMediaRequest;
        private readonly InstagramCityPageRequest _instagramCityPageRequest;

        public InstagramProcessor(IUserAgentProvider userAgentProvider, HttpMessageHandler messageHandlerOverride)
        {
            Logger.Info("Initializing Controller");

            _instagramSearchRequest = new InstagramSearchRequest(userAgentProvider, messageHandlerOverride);
            _instagramUserPageRequest = new InstagramUserPageRequest(userAgentProvider, messageHandlerOverride);
            _instagramMediaRequest = new InstagramMediaRequest(userAgentProvider, messageHandlerOverride);
            _instagramCityPageRequest = new InstagramCityPageRequest();

            Logger.Info("Controller Initialized");
        }

        public InstagramProcessor(IUserAgentProvider userAgentProvider, IWebProxyProvider webProxyProvider)
        {
            Logger.Info("Initializing Controller");

            _instagramSearchRequest = new InstagramSearchRequest(userAgentProvider, webProxyProvider);
            _instagramUserPageRequest = new InstagramUserPageRequest(userAgentProvider, webProxyProvider);
            _instagramMediaRequest = new InstagramMediaRequest(userAgentProvider, webProxyProvider);
            _instagramCityPageRequest = new InstagramCityPageRequest(userAgentProvider, webProxyProvider);

            Logger.Info("Controller Initialized");
        }

        public void Run(string city)
        {
            Logger.Info("Running");

            UserPage cityPage;
            using (var driver = new ChromeDriver(Environment.CurrentDirectory))
            {
                // TODO Ugly, refactor to class and parser.
                driver.Navigate().GoToUrl($@"https://www.instagram.com/{city}/");
                var result = driver.ExecuteScript("return window._sharedData");
                var resultJson = JObject.Parse(JsonConvert.SerializeObject(result));
                cityPage = JsonConvert.DeserializeObject<UserPage>((resultJson["entry_data"]["ProfilePage"] as JArray).First()["graphql"].ToString());
            }

            Crawl(cityPage);

            Logger.Info($"City page not found {city}, exiting.");
        }

        private void Crawl(UserPage userPage)
        {
            var mediaPages = GetMediaPages(userPage);

            var maxMediaId = userPage.user.media.nodes.Last()
                                .id;

            foreach (var mediaPage in mediaPages)
            {
                var location = mediaPage.graphql.shortcode_media.location;

                if (location != null)
                    Console.WriteLine("Media Location found");

                if (mediaPage.graphql.shortcode_media.owner.profile_url != userPage.user.profile_url && !mediaPage.graphql.shortcode_media.owner.is_private)
                    Crawl(GetUserPage(mediaPage.graphql.shortcode_media.owner.profile_url));
            }

            Crawl(GetNextUserMediaPage(userPage, maxMediaId));
        }

        private UserPage GetCityPage(string city)
        {
            var result = _instagramCityPageRequest.GetCityPage(city);

            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserPage>(result);
        }

        private List<UserPage> GetUserPages(SearchResults searchResults)
        {
            return searchResults.users.Select(u => GetUserPage(u.details.profile_url))
                                .ToList();
        }

        private List<Media> GetMediaPages(UserPage userPage)
        {
            return userPage.user.media.nodes.Select(GetMedia)
                           .ToList();
        }

        private UserPage GetNextUserMediaPage(UserPage userPage, string maxMediaId)
        {
            return GetUserPage(userPage.user.profile_url, maxMediaId);
        }

        private SearchResults Search(string query)
        {
            var result = _instagramSearchRequest.Search(query);

            if (result == null)
                return null;

            Throttle();

            return JsonConvert.DeserializeObject<SearchResults>(result);
        }

        private UserPage GetUserPage(string profileUrl, string maxMediaId = null)
        {
            var result = _instagramUserPageRequest.GetUserPage(profileUrl, maxMediaId);

            if (result == null)
                return null;

            Throttle();

            return JsonConvert.DeserializeObject<UserPage>(result);
        }

        private Media GetMedia(Node mediaNode)
        {
            var result = _instagramMediaRequest.GetMediaPage(mediaNode);

            if (result == null)
                return null;

            Throttle();

            return
                JsonConvert.DeserializeObject<Media>(result);
        }

        public void Throttle()
        {
            var sleepDuration = NextSleepDuration;

            Logger.Info($"Get Media Request made, sleeping {sleepDuration / 1000} seconds prior.");

            Thread.Sleep(sleepDuration);
        }

        private int NextSleepDuration =>
            new Random().Next(1000, 10000);
    }
}
