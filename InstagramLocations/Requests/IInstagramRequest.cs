using System.Collections.Specialized;

namespace InstagramLocations.Requests
{
    public interface IInstagramRequest
    {
        string DownloadString(string uri, NameValueCollection queryCollection);
    }
}
