namespace GraphFramework
{
    public class MDFS
    {
        private readonly TwinGraph _tg;
        private readonly VertexStack _k;

        public MDFS(TwinGraph tg, VertexStack k)
        {
            _tg = tg;
            _k = k;
        }

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
                var top = _k.Top();
                top.Pushed();
                foreach (var arc in top.OutboundArcs)
                {
                    var v = (ABVertex) arc.End;
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
                                if (v.IsPushed)
                                {
                                    while (v.Lset != null)
                                    {
                                        ABVertex nextVertex = v.Lset;
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
}