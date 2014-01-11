namespace GraphFramework
{
    public class MDFS
    {
        private TwinGraph _tg;
        private VertexStack _k;

        public void Run()
        {
            _k.Push(_tg.StartVertex);
            Search();
        }

        private void Search()
        {
            if (_k.Top() == _tg.EndVertex)
            {
                Reconstruct();
            }
            else
            {
                Vertex top = _k.Top();
                top.Pushed();
                foreach (var arc in top.OutboundArcs)
                {
                    Vertex v = arc.End;
                    if (v.Type == VertexType.B)
                    {
                        _k.Push(v);
                        Search();
                    }
                    else
                    {
                        if(_k.Contains(v)){}
                        else
                        {
                            if (_k.Contains(v.Twin)){}
                            else
                            {
                                if (v.isPushed)
                                {
                                    while (v.Lset != null)
                                    {
                                        Vertex nextVertex = v.Lset;
                                        _k.Push(nextVertex);
                                        Search();
                                    }
                                }
                                else
                                {
                                    _k.Push(v);
                                    Search();
                                }
                            }
                        }
                    }
                }
                _k.Pop();
                _k.Pop();
            }
        }

        private void Reconstruct()
        {
            
        }
    }

    public enum VertexType
    {
        A,
        B
    }

    public interface VertexStack
    {
        void Push(Vertex vertex);
        Vertex Top();
        bool Contains(Vertex vertex);
        void Pop();
    }
}