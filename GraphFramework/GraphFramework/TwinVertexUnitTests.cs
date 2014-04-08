using System;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class TwinVertexUnitTests
    {
        public class TheConstructor : TwinVertexUnitTests
        {
            private Vertex _v;
            private TwinVertex _tv;
            private TwinGraph _tg;

            [SetUp]
            public void DerivedInit()
            {
                _v = new Vertex();
                _tg = new TwinGraph();
                _tv = new TwinVertex(_v, _tg);
            }

            [Test]
            public void SetsPrecursorOnCreate()
            {
                Assert.That(_tv.Precursor, Is.SameAs(_v));
            }

            [Test]
            public void SetsTwinGraphOnCreate()
            {
                Assert.That(_tv.Graph, Is.SameAs(_tg));
            }

            [Test]
            public void CreatesAVertexOnCreate()
            {   
                Assert.That(_tv.A, Is.Not.Null);
            }

            [Test]
            public void CreatesBVertexOnCreate()
            {
                Assert.That(_tv.B, Is.Not.Null);
            }

            [Test]
            public void SetsCorrectTypeToAVertexOnCreate()
            {
                Assert.That(_tv.A.Type, Is.EqualTo(VertexType.A));
            }

            [Test]
            public void SetsCorrectTypeToBVertexOnCreate()
            {
                Assert.That(_tv.B.Type, Is.EqualTo(VertexType.B));
            }

            [Test]
            public void SetsVertexBAsTwinOfVertexAOnCreate()
            {
                Assert.That(_tv.A.Twin, Is.EqualTo(_tv.B));
            }
            
            [Test]
            public void SetsVertexAAsTwinOfVertexBOnCreate()
            {
                Assert.That(_tv.A.Twin, Is.EqualTo(_tv.B));
            }
        }

        public class TheAddEdgeMethod : TwinVertexUnitTests
        {
            private TwinVertex _tv1;
            private TwinVertex _tv2;
            private Tuple<Arc, Arc> _e;

            [SetUp]
            public void DerivedInit()
            {
                _tv1 = new TwinVertex(null, null);
                _tv2 = new TwinVertex(null, null);
            }
            
            [Test]
            public void AddsNotMatchingEdgeToNeighbour()
            {
                _e = _tv1.AddEdge(_tv2, false);
                Assert.That(_tv1.B.OutboundArcs, Has.Member(_e.Item1));
                Assert.That(_tv2.B.OutboundArcs, Has.Member(_e.Item2));
            }

            [Test]
            public void AddsMatchingEdgeToNeighbour()
            {
                _e = _tv1.AddEdge(_tv2, true);
                Assert.That(_tv1.A.OutboundArcs, Has.Member(_e.Item1));
                Assert.That(_tv2.A.OutboundArcs, Has.Member(_e.Item2));
            }
        }

        public class TheNameProperty : TwinVertexUnitTests
        {
            [Test]
            public void TwinVertexHasNameOfPrecursorWithTVAppended()
            {
                const string name = "badaboom";
                var v = new Vertex(name);
                var tv = new TwinVertex(v, null);
                Assert.That(tv.Name, Is.EqualTo(name + ".TV"));
            }
        }
    }
}