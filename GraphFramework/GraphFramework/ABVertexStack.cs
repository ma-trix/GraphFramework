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

        public StackVertex Top()
        {
            return _top;
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