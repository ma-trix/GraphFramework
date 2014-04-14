using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;

namespace GraphFramework
{
    public class MDFS
    {
        public static int _step = 0;
        private readonly TwinGraph _tg;
        private readonly IVertexStack _k;
        private readonly LinkedList<ABVertex> _l;
        private IStackableVertex _start;
        private LinkedList<Arc> _mAugmentingPath; 
        
        private static readonly ILog Log = LogManager.GetLogger
    (MethodBase.GetCurrentMethod().DeclaringType);

        public MDFS(TwinGraph tg, IVertexStack k, LinkedList<ABVertex> l)
        {
            _tg = tg;
            _k = k;
            _l = l;
        }

        public LinkedList<Arc> Run()
        {
            _start = _k.Push(_tg.StartVertex, null);
            Search();
            return _mAugmentingPath;
        }

        private void Search()
        {
            if (_k.Top().Value == _tg.EndVertex)
            {
                _mAugmentingPath = Reconstruct(_k.Top(), _start, new LinkedList<Arc>());
                StringBuilder sb = new StringBuilder();
                foreach (var arc in _mAugmentingPath)
                {
                    sb.Append(arc);
                    sb.Append(", ");
                }
                Log.Info(sb.ToString());
                Log.Info("ALGOTITHM HAS FOUND M-AUGMENTING PATH");
            }
            else
            {
                var stackTop = _k.Top();
                var top = stackTop.Value;
                top.Pushed();
                foreach (var arc in top.OutboundArcs)
                {
                    if (_mAugmentingPath != null)
                    {
                        return;
                    }
                    var w = (ABVertex) arc.End;
                    if (w.Type == VertexType.B)
                    {
                        Log.Info("Case 1, vertex " + w.Name + " and arc " + arc);
                        _k.Push(w, arc);
                        Search();
                    }
                    else
                    {
                        if (_k.Contains(w))
                        {
                            Log.Info("Case 2.1, vertex " + w.Name + " and arc " + arc);
                            w.AddToE(new Connection(arc, stackTop, w));
                        }
                        else
                        {
                            if (_k.Contains(w.Twin))
                            {
                                if (w.IsPushed)
                                {
                                    Log.Info("Case 2.2.1, vertex " + w.Name + " and arc " + arc);
                                    w.AddToE(new Connection(arc, stackTop, w));
                                }
                                else
                                {
                                    Log.Info("Case 2.2.2, vertex " + w.Name + " and arc " + arc);
                                    w.AddToR(new Connection(arc, stackTop, w));
                                }
                            }
                            else
                            {
                                if (w.IsPushed)
                                {
                                    if (w.L != null)
                                    {
                                        Log.Info("Case 2.3.1, vertex " + w.Name + " and arc " + arc);
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
                                    Log.Info("Case 2.3.2, vertex " + w.Name + " and arc " + arc);
                                    _k.Push(w, arc);
                                    Search();
                                }
                            }
                        }
                    }
                }
                if (_mAugmentingPath != null)
                {
                    return;
                }
                if (top.Type == VertexType.B && !top.Twin.IsPushed)
                {
                    Log.Info(" vertex " + top.Name + " ,twin is pushed: " + top.Twin.IsPushed);
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
                //_k.Pop();
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
                        Log.Info("Vertex " + zB.Value.Name + " gets P " + pcur.Arc);
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
                    var q = ReconstructQ(nodeCur, eA.End, new LinkedList<Arc>());
                    path.PrependRange(q);
                    nodeCur = eA.Start;
                }
            }
            if (nodeCur.ArcFromAncestor != null)
            {
                path.AddFirst(nodeCur.ArcFromAncestor);
            }
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
            if (uA.ArcFromAncestor == null)
            {
                var arc = ArcHelper.FindArc(st.Value.P.Start.Value, uA.Value, uA.Value.InboundArcs);
                pathQ.AddLast(arc);
            }
            else
            {
                pathQ.AddLast(uA.ArcFromAncestor);
            }
            return path;
        }
    }
}