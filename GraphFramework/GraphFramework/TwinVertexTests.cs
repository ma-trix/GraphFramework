﻿using NUnit.Framework;
using System.Linq;

namespace GraphFramework
{
    [TestFixture]
    public class TwinVertexTests
    {
        [Test]
        public void KnowsItsPrecursor()
        {
            Vertex v = new Vertex();
            TwinVertex tv = new TwinVertex(v, null);
            Assert.AreSame(v, tv.Precursor);
        }

        [Test]
        public void KnowsItsTwinGraph()
        {
            TwinGraph tg = new TwinGraph();
            TwinVertex tv = new TwinVertex(null, tg);
            Assert.AreSame(tg, tv.Graph);
        }

        [Test]
        public void CreatesAandBVertices()
        {
            Vertex v = new Vertex();
            TwinVertex tv = new TwinVertex(v, null);
            Assert.NotNull(tv.A);
            Assert.NotNull(tv.B);
        }

        [Test]
        public void AddsNotMatchingEdgeToNeighbour()
        {
            TwinVertex tv1 = new TwinVertex(null, null);
            TwinVertex tv2 = new TwinVertex(null, null);
            tv1.AddNonMatchingEdge(tv2);
            Assert.That(tv1.B.OutboundArcs.Count(a => a.End == tv2.A), Is.EqualTo(1));
            Assert.That(tv2.B.OutboundArcs.Count(a => a.End == tv1.A), Is.EqualTo(1));
        }

        [Test]
        public void AddsMatchingEdgeToNeighbour()
        {
            TwinVertex tv1 = new TwinVertex(null, null);
            TwinVertex tv2 = new TwinVertex(null, null);
            tv1.AddMatchingEdge(tv2);
            Assert.That(tv1.A.OutboundArcs.Count(a => a.End == tv2.B), Is.EqualTo(1));
            Assert.That(tv2.A.OutboundArcs.Count(a => a.End == tv1.B), Is.EqualTo(1));
        }
    }
}