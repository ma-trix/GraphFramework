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
                A = new ABVertex(VertexType.A, precursor.Name, Graph);
                B = new ABVertex(VertexType.B, precursor.Name, Graph);
            }
            else
            {
                A = new ABVertex(VertexType.A, Graph);
                B = new ABVertex(VertexType.B, Graph);
            }
            A.SetTwin(B);
            B.SetTwin(A);
        }

        public Vertex Precursor { get; set; }

        public TwinGraph Graph { get; set; }

        public ABVertex A { get; private set; }
        
        public ABVertex B { get; private set; }

        public String Name { get { return Precursor.Name + ".TV"; } }

        public bool InMatching { get; private set; }

        public Tuple<Arc, Arc> AddEdge(TwinVertex tv, bool inMatching)
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
                InMatching = true;
            }
            else
            {
                start1 = B;
                start2 = tv.B;
                end1 = A;
                end2 = tv.A;
            }
            var a1 = start1.AddOutboundArc(end2, inMatching);
            var a2 = start2.AddOutboundArc(end1, inMatching);
            return new Tuple<Arc, Arc>(a1, a2);
        }
    }
}
