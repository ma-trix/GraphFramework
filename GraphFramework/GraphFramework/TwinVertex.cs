namespace GraphFramework
{
    public class TwinVertex
    {
        public TwinVertex(Vertex precursor, TwinGraph tg)
        {
            Precursor = precursor;
            Graph = tg;
            A = new Vertex();
            B = new Vertex();
        }

        public Vertex Precursor { get; set; }

        public TwinGraph Graph { get; set; }

        public Vertex A { get; private set; }
        
        public Vertex B { get; private set; }

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
