using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramLocations.Providers
{
    public interface IUserAgentProvider
    {
        string GetUserAgentString();
    }
}
