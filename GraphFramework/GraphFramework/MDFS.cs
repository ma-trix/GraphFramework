using System.Collections.Generic;

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
                        if (_k.Contains(v))
                        {
                            v.AddToE(top);
                        }
                        else
                        {
                            if (_k.Contains(v.Twin))
                            {
                                if (v.IsPushed)
                                {
                                    v.AddToE(top);
                                }
                                else
                                {
                                    v.AddToR(top);
                                }
                            }
                            else
                            {
                                if (v.IsPushed)
                                {
                                    if (v.L != null)
                                    {
                                        top.Expand(arc);
                                        _k.Push(v.L);
                                        v.L = null;
                                        Search();
                                    }
                                    else
                                    {
                                        if (!v.isInL())
                                        {
                                            v.AddToE(top);
                                        }
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
                if (top.Type == VertexType.B && !top.Twin.IsPushed)
                {
                    var Lcur = top.Twin;
                    Lcur.EmptyD();
                    var Ldef = new LinkedList<ABVertex>();
                    foreach (var v in Lcur.R)
                    {
                        ConstrL(new Arc(null, v, Lcur), top);
                    }
                    while (Ldef.Count > 0)
                    {
                        var v = Ldef.Last.Value;
                        Ldef.Remove(v);
                        foreach (var x in v.E)
                        {
                            ConstrL(new Arc(null, x, v), top);
                        }
                    }
                }
                _k.Pop();
                _k.Pop();
            }
        }

        private void ConstrL(Arc arc, ABVertex top)
        {
            
        }

        private void Reconstruct()
        {
            
        }
    }
}