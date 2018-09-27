using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramLocations.Processor
{
    public interface IInstagramProcessor
    {
        void Run(string city);
    }
}
