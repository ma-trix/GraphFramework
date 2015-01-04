using System.Collections.Generic;
using System.Text;
using ExampleGraphTests;
using GraphFramework;

namespace ExampleWeightedGraphTests
{
    static class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static LinkedList<Arc> _mAugmentingPath;

        static void Main(string[] args)
        {
            var lh = new LoggingHelper();
            lh.ClearLogFile();
            Log.Info("=====================================================================");
            var g0 = ExampleWeightedGraph.GenerateExampleWeightedTwinGraph2();
            TwinGraph g0Star = GenerateG0StarFrom(g0);

            _mAugmentingPath = new LinkedList<Arc>();
             var i = 0;
            var mAugmentingPaths = new LinkedList<LinkedList<Arc>>();
            while (true)
            {
                i++;
                Log.Info("ITERATION " + i + " =============================================");
                var k = new ABVertexStack();
                var l = new LinkedList<ABVertex>();
                var mdfsw = new MDFSW(g0Star, k, l);
                MDFSW._step = 0;

                _mAugmentingPath = mdfsw.Run();
                if (_mAugmentingPath == null)
                {
                    //Extension step instead of breaking
                    break;
                }
                mAugmentingPaths.AddLast(_mAugmentingPath);
                g0Star.SymmetricDifferenceWith(_mAugmentingPath);

                g0Star.LogArcsWeighted();
                g0Star.LogVerticesWeighted();
            }
            LogPaths(mAugmentingPaths);
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
                Log.Info(sb.ToString());
            }
        }

        private static TwinGraph GenerateG0StarFrom(TwinGraph g0)
        {
            double maxWeight = FindMaxWeightEdgeIn(g0);
            double initialVertexWeight = maxWeight/2;
            foreach (var tv in g0.Vertices)
            {
                tv.Precursor.DoubleWeight = initialVertexWeight;
            }
            TwinGraph g0Star = g0;
            return g0Star;
        }

        private static double FindMaxWeightEdgeIn(TwinGraph g0)
        {
            Arc maxElement = g0.Arcs.First.Value;
            LinkedList<Arc> arcsInGStar = new LinkedList<Arc>();
            foreach (var arc in g0.Arcs)
            {
                if (arc.Weight >= maxElement.Weight)
                {
                    if (arc.Weight > maxElement.Weight)
                    {
                        arcsInGStar = new LinkedList<Arc>();    //reset list of arcs to be added to GStar
                    }
                    arcsInGStar.AddLast(arc);
                    maxElement = arc;
                }
                else
                {
                    arc.IsInGStar = false;
                }
            }
            foreach (var arc in arcsInGStar)
            {
                arc.IsInGStar = true;
            }
            foreach (var outboundArc in g0.StartVertex.OutboundArcs)
            {
                outboundArc.IsInGStar = true;
            }
            foreach (var inboundArc in g0.EndVertex.InboundArcs)
            {
                inboundArc.IsInGStar = true;
            }
            return maxElement.Weight;
        }
    }

    internal class ExampleWeightedGraph
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static TwinGraph GenerateExampleWeightedTwinGraph1()
        {
            Graph g = GenerateExampleWeightedGraph1();
            Log.Info("Graph generated ===================================================");
            TwinGraph tg = new TwinGraph(g, true);
            return tg;
        }

        private static Graph GenerateExampleWeightedGraph1()
        {
            Graph g = new Graph();
            Vertex v1 = new Vertex("1");
            Vertex v2 = new Vertex("2");
            g.AddVertex(v1);
            g.AddVertex(v2);

            g.AddEdge(v1, v2, false, 10);

            return g;
        }

        public static TwinGraph GenerateExampleWeightedTwinGraph2()
        {
            Graph g = GenerateExampleWeightedGraph2();
            Log.Info("Graph generated ===================================================");
            TwinGraph tg = new TwinGraph(g, true);
            return tg;
        }

        private static Graph GenerateExampleWeightedGraph2()
        {
            Graph g = new Graph();
            Vertex v1 = new Vertex("1");
            Vertex v2 = new Vertex("2");
            Vertex v3 = new Vertex("3");
            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);

            g.AddEdge(v1, v2, false, 10);
            g.AddEdge(v2, v3, false, 8);

            return g;
        }
    }
}
