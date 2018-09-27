using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstagramLocations.Models
{
    public class UserDetails
    {
        public string pk { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public bool is_private { get; set; }
        public string profile_pic_url { get; set; }
        public string profile_pic_id { get; set; }
        public bool is_verified { get; set; }
        public bool has_anonymous_profile_picture { get; set; }
        public int follower_count { get; set; }
        public string byline { get; set; }
        public double mutual_followers_count { get; set; }
        public string profile_url => $@"https://www.instagram.com/{username}";
    }

    public class SearchResultUser
    {
        public int position { get; set; }

        [JsonProperty("user")]
        public UserDetails details { get; set; }
    }

    public class Location
    {
        public string pk { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string short_name { get; set; }
        public double lng { get; set; }
        public double lat { get; set; }
        public string external_source { get; set; }
        public object facebook_places_id { get; set; }
    }

    public class PlaceDetails
    {
        public Location location { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public List<object> media_bundles { get; set; }
        public string slug { get; set; }
    }

    public class Place
    {
        [JsonProperty("place")]
        public PlaceDetails details { get; set; }
        public int position { get; set; }
    }

    public class HashtagDetails
    {
        public string name { get; set; }
        public long id { get; set; }
        public int media_count { get; set; }
    }

    public class Hashtag
    {
        public int position { get; set; }

        [JsonProperty("hashtag")]
        public HashtagDetails details { get; set; }
    }

    public class SearchResults
    {
        public List<SearchResultUser> users { get; set; }
        public List<Place> places { get; set; }
        public List<Hashtag> hashtags { get; set; }
        public bool has_more { get; set; }
        public string rank_token { get; set; }
        public bool clear_client_cache { get; set; }
        public string status { get; set; }
    }
}
