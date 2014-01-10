namespace GraphFramework
{
    public class Arc
    {
        public Vertex Start { get; set; }
        public Vertex End { get; set; }
        public Graph Graph { get; private set; }
        public bool IsInMatching { get; private set; }
        
        public Arc(Graph graph, Vertex start, Vertex end)
        {
            Start = start;
            End = end;
            Graph = graph;
        }

        public void AddToMatching()
        {
            IsInMatching = true;
        }
    }
}