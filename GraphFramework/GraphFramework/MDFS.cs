using System.Collections.Generic;

namespace GraphFramework
{
    public class MDFS
    {
        private readonly TwinGraph _tg;
        private readonly IVertexStack _k;

        public MDFS(TwinGraph tg, IVertexStack k)
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
                            v.AddToE(arc);
                        }
                        else
                        {
                            if (_k.Contains(v.Twin))
                            {
                                if (v.IsPushed)
                                {
                                    v.AddToE(arc);
                                }
                                else
                                {
                                    v.AddToR(arc);
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
                                        if (!v.IsInL())
                                        {
                                            v.AddToE(arc);
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
                    foreach (var a in Lcur.R)
                    {
                        ConstrL(a, top);
                    }
                    while (Ldef.Count > 0)
                    {
                        var v = Ldef.Last.Value;
                        Ldef.Remove(v);
                        foreach (var x in v.E)
                        {
                            ConstrL(x, top);
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