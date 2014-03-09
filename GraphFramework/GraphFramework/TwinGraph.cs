using System.Collections.Generic;
using System.Linq;

namespace GraphFramework
{
    public class TwinGraph
    {
        public TwinGraph()
        {
            _startVertex = new ABVertex(VertexType.A, "s");
            _endVertex = new ABVertex(VertexType.B, "t");
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

        public LinkedList<TwinVertex> Vertices { get; private set; }

        public ABVertex StartVertex { get { return _startVertex; } }
        public ABVertex EndVertex { get {return _endVertex; } }
        private readonly ABVertex _startVertex;
        private readonly ABVertex _endVertex;
        public LinkedList<Arc> Arcs { get; private set; }

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
       
        public void RemoveTwinVertex(TwinVertex tv)
        {
            if (!VertexHelper.DeleteTwinVertex(tv, Vertices))
                throw new NoVertexException();
        }

        public void RemoveArc(TwinVertex tvFrom, TwinVertex tvTo, bool inMatching)
        {
            if (inMatching)
            {
                if (ArcHelper.DeleteArc(tvFrom.A, tvTo.B, Arcs))
                {
                    tvFrom.A.RemoveArc(tvTo.B);
                }
                else
                {
                    throw new NoArcException();
                }
            }
            else
            {
                if (ArcHelper.DeleteArc(tvFrom.B, tvTo.A, Arcs))
                {
                    tvFrom.B.RemoveArc(tvTo.A);
                }
                else
                {
                    throw new NoArcException();
                }
            }
        }

        public void RemoveEdge(TwinVertex tvFrom, TwinVertex tvTo, bool inMatching)
        {
            if (inMatching)
            {
                if (!ArcHelper.DoesArcExist(tvFrom.A, tvTo.B, Arcs))
                    throw new NoArcException();
                if (!ArcHelper.DoesArcExist(tvTo.A, tvFrom.B, Arcs))
                    throw new NoArcException();
            }
            else
            {
                if (!ArcHelper.DoesArcExist(tvFrom.B, tvTo.A, Arcs))
                    throw new NoArcException();
                if (!ArcHelper.DoesArcExist(tvTo.B, tvFrom.A, Arcs))
                    throw new NoArcException();
            }
            RemoveArc(tvFrom, tvTo, inMatching);
            RemoveArc(tvTo, tvFrom, inMatching);
        }
    }
}