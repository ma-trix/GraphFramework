using System.Collections.Generic;
using System.Linq;

namespace GraphFramework
{
    public class TwinGraph
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public TwinGraph()
        {
            _startVertex = new ABVertex(VertexType.A, "s");
            _endVertex = new ABVertex(VertexType.B, "t");
            Vertices = new LinkedList<TwinVertex>();
            Arcs = new LinkedList<Arc>();
        }

        public TwinGraph(Graph graph) : this()
        {
            Log.Info("Generating TwinGraph from Graph");
            Log.Info("Generating twin vertices from vertices");
            foreach (var vertex in graph.vertices)
            {
                AddTwinVertex(new TwinVertex(vertex, this));
            }
            Log.Info("Generating arcs for twin vertices from arcs");
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
            Log.Info("Adding TwinVertex " + tv.Name + " with precursor " + (tv.Precursor != null ? tv.Precursor.Name : "NULL"));
            Vertices.AddLast(tv);
        }

        public Arc AddArc(TwinVertex startVertex, TwinVertex endVertex, bool inMatching)
        {
            ABVertex start;
            ABVertex end;
            if (inMatching)
            {
                start = startVertex.A;
                end = endVertex.B;
            }
            else
            {
                start = startVertex.B;
                end = endVertex.A;
            }
            return AddArc(start, end, inMatching);

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

        private Arc AddArc(Vertex startVertex, Vertex endVertex, bool inMatching)
        {
            Arc a = startVertex.AddOutboundArc(endVertex, inMatching);
            Arcs.AddLast(a);
            Log.Info("Added arc " + a.Start.Name + " -> " + a.End.Name + " " + inMatching);
            return a;
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