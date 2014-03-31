using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertexStack : IVertexStack
    {
        public readonly LinkedList<IStackVertex> CurrentStack = new LinkedList<IStackVertex>();
        private IStackVertex _top = null;

        public IStackVertex Push(ABVertex vertex)
        {
            vertex.Pushed(_top);
            if (_top != null)
            {
                _top.AddDescendant(vertex);
            }
            _top = vertex;
            CurrentStack.AddLast(vertex);
            return vertex;
        }

        public IStackVertex Top()
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