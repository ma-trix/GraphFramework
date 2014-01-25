using System.Collections;
using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertex : Vertex
    {
        public ABVertex(VertexType type)
        {
            Type = type;
        }

        public ABVertex Twin { get; private set; }
        public bool IsPushed { get; private set; }
        public VertexType Type { get; private set; }
        public ABVertex L { get; set; }

        public LinkedList<ABVertex> R { get; set; }

        public void Pushed()
        {
            IsPushed = true;
        }

        public void SetTwin(ABVertex twin)
        {
            Twin = twin;
        }

        public void AddToE(ABVertex top)
        {
            throw new System.NotImplementedException();
        }

        public void AddToR(ABVertex top)
        {
            throw new System.NotImplementedException();
        }

        public void Expand(Arc arc)
        {
            throw new System.NotImplementedException();
        }

        public bool isInL()
        {
            throw new System.NotImplementedException();
        }

        public void EmptyD()
        {
            throw new System.NotImplementedException();
        }

        public LinkedList<ABVertex> E;
    }
}