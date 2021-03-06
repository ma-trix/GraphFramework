﻿using System;
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
            _startVertex = new ABVertex(VertexType.A, "s", this);
            _endVertex = new ABVertex(VertexType.B, "t", this);
            Vertices = new LinkedList<TwinVertex>();
            Arcs = new LinkedList<Arc>();
        }

        public TwinGraph(Graph graph) : this()
        {
            Log.Info("Generating TwinGraph from Graph");
            Log.Info("Generating twin vertices from vertices");
            foreach (var vertex in graph.Vertices)
            {
                AddTwinVertex(new TwinVertex(vertex, this));
            }
            Log.Info("Generating arcs for twin vertices from arcs");
            foreach (var arc in graph.Arcs)
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
            if (!tv.B.IsInMatching && !tv.A.IsInMatching)
            {
                AddArc(StartVertex, tv.B, false);
                AddArc(tv.A, EndVertex, false);
            }
        }

        public Arc AddArc(TwinVertex startVertex, TwinVertex endVertex, bool inMatching)
        {
            ABVertex start;
            ABVertex end;
            if (inMatching)
            {
                start = startVertex.A;
                end = endVertex.B;
                bool arcExisted = ArcHelper.DeleteArc(StartVertex, startVertex.B, Arcs);
                if (arcExisted)
                {
                    StartVertex.RemoveArc(startVertex.B);
                }
                arcExisted = ArcHelper.DeleteArc(StartVertex, endVertex.B, Arcs);
                if (arcExisted)
                {
                    StartVertex.RemoveArc(endVertex.B);
                }
                arcExisted = ArcHelper.DeleteArc(startVertex.A, EndVertex, Arcs);
                if (arcExisted)
                {
                    startVertex.A.RemoveArc(EndVertex);
                }
                arcExisted = ArcHelper.DeleteArc(endVertex.A, EndVertex, Arcs);
                if (arcExisted)
                {
                    endVertex.A.RemoveArc(EndVertex);
                }
            }
            else
            {
                start = startVertex.B;
                end = endVertex.A;
            }
            return AddArc(start, end, inMatching);

        }

        public Tuple<Arc,Arc> AddEdge(TwinVertex startVertex, TwinVertex endVertex, bool inMatching)
        {
            ABVertex start1;
            ABVertex start2;
            ABVertex end1;
            ABVertex end2;
            if (inMatching)
            {
                start1 = startVertex.A;
                end1 = endVertex.B;
                start2 = endVertex.A;
                end2 = startVertex.B;
            }
            else
            {
                start1 = startVertex.B;
                end1 = endVertex.A;
                start2 = endVertex.B;
                end2 = startVertex.A;
            }

            var a1 = AddArc(start1, end1, inMatching);
            var a2 = AddArc(start2, end2, inMatching);  
            return new Tuple<Arc, Arc>(a1, a2);
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
            Log.Info("Removing arc " + tvFrom + " -> " + tvTo + " M?: " + inMatching + " from TwinGraph");
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
            Log.Info("Removing edge " + tvFrom + " <-> " + tvTo + " M?: " + inMatching + " from TwinGraph");
            RemoveArc(tvFrom, tvTo, inMatching);
            RemoveArc(tvTo, tvFrom, inMatching);
        }

        public void SymmetricDifferenceWith(LinkedList<Arc> mAugmentingPath)
        {
            foreach (var arc in mAugmentingPath)
            {
                if (arc.Start != StartVertex && arc.End != EndVertex)
                {
                    arc.RevertFromTwin();
                }
            }
        }

        public void LogVertices()
        {
            foreach (var twinVertex in Vertices)
            {
                Log.Info(twinVertex.Name + " " + twinVertex.A.IsInMatching + " " + twinVertex.B.IsInMatching);
            }
        }

        public void LogArcs()
        {
            foreach (var arc in Arcs)
            {
                Log.Info(arc + " " + arc.IsInMatching);
            }
        }

        
        
        
        public TwinGraph(Graph graph, bool weighted)
            : this()
        {
            Log.Info("Generating TwinGraph from Graph");
            Log.Info("Generating twin vertices from vertices");
            foreach (var vertex in graph.Vertices)
            {
                AddTwinVertex(new TwinVertex(vertex, this));
            }
            Log.Info("Generating arcs for twin vertices from arcs");
            foreach (var arc in graph.Arcs)
            {
                TwinVertex tv1 = Vertices.FirstOrDefault(tv => tv.Precursor.Guid == arc.Start.Guid);
                TwinVertex tv2 = Vertices.FirstOrDefault(tv => tv.Precursor.Guid == arc.End.Guid);
                AddArcWeighted(tv1, tv2, arc.IsInMatching, arc.Weight);
            }
        }

        public Arc AddArcWeighted(TwinVertex startVertex, TwinVertex endVertex, bool inMatching, double weight)
        {
            ABVertex start;
            ABVertex end;
            if (inMatching)
            {
                start = startVertex.A;
                end = endVertex.B;
                bool arcExisted = ArcHelper.DeleteArc(StartVertex, startVertex.B, Arcs);
                if (arcExisted)
                {
                    StartVertex.RemoveArc(startVertex.B);
                }
                arcExisted = ArcHelper.DeleteArc(StartVertex, endVertex.B, Arcs);
                if (arcExisted)
                {
                    StartVertex.RemoveArc(endVertex.B);
                }
                arcExisted = ArcHelper.DeleteArc(startVertex.A, EndVertex, Arcs);
                if (arcExisted)
                {
                    startVertex.A.RemoveArc(EndVertex);
                }
                arcExisted = ArcHelper.DeleteArc(endVertex.A, EndVertex, Arcs);
                if (arcExisted)
                {
                    endVertex.A.RemoveArc(EndVertex);
                }
            }
            else
            {
                start = startVertex.B;
                end = endVertex.A;
            }
            return AddArcWeighted(start, end, inMatching, weight);

        }

        private Arc AddArcWeighted(Vertex startVertex, Vertex endVertex, bool inMatching, double weight)
        {
            Arc a = startVertex.AddOutboundArc(endVertex, inMatching, weight);
            Arcs.AddLast(a);
            Log.Info("Added arc " + a.Start.Name + " -> " + a.End.Name + " " + inMatching + " " + weight);
            return a;
        }


        public void LogVerticesWeighted()
        {
            foreach (var twinVertex in Vertices)
            {
                Log.Info(twinVertex.Name + " " + twinVertex.A.IsInMatching + " " + twinVertex.B.IsInMatching + " " + twinVertex.Precursor.DoubleWeight);
            }
        }

        public void LogArcsWeighted()
        {
            foreach (var arc in Arcs)
            {
                Log.Info(arc + ", inMatching: " + arc.IsInMatching + ", inGStar: " + arc.IsInGStar + ", weight: " + arc.Weight);
            }
        }
    }
}