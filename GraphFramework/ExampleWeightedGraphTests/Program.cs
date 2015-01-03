using ExampleGraphTests;

namespace ExampleWeightedGraphTests
{
    static class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var lh = new LoggingHelper();
            lh.ClearLogFile();
            Log.Info("=====================================================================");
        }
    }
}
