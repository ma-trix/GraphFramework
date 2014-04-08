using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertexStack : IVertexStack
    {
        public readonly LinkedList<IStackableVertex> CurrentStack = new LinkedList<IStackableVertex>();
        private IStackableVertex _top = null;

        public IStackableVertex Push(ABVertex vertex)
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

        public IStackableVertex Top()
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