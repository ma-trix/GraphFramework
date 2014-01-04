using System.Collections.Generic;
using System.Linq;

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

        public LinkedList<TwinVertex> Vertices { get; private set; }

        private readonly Vertex _startVertex;
        private readonly Vertex _endVertex;

        public TwinGraph()
        {
            _startVertex = new Vertex();
            _endVertex = new Vertex();
            Vertices = new LinkedList<TwinVertex>();
            Arcs = new LinkedList<Arc>();
        }

        public TwinGraph(Graph graph) : this()
        {
            foreach (var vertex in graph.vertices)
            {
                AddTwinVertex(new TwinVertex(vertex, this));
            }
            foreach (var arc in graph.arcs)
            {
                TwinVertex tv1 = Vertices.FirstOrDefault(tv => tv.Precursor.Guid == arc.Start.Guid);
                TwinVertex tv2 = Vertices.FirstOrDefault(tv => tv.Precursor.Guid == arc.End.Guid);
                AddArc(tv1, tv2, arc.IsInMatching);
            }
        }

        public void AddTwinVertex(TwinVertex tv)
        {
            Vertices.AddLast(tv);
        }

        public void AddArc(TwinVertex startVertex, TwinVertex endVertex, bool inMatching)
        {
            if (inMatching)
            {
                AddArc(startVertex.A, endVertex.B, inMatching);
            }
            else
            {
                AddArc(startVertex.B, endVertex.A, inMatching);
            }
        }

        public void AddEdge(TwinVertex startVertex, TwinVertex endVertex, bool inMatching)
        {
            if (inMatching)
            {
                AddArc(startVertex.A, endVertex.B, inMatching);
                AddArc(endVertex.A, startVertex.B, inMatching);
            }
            else
            {
                AddArc(startVertex.B, endVertex.A, inMatching);
                AddArc(endVertex.B, startVertex.A, inMatching);  
            }
        }

        private void AddArc(Vertex startVertex, Vertex endVertex, bool inMatching)
        {
            Arc a = startVertex.AddOutboundArc(endVertex);;
            if (inMatching)
            {
                a.AddToMatching();
            }
            Arcs.AddLast(a);
        }

        public LinkedList<Arc> Arcs { get; private set; }
    }
}