namespace GraphFramework
{
    public class ABVertex : Vertex
    {
        public ABVertex(VertexType type)
        {
            Type = type;
        }

        public ABVertex Twin { get; private set; }
        public bool IsPushed { get; private set; }
        public VertexType Type { get; private set; }
        public ABVertex Lset { get; private set; }

        public void Pushed()
        {
            IsPushed = true;
        }

        public void SetTwin(ABVertex twin)
        {
            Twin = twin;
        }
    }
}