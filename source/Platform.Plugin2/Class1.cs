using Platform.Data.Modules.Interfaces;
using Serilog;
using Serilog.Events;

namespace Platform.Plugin2
{
    public class Class1 : IScript
    {
        public string Name { get => "Test Plugin"; }

        public Class1()
        {

        }

        public Class1(ILogger logger)
        {
            logger.Write(LogEventLevel.Information, "HELLO FROM SECOND ASEMBLY!");
        }

        public void OnLoad()
        {
            Console.WriteLine("LOADED 2");
        }
    }
}