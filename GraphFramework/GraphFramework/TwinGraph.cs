namespace GraphFramework
{
    public class TwinGraph
    {
        public Vertex StartVertex
        {
            get { return _startVertex; }
        }

        public Vertex EndVertex
        {
            get {return _endVertex; }
        }

        private readonly Vertex _startVertex;
        private readonly Vertex _endVertex;

        public TwinGraph()
        {
            _startVertex = new Vertex();
            _endVertex = new Vertex();
        }
    }
}