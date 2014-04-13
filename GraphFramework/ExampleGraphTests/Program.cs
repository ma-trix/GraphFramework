using System.Collections.Generic;
using GraphFramework;

namespace ExampleGraphTests
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            LoggingHelper lh = new LoggingHelper();
            lh.ClearLogFile();
            log.Info("=====================================================================");
            var e = new ExampleGraph();
            //e.Generate2VertexExampleGraph();
            var tg = e.GenerateExampleTwinGraph();
            tg.LogVertices();
            tg.LogArcs();
            var k = new ABVertexStack();
            var l = new LinkedList<ABVertex>();
            var mdfs = new MDFS(tg, k, l);
            var maug = mdfs.Run();
            tg.SymmetricDifferenceWith(maug);
            tg.LogArcs();
            tg.LogVertices();
        }
    }
}
