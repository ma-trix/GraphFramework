using System.Collections.Generic;
using System.Text;
using GraphFramework;

namespace ExampleGraphTests
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static LinkedList<Arc> _mAugmentingPath;

        static void Main(string[] args)
        {
            LoggingHelper lh = new LoggingHelper();
            lh.ClearLogFile();
            log.Info("=====================================================================");
            var e = new ExampleGraph();
            //e.Generate2VertexExampleGraph();
            var tg = e.GenerateExampleTwinGraph0();
            tg.LogVertices();
            tg.LogArcs();
            _mAugmentingPath = new LinkedList<Arc>();
            var i = 0;
            var _mAugmentingPaths = new LinkedList<LinkedList<Arc>>();
            while (true)
            {
                i++;
                log.Info("ITERATION " + i + " =============================================");
                var k = new ABVertexStack();
                var l = new LinkedList<ABVertex>();
                var mdfs = new MDFS(tg, k, l);
                MDFS._step = 0;
                
                _mAugmentingPath = mdfs.Run();
                if (_mAugmentingPath == null)
                {
                    break;
                }
                _mAugmentingPaths.AddLast(_mAugmentingPath);
                tg.SymmetricDifferenceWith(_mAugmentingPath);
                tg.LogArcs();
                tg.LogVertices();   
            }
            LogPaths(_mAugmentingPaths);
        }

        private static void LogPaths(LinkedList<LinkedList<Arc>> mAugmentingPaths)
        {
            var i = 0;
            foreach (var mAugmentingPath in mAugmentingPaths)
            {
                i++;
                var sb = new StringBuilder();
                sb.Append(i);
                sb.Append(" | ");
                foreach (var arc in mAugmentingPath)
                {
                    sb.Append(arc);
                    sb.Append(", ");
                }
                log.Info(sb.ToString());
            }
        }
    }
}
