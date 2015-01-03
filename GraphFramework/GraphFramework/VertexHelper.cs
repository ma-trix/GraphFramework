using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public static class VertexHelper
    {
        public static bool DoesVertexExist(Guid guid, LinkedList<Vertex> inList)
        {
            var vertex = inList.First;
            while (vertex != null)
            {
                var nextVertex = vertex.Next;
                if (vertex.Value.Guid == guid)
                {
                    return true;
                }
                vertex = nextVertex;
            }
            return false;
        }

        public static bool DoesABVertexExist(Guid guid, LinkedList<IStackableVertex> inList)
        {
            var vertex = inList.First;
            while (vertex != null)
            {
                var nextVertex = vertex.Next;
                if (((ABVertex) vertex.Value).Guid == guid)
                {
                    return true;
                }
                vertex = nextVertex;
            }
            return false;
        }

        public static bool DoesTwinVertexExist(Guid precursorGuid, LinkedList<TwinVertex> inList)
        {
            var twinVertex = inList.First;
            while (twinVertex != null)
            {
                var nextTwinVertex = twinVertex.Next;
                if (twinVertex.Value.Precursor.Guid == precursorGuid)
                {
                    return true;
                }
                twinVertex = nextTwinVertex;
            }
            return false;
        }
        
        public static TwinVertex FindTwinVertex(Guid precursorGuid, LinkedList<TwinVertex> inList)
        {
            var twinVertex = inList.First;
            while (twinVertex != null)
            {
                var nextTwinVertex = twinVertex.Next;
                if (twinVertex.Value.Precursor.Guid == precursorGuid)
                {
                    return twinVertex.Value;
                }
                twinVertex = nextTwinVertex;
            }
            return null;
        }

        public static bool DeleteTwinVertex(TwinVertex tv, LinkedList<TwinVertex> fromList)
        {
            var twinVertex = fromList.First;
            while (twinVertex != null)
            {
                var nextTwinVertex = twinVertex.Next;
                if (twinVertex.Value == tv)
                {
                    fromList.Remove(twinVertex);
                    return true;
                }
                twinVertex = nextTwinVertex;
            }
            return false;
        }
    }
}