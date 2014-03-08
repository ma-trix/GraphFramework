using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class MDFS
    {
        private readonly TwinGraph _tg;
        private readonly IVertexStack _k;
        private readonly LinkedList<ABVertex> L; 

        public MDFS(TwinGraph tg, IVertexStack k, LinkedList<ABVertex> L)
        {
            _tg = tg;
            _k = k;
            this.L = L;
        }

        public void Run()
        {
            _k.Push(_tg.StartVertex);
            Search();
        }

        private void Search()
        {
            if (_k.Top().Value == _tg.EndVertex)
            {
                Reconstruct();
            }
            else
            {
                var stackTop = _k.Top();
                var top = stackTop.Value;
                top.Pushed();
                foreach (var arc in top.OutboundArcs)
                {
                    var w = (ABVertex) arc.End;
                    if (w.Type == VertexType.B)
                    {
                        _k.Push(w);
                        Search();
                    }
                    else
                    {
                        if (_k.Contains(w))
                        {
                            w.AddToE(new Tuple<Arc, StackVertex>(arc, stackTop));
                        }
                        else
                        {
                            if (_k.Contains(w.Twin))
                            {
                                if (w.IsPushed)
                                {
                                    w.AddToE(new Tuple<Arc, StackVertex>(arc, stackTop));
                                }
                                else
                                {
                                    w.AddToR(new Tuple<Arc, StackVertex>(arc, stackTop));
                                }
                            }
                            else
                            {
                                if (w.IsPushed)
                                {
                                    if (w.L != null)
                                    {
                                        top.Expand(arc);
                                        _k.Push(w.L);
                                        w.L = null;
                                        Search();
                                    }
                                    else
                                    {
                                        if (!w.IsInL())
                                        {
                                            w.AddToE(new Tuple<Arc, StackVertex>(arc, stackTop));
                                        }
                                    }
                                }
                                else
                                {
                                    _k.Push(w);
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
                        ConstrL(a, top, Lcur, Ldef);
                    }
                    while (Ldef.Count > 0)
                    {
                        var v = Ldef.Last.Value;
                        Ldef.Remove(v);
                        foreach (var x in v.E)
                        {
                            ConstrL(x, top, Lcur, Ldef);
                        }
                    }
                }
                _k.Pop();
                _k.Pop();
            }
        }

        private void ConstrL(Tuple<Arc, StackVertex> connection , ABVertex xB, ABVertex Lcur, LinkedList<ABVertex> Ldef)
        {
            var qB = connection.Item2;
            var uA = connection.Item1.End;
            var zB = qB;
            var n = qB.Ancestor;

            while (n.Value != xB)
            {
                if (n.Value.Type == VertexType.A)
                {
                    if (!L.Contains(n.Value))
                    {
                        Lcur.AddToD(n.Value);
                        AddToL(n.Value);
                        n.Value.P = connection;
                        Ldef.AddLast(n.Value);
                    }
                    else
                    {
                        var rB = FindCurrentDContaining(n.Value);
                        Lcur.AddAnotherDToD(rB.Value.D);
                        zB = rB;
                    }
                }
                n = n.Ancestor;
            }
        }

        private void AddToL(ABVertex v)
        {
            L.AddLast(v);
            v.AddedToL();
        }

        private StackVertex FindCurrentDContaining(ABVertex stackVertex)
        {
            throw new NotImplementedException();
        }

        private void Reconstruct()
        {
            
        }
    }
}