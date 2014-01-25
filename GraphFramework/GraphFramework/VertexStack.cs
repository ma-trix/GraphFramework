namespace GraphFramework
{
    public class VertexStack : IVertexStack
    {
        private ABVertex _top;

        public StackVertex Push(ABVertex vertex)
        {
            var wrapped = new StackVertex(vertex, _top);
            _top = vertex;
            return wrapped;
        }

        public ABVertex Top()
        {
            return _top;
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