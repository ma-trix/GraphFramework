namespace GraphFramework
{
    public class StackVertex
    {
        public StackVertex(ABVertex value, ABVertex ancestor)
        {
            Value = value;
            Ancestor = ancestor;
        }

        public ABVertex Value { get; private set; }
        public ABVertex Ancestor { get; private set; }
    }
}