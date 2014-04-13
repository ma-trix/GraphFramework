using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertexStack : IVertexStack
    {
        public readonly LinkedList<IStackableVertex> CurrentStack = new LinkedList<IStackableVertex>();
        private IStackableVertex _top;

        public IStackableVertex Push(ABVertex vertex, Arc arcFromAncestor)
        {
            vertex.Pushed(_top, arcFromAncestor);
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