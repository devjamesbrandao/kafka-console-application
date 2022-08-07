using CommandLine;
namespace Kafka
{
    public class Options
    {
        [Option('r', "requests", Default = 1, Required = false)]
        public int requisicoes { get; set; }

        [Option('h', "host", Default = "localhost", Required = false)]
        public string host { get; set; }
    }
}
