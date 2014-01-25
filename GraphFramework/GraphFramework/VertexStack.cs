namespace GraphFramework
{
    public class VertexStack : IVertexStack
    {
        private StackVertex _top = new StackVertex(null, null);

        public StackVertex Push(ABVertex vertex)
        {
            var pushedVertex = new StackVertex(vertex, _top.Value);
            _top.AddDescendant(vertex);
            _top = pushedVertex;
            return pushedVertex;
        }

        public ABVertex Top()
        {
            return _top.Value;
        }

        public bool Contains(Vertex vertex)
        {
            throw new System.NotImplementedException();
        }

        public void Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}