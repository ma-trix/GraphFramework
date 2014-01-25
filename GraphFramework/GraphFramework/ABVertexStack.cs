using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertexStack : IVertexStack
    {
        public readonly LinkedList<ABVertex> CurrentStack = new LinkedList<ABVertex>(); 
        private StackVertex _top = new StackVertex(null, null);

        public StackVertex Push(ABVertex vertex)
        {
            var pushedVertex = new StackVertex(vertex, _top);
            _top.AddDescendant(vertex);
            _top = pushedVertex;
            CurrentStack.AddLast(vertex);
            return pushedVertex;
        }

        public ABVertex Top()
        {
            return _top.Value;
        }

        public bool Contains(ABVertex vertex)
        {
            return VertexHelper.DoesABVertexExist(vertex.Guid, CurrentStack);
        }

        public void Pop()
        {
            CurrentStack.RemoveLast();
            _top = _top.Ancestor;
        }
    }
}