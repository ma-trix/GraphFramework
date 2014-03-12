using GraphFramework;

namespace ExampleGraphTests
{
    public class ExampleGraph
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Graph Generate2VertexExampleGraph()
        {
            Graph g = new Graph();
            Vertex v1 = new Vertex("1");
            Vertex v2 = new Vertex("2");
            Log.Info("Adding vertex " + v1.Name);
            g.AddVertex(v1);
            Log.Info("Adding vertex " + v2.Name);
            g.AddVertex(v2);
            Log.Info("Adding edge " + v1.Name + " <-> " + v2.Name);
            g.AddEdge(v1, v2);
            Log.Info("Removing edge " + v1.Name + " <-> " + v1.Name);
            g.RemoveEdge(v1,v2);
            g.AddArc(v1, v2);
            Log.Info("Adding arc " + v1.Name + " -> " + v2.Name);
            g.AddArc(v2, v1);
            Log.Info("Adding arc " + v2.Name + " -> " + v1.Name);
            g.RemoveArc(v2, v1);
            Log.Info("Removing arc " + v1.Name + " -> " + v2.Name);
            g.RemoveArc(v1, v2);
            Log.Info("Removing arc " + v2.Name + " -> " + v2.Name);
            g.RemoveVertex(v1);
            Log.Info("Removing vertex " + v1.Name);
            g.RemoveVertex(v2);
            Log.Info("Removing vertex " + v1.Name);
            return g;
        }

        public TwinGraph Generate2VertexExampleTwinGraph()
        {
            Graph g = Generate2VertexExampleGraph();
            TwinGraph tg = new TwinGraph(g);
            return tg;
        }

        public TwinGraph GenerateExampleTwinGraph()
        {
            Graph g = GenerateExampleGraph();
            TwinGraph tg = new TwinGraph(g);
            return tg;
        }

        public Graph GenerateExampleGraph()
        {
            Graph g = new Graph();
            Vertex v1 = new Vertex("1");
            Vertex v2 = new Vertex("2");
            Vertex v3 = new Vertex("3");
            Vertex v4 = new Vertex("4");
            Vertex v5 = new Vertex("5");
            Vertex v6 = new Vertex("6");
            Vertex v7 = new Vertex("7");
            Vertex v8 = new Vertex("8");
            Vertex v9 = new Vertex("9");
            Vertex v10 = new Vertex("10");
            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);
            g.AddVertex(v4);
            g.AddVertex(v5);
            g.AddVertex(v6);
            g.AddVertex(v7);
            g.AddVertex(v8);
            g.AddVertex(v9);
            g.AddVertex(v10);

            g.AddEdge(v1, v2);
            g.AddEdge(v2, v3);
            g.AddEdge(v3, v4);
            g.AddEdge(v4, v5);
            g.AddEdge(v5, v6);
            g.AddEdge(v6, v7);
            g.AddEdge(v7, v3);
            g.AddEdge(v6, v8);
            g.AddEdge(v8, v9);
            g.AddEdge(v9, v1);
            g.AddEdge(v2, v10);

            return g;
        }
    }
}