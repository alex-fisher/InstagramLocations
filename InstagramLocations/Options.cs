using CommandLine;

namespace InstagramLocations
{
    public class Options
    {
        [Option('c', "city", Required = false, HelpText = "Initial city to process")]
        public string City { get; set; }
    }
}
