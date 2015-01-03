using ExampleGraphTests;
using GraphFramework;

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
            var wtg1 = ExampleWeightedGraph.GenerateExampleWeightedTwinGraph1();
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
            TwinGraph tg = new TwinGraph(g);
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
    }
}
