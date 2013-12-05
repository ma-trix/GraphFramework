namespace GraphFramework
{
    public class Arc
    {
        public Vertex Start;
        public Vertex End;
        private Graph graph;

        public Arc(Graph graph, Vertex start, Vertex end)
        {
            Start = start;
            End = end;
            this.graph = graph;
        }

        public Graph Graph
        {
            get { return graph; }
        }
    }
}