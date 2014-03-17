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

        public void AddEdge(TwinVertex tv, bool inMatching)
        {
            ABVertex start1;
            ABVertex start2;
            ABVertex end1;
            ABVertex end2;
            if (inMatching)
            {
                start1 = A;
                start2 = tv.A;
                end1 = B;
                end2 = tv.B;
            }
            else
            {
                start1 = B;
                start2 = tv.B;
                end1 = A;
                end2 = tv.A;
            }
            start1.AddOutboundArc(end2, inMatching);
            start2.AddOutboundArc(end1, inMatching);
        }
    }
}
