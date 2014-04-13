using System.Collections.Generic;

namespace GraphFramework
{
    public class MDFS
    {
        private readonly TwinGraph _tg;
        private readonly IVertexStack _k;
        private readonly LinkedList<ABVertex> _l;
        private IStackableVertex _start;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MDFS(TwinGraph tg, IVertexStack k, LinkedList<ABVertex> l)
        {
            _tg = tg;
            _k = k;
            _l = l;
        }

        public void Run()
        {
            _start = _k.Push(_tg.StartVertex, null);
            Search();
        }

        private void Search()
        {
            if (_k.Top().Value == _tg.EndVertex)
            {
                var augmentingPath = new LinkedList<Arc>();
                Reconstruct(_k.Top(), _start, augmentingPath);
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
                        _k.Push(w, arc);
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
                                        top.Expand(new ExpandedArc(arc));
                                        _k.Push(w.L, null);
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
                                    _k.Push(w, arc);
                                    Search();
                                }
                            }
                        }
                    }
                }
                if (top.Type == VertexType.B && !top.Twin.IsPushed)
                {
                    var lcur = top.Twin;
                    lcur.EmptyD();
                    var ldef = new LinkedList<ABVertex>();
                    foreach (var a in lcur.R)
                    {
                        ConstrL(a, top, lcur, ldef);
                    }
                    while (ldef.Count > 0)
                    {
                        var v = ldef.Last.Value;
                        ldef.Remove(v);
                        foreach (var x in v.E)
                        {
                            ConstrL(x, top, lcur, ldef);
                        }
                    }
                }
                _k.Pop();
                _k.Pop();
            }
        }

        private void ConstrL(Connection pcur , ABVertex xB, ABVertex lcur, LinkedList<ABVertex> ldef)
        {
            var qB = pcur.Start;
            var zB = qB;
            
            while (zB.Value != xB)
            {
                if (zB.Value.Type == VertexType.A)
                {
                    if (!_l.Contains(zB.Value))
                    {
                        lcur.AddToD(zB.Value);
                        zB.Value.L = lcur;
                        AddToL(zB.Value);
                        zB.Value.P = pcur;
                        ldef.AddLast(zB.Value);
                    }
                    else
                    {
                        var rB = FindCurrentDContaining(zB.Value);
                        lcur.AddAnotherDtoD(rB.Value.D);
                        zB = rB;
                        continue;
                    }
                }
                zB = zB.Ancestor;
            }
        }

        private void AddToL(ABVertex v)
        {
            _l.AddLast(v);
            v.AddedToL();
        }

        private IStackableVertex FindCurrentDContaining(ABVertex v)
        {
            return v.L;
        }

        private LinkedList<Arc> Reconstruct(IStackableVertex top, IStackableVertex start, LinkedList<Arc> path)
        {
            var nodeCur = top;
            while (nodeCur != start)
            {
                if (!nodeCur.Ancestor.IsExpanded)
                {
                    path.AddFirst(nodeCur.ArcFromAncestor);
                    nodeCur = nodeCur.Ancestor;
                }
                else
                {
                    var eA = nodeCur.Ancestor.ExpandedArc;
                    var q = ReconstructQ(nodeCur, eA.End, path);
                    path.PrependRange(q);
                    nodeCur = eA.Start;
                }
            }
            path.AddFirst(nodeCur.ArcFromAncestor);
            return path;
        }

        private LinkedList<Arc> ReconstructQ(IStackableVertex uA, IStackableVertex wA, LinkedList<Arc> path)
        {
            var st = wA;
            var p1St = st.Value.P.Start;
            var p2St = st.Value.P.End;
            var pathQ = Reconstruct(p1St, st, path);
            while (p2St != uA)
            {
                st = st.Value.P.End;
                var block = Reconstruct(st.Value.P.Start, st, path);
                pathQ.AppendRange(block);
            }
            pathQ.AddLast(uA.ArcFromAncestor);
            return path;
        }
    }
}