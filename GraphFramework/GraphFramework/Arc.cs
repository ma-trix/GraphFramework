namespace GraphFramework
{
    public class Arc
    {
        public Vertex Start;
        public Vertex End;

        public Arc(Graph graph, Vertex start, Vertex end)
        {
            Start = start;
            End = end;
            Graph = graph;
        }

        public Graph Graph { get; private set; }

        public bool IsInMatching { get; private set; }

        public void AddToMatching()
        {
            IsInMatching = true;
        }
    }
}