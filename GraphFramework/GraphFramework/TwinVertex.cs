using System;

namespace GraphFramework
{
    public class TwinVertex
    {
        public TwinVertex(Vertex precursor, TwinGraph tg)
        {
            Precursor = precursor;
            Graph = tg;
            if (precursor != null)
            {
                A = new ABVertex(VertexType.A, precursor.Name);
                B = new ABVertex(VertexType.B, precursor.Name);
            }
            else
            {
                A = new ABVertex(VertexType.A);
                B = new ABVertex(VertexType.B);
            }
            A.SetTwin(B);
            B.SetTwin(A);
        }

        public Vertex Precursor { get; set; }

        public TwinGraph Graph { get; set; }

        public ABVertex A { get; private set; }
        
        public ABVertex B { get; private set; }

        public String Name { get { return Precursor.Name + ".TV"; } }

        public void AddNonMatchingEdge(TwinVertex tv2)
        {
            B.AddOutboundArc(tv2.A, false);
            tv2.B.AddOutboundArc(A, false);
        }

        public void AddMatchingEdge(TwinVertex tv2)
        {
            A.AddOutboundArc(tv2.B, true);
            tv2.A.AddOutboundArc(B, true);
        }
    }
}
