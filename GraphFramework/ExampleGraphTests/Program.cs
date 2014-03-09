using System;

namespace ExampleGraphTests
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            log.Info("=====================================================================");
            var e = new ExampleGraph();
            e.Generate2VertexExampleTwinGraph();
            Console.ReadLine();
        }
    }
}
