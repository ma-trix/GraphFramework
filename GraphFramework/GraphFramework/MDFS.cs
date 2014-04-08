using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class MDFS
    {
        private readonly TwinGraph _tg;
        private readonly IVertexStack _k;
        private readonly LinkedList<ABVertex> L;
        private IStackVertex _start;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MDFS(TwinGraph tg, IVertexStack k, LinkedList<ABVertex> L)
        {
            _tg = tg;
            _k = k;
            this.L = L;
        }

        public void Run()
        {
            _start = _k.Push(_tg.StartVertex);
            Search();
        }

        private void Search()
        {
            if (_k.Top().Value == _tg.EndVertex)
            {
                Reconstruct(_k.Top(), _start);
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
                            w.AddToE(new Connection(arc, stackTop, w));
                        }
                        else
                        {
                            if (_k.Contains(w.Twin))
                            {
                                if (w.IsPushed)
                                {
                                    w.AddToE(new Connection(arc, stackTop, w));
                                }
                                else
                                {
                                    w.AddToR(new Connection(arc, stackTop, w));
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
                                            w.AddToE(new Connection(arc, stackTop, w));
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

        private void ConstrL(Connection connection , ABVertex xB, ABVertex Lcur, LinkedList<ABVertex> Ldef)
        {
            var qB = connection.Start;
            var uA = connection.End;
            var zB = qB;
            
            while (zB.Value != xB)
            {
                if (zB.Value.Type == VertexType.A)
                {
                    if (!L.Contains(zB.Value))
                    {
                        Lcur.AddToD(zB.Value);
                        zB.Value.L = Lcur;
                        AddToL(zB.Value);
                        zB.Value.P = connection;
                        Ldef.AddLast(zB.Value);
                    }
                    else
                    {
                        var rB = FindCurrentDContaining(zB.Value);
                        Lcur.AddAnotherDToD(rB.Value.D);
                        zB = rB;
                        continue;
                    }
                }
                zB = zB.Ancestor;
            }
        }

        private void AddToL(ABVertex v)
        {
            L.AddLast(v);
            v.AddedToL();
        }

        private IStackVertex FindCurrentDContaining(ABVertex v)
        {
            return v.L;
        }

        private void Reconstruct(IStackVertex top, IStackVertex start)
        {
            var nodeCur = top;
            while (nodeCur != start)
            {
                if (!nodeCur.Ancestor.IsExpanded)
                {
                    nodeCur = nodeCur.Ancestor;
                }
                else
                {
                    var eA = nodeCur.Ancestor.ExpandedArc;
                    ReconstructQ(nodeCur, eA.End);
                    nodeCur = eA.Start;
                }
            }
        }

        private void ReconstructQ(IStackVertex uA, IStackVertex wA)
        {
            var st = wA;
            var p1st = st.Value.P.Start;
            var p2st = st.Value.P.End;
            Reconstruct(p1st, st);
            while (p2st != uA.Value)
            {
                st = st.Value.P.End;
                Reconstruct(st.Value.P.Start, st);
            }
        }
    }
}