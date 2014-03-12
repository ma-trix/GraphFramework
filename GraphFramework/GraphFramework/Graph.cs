using System.Collections.Generic;

namespace GraphFramework
{
    public class Graph
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LinkedList<Vertex> vertices = new LinkedList<Vertex>();
        public LinkedList<Arc> arcs = new LinkedList<Arc>();

        public void AddVertex(Vertex vertex)
        {
            vertices.AddLast(vertex);
            Log.Info("Added vertex " + vertex.Name);
            vertex.Graph = this;
        }

        public void RemoveVertex(Vertex vertexToRemove)
        {
            if (!vertices.Contains(vertexToRemove))
            {
                throw new NoVertexException();
            }
            vertexToRemove.RemoveInboundArcs();
            vertexToRemove.RemoveOutboundArcs();
            vertexToRemove.Graph = null;
            vertices.Remove(vertexToRemove);
        }

        public void AddArc(Vertex startVertex, Vertex endVertex)
        {
            Arc a = startVertex.AddOutboundArc(endVertex);
            Log.Info("Added arc " + a + " to graph");
            arcs.AddLast(a);
        }

        public void RemoveArc(Vertex v1, Vertex v2)
        {
            if (ArcHelper.DeleteArc(v1, v2, arcs))
            {
                Log.Info("Removed arc " + v1.Name + " -> " + v2.Name + " from graph");
                v1.RemoveArc(v2);
            }
            else
            {
                throw new NoArcException();
            }
        }

        public void AddEdge(Vertex startVertex, Vertex endVertex)
        {
            Log.Info("Adding edge " + startVertex.Name + " <-> " + endVertex.Name);
            AddArc(startVertex, endVertex);
            AddArc(endVertex, startVertex);
        }

        public void RemoveEdge(Vertex v1, Vertex v2)
        {
            if (!ArcHelper.DoesArcExist(v1, v2, arcs))
                throw new NoArcException();
            if (!ArcHelper.DoesArcExist(v2, v1, arcs))
                throw new NoArcException();
            RemoveArc(v1, v2);
            RemoveArc(v2, v1);
            
        }
    }
}
