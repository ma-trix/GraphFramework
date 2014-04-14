using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertexStack : IVertexStack
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            MDFS._step++;
            Log.Info("(" + MDFS._step + ") Pushed vertex " + vertex.Name + " and arc " + arcFromAncestor);
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
            MDFS._step++;
            Log.Info("(" + MDFS._step + ") Popped vertex " + ((ABVertex)_top).Name);
            _top = _top.Ancestor;
            if (_top != null)
            {
                Log.Info("New top vertex " + ((ABVertex)_top).Name);    
            }
        }
    }
}