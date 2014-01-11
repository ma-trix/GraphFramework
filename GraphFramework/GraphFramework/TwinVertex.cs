namespace GraphFramework
{
    public class TwinVertex
    {
        public TwinVertex(Vertex precursor, TwinGraph tg)
        {
            Precursor = precursor;
            Graph = tg;
            A = new ABVertex(VertexType.A);
            B = new ABVertex(VertexType.B);
            A.SetTwin(B);
            B.SetTwin(A);
        }

        public Vertex Precursor { get; set; }

        public TwinGraph Graph { get; set; }

        public ABVertex A { get; private set; }
        
        public ABVertex B { get; private set; }

        public void AddNonMatchingEdge(TwinVertex tv2)
        {
            B.AddOutboundArc(tv2.A);
            tv2.B.AddOutboundArc(A);
        }

        public void AddMatchingEdge(TwinVertex tv2)
        {
            A.AddOutboundArc(tv2.B);
            tv2.A.AddOutboundArc(B);
        }
    }
}
