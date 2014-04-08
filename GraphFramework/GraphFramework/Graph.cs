using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class Graph
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LinkedList<Vertex> Vertices = new LinkedList<Vertex>();
        public LinkedList<Arc> Arcs = new LinkedList<Arc>();

        public void AddVertex(Vertex vertex)
        {
            Vertices.AddLast(vertex);
            Log.Info("Added vertex " + vertex.Name);
            vertex.Graph = this;
        }

        public void RemoveVertex(Vertex vertexToRemove)
        {
            if (!Vertices.Contains(vertexToRemove))
            {
                throw new NoVertexException();
            }
            vertexToRemove.RemoveInboundArcs();
            vertexToRemove.RemoveOutboundArcs();
            vertexToRemove.Graph = null;
            Vertices.Remove(vertexToRemove);
        }

        public Arc AddArc(Vertex startVertex, Vertex endVertex, bool inMatching)
        {
            Arc a = startVertex.AddOutboundArc(endVertex, inMatching);
            Log.Info("Added arc " + a + " to graph");
            Arcs.AddLast(a);
            return a;
        }

        public void RemoveArc(Vertex v1, Vertex v2)
        {
            if (ArcHelper.DeleteArc(v1, v2, Arcs))
            {
                Log.Info("Removed arc " + v1.Name + " -> " + v2.Name + " from graph");
                v1.RemoveArc(v2);
            }
            else
            {
                throw new NoArcException();
            }
        }

        public Tuple<Arc, Arc> AddEdge(Vertex startVertex, Vertex endVertex, bool inMatching)
        {
            Log.Info("Adding edge " + startVertex.Name + " <-> " + endVertex.Name);
            var there = AddArc(startVertex, endVertex, inMatching);
            var back = AddArc(endVertex, startVertex, inMatching);
            return new Tuple<Arc, Arc>(there, back);
        }

        public void RemoveEdge(Vertex v1, Vertex v2)
        {
            if (!ArcHelper.DoesArcExist(v1, v2, Arcs))
                throw new NoArcException();
            if (!ArcHelper.DoesArcExist(v2, v1, Arcs))
                throw new NoArcException();
            RemoveArc(v1, v2);
            RemoveArc(v2, v1);
            
        }
    }
}
