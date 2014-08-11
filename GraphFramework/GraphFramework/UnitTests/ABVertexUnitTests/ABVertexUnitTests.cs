using NUnit.Framework;

namespace GraphFramework.UnitTests.ABVertexUnitTests
{
    public abstract class ABVertexUnitTests
    {
        private ABVertex _abv;
        private TwinGraph _tg;

        [SetUp]
        public void Init()
        {
            _tg = new TwinGraph();
            _abv = new ABVertex(VertexType.A, _tg);
        }

        [TestFixture]
        public class TheConstructor1 : ABVertexUnitTests
        {
            [Test]
            public void NewTypedVertexIsNotPushed()
            {
                Assert.That(_abv.IsPushed, Is.False);
            }

            [Test]
            public void NewVertexHasEmptyE()
            {
                Assert.That(_abv.E, Is.Empty);
            }
            
            [Test]
            public void NewVertexHasEmptyR()
            {
                Assert.That(_abv.R, Is.Empty);
            }

            [Test]
            public void NewVertexHasEmptyD()
            {
                Assert.That(_abv.D, Is.Empty);
            }
        }

        [TestFixture]
        public class TheConstructor2 : ABVertexUnitTests
        {
            private ABVertex _abvA;
            private ABVertex _abvB;
            private const string Name = "bazinga";

            [Test]
            public void InitializesABVertexTypeAWithName()
            {
                _abvA = new ABVertex(VertexType.A, Name, null);
                Assert.That(_abvA.Name, Is.EqualTo(Name + ".A"));
            }

            [Test]
            public void InitializesABVertexTypeBWithName()
            {
                _abvB = new ABVertex(VertexType.B, Name, null);
                Assert.That(_abvB.Name, Is.EqualTo(Name + ".B"));
            }
        }

        [TestFixture]
        public class ThePushedMethod : ABVertexUnitTests
        {
            [Test]
            public void PushingABVertexMarksItAsPushed()
            {
                _abv.Pushed();
                Assert.That(_abv.IsPushed, Is.True);
            }
        }

        [TestFixture]
        public class TheAddToDerMethods : ABVertexUnitTests
        {
            private ABVertex _v;
            private Arc _arc;
            private Connection _connection;

            [SetUp]
            public void DerivedInit()
            {
                _v = new ABVertex(VertexType.B, _tg);
                _arc = new Arc(null, _v, _abv);
                _connection = new Connection(_arc, null, null);
            }

            [Test]
            public void AddsToE()
            {
                _abv.AddToE(_connection);
                Assert.That(_abv.E, Has.Member(_connection));
            }

            [Test]
            public void AddsToR()
            {
                _abv.AddToR(_connection);
                Assert.That(_abv.R, Has.Member(_connection));
            }

            [Test]
            public void AddsToD()
            {
                _abv.AddToD(_v);
                Assert.That(_abv.D, Has.Member(_v));
            }
        }

        [TestFixture]
        public class TheOtherMethods : ABVertexUnitTests
        {
            private ABVertex _v;

            [SetUp]
            public void DerivedInit()
            {
                _abv = new ABVertex(VertexType.A, _tg);
                _v = new ABVertex(VertexType.B, _tg);
            }

            [Test]
            public void EmptiesD()
            {
                _abv.AddToD(_v);
                _abv.EmptyD();
                Assert.That(_abv.D, Is.Empty);
            }

            [Test]
            public void AddsAnotherDtoD()
            {
                _v.AddToD(_abv);
                _v.AddToD(_v);
                _abv.AddAnotherDtoD(_v.D);
                Assert.That(_abv.D, Has.Member(_abv));
                Assert.That(_abv.D, Has.Member(_v));
            }
            
            [Test]
            public void TwinsHaveNameOfPrecursorWithTypeAppended()
            {
                const string name = "badaboom";
                var v = new Vertex(name);
                var tv = new TwinVertex(v, null);
                Assert.That(tv.A.Name, Is.EqualTo(name + ".A"));
                Assert.That(tv.B.Name, Is.EqualTo(name + ".B"));
            }
            
            [Test]
            public void NewVertexIsNotExpanded()
            {
                Assert.That(_v.IsExpanded, Is.False);
            }

            [Test]
            public void ExpandsVertex()
            {
                var a = new ExpandedArc();
                _v.Expand(a);
                Assert.That(_v.IsExpanded, Is.True);
            }

            [Test]
            public void SavesExpandedArc()
            {
                var a = new ExpandedArc();
                _v.Expand(a);
                Assert.That(_v.ExpandedArc, Is.SameAs(a));
            }
        }

        [TestFixture]
        public class TheIStackVertexUnitInterfaceMethods : ABVertexUnitTests
        {
            [Test]
            public void NewStackVertexHasNoDescendants()
            {
                Assert.That(_abv.Descendants, Is.Empty);
            }

            [Test]
            public void ValueSelfReferences()
            {
                Assert.That(_abv.Value, Is.SameAs(_abv));
            }

            [Test]
            public void NewABVertexHasNoAncestor()
            {
                Assert.That(_abv.Ancestor, Is.Null);
            }

            [Test]
            public void AddsDescendant()
            {
                IStackableVertex v = new ABVertex(VertexType.A, _tg);
                _abv.AddDescendant(v);
                Assert.That(_abv.Descendants, Has.Member(v));
            }
        }

        [TestFixture]
        public class TheArcRevertedMethod : ABVertexUnitTests
        {
            [Test]
            public void RevertingArcRevertsArcInTwin()
            {
                var v1A = new ABVertex(VertexType.A, "1", _tg);
                var v1B = new ABVertex(VertexType.B, "1", _tg);
                var v2A = new ABVertex(VertexType.A, "2", _tg);
                var v2B = new ABVertex(VertexType.B, "2", _tg);

                v1A.SetTwin(v1B);
                v1B.SetTwin(v1A);
                v2A.SetTwin(v2B);
                v2B.SetTwin(v2A);

                var a1 = v1A.AddOutboundArc(v2B, false);
                var a2 = v2A.AddOutboundArc(v1B, false);

                a1.RevertFromTwin(); 
                Assert.That(a1.Start, Is.SameAs(v2B));
                Assert.That(a1.End, Is.SameAs(v1A));
                Assert.That(a1.IsInMatching, Is.True);
                Assert.That(a2.Start, Is.SameAs(v1B));
                Assert.That(a2.End, Is.SameAs(v2A));
                Assert.That(a2.IsInMatching, Is.True);
            }
        }
    }
}