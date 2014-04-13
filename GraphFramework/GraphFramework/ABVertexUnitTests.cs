using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ABVertexUnitTests
    {
        protected ABVertex Abv;

        public void Init()
        {
            Abv = new ABVertex(VertexType.A);
        }

        public class TheConstructor1 : ABVertexUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
            }
            
            [Test]
            public void NewTypedVertexIsNotPushed()
            {
                Assert.That(Abv.IsPushed, Is.False);
            }

            [Test]
            public void NewVertexHasEmptyE()
            {
                Assert.That(Abv.E, Is.Empty);
            }
            
            [Test]
            public void NewVertexHasEmptyR()
            {
                Assert.That(Abv.R, Is.Empty);
            }

            [Test]
            public void NewVertexHasEmptyD()
            {
                Assert.That(Abv.D, Is.Empty);
            }
        }

        public class TheConstructor2 : ABVertexUnitTests
        {
            private ABVertex _abvA;
            private ABVertex _abvB;
            private const string Name = "bazinga";

            [SetUp]
            public void DerivedInit()
            {
                Init();
            }

            [Test]
            public void InitializesABVertexTypeAWithName()
            {
                _abvA = new ABVertex(VertexType.A, Name);
                Assert.That(_abvA.Name, Is.EqualTo(Name + ".A"));
            }

            [Test]
            public void InitializesABVertexTypeBWithName()
            {
                _abvB = new ABVertex(VertexType.B, Name);
                Assert.That(_abvB.Name, Is.EqualTo(Name + ".B"));
            }
        }

        public class ThePushedMethod : ABVertexUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
            }

            [Test]
            public void PushingABVertexMarksItAsPushed()
            {
                Abv.Pushed();
                Assert.That(Abv.IsPushed, Is.True);
            }
        }

        public class TheAddToDerMethods : ABVertexUnitTests
        {
            private ABVertex _v;
            private Arc _arc;
            private Connection _connection;

            [SetUp]
            public void DerivedInit()
            {
                Init();
                _v = new ABVertex(VertexType.B);
                _arc = new Arc(null, _v, Abv);
                _connection = new Connection(_arc, null, null);
            }

            [Test]
            public void AddsToE()
            {
                Abv.AddToE(_connection);
                Assert.That(Abv.E, Has.Member(_connection));
            }

            [Test]
            public void AddsToR()
            {
                Abv.AddToR(_connection);
                Assert.That(Abv.R, Has.Member(_connection));
            }

            [Test]
            public void AddsToD()
            {
                Abv.AddToD(_v);
                Assert.That(Abv.D, Has.Member(_v));
            }
        }

        public class TheOtherMethods : ABVertexUnitTests
        {
            private ABVertex _v;

            [SetUp]
            public void DerivedInit()
            {
                Init();
                Abv = new ABVertex(VertexType.A);
                _v = new ABVertex(VertexType.B);
            }

            [Test]
            public void EmptiesD()
            {
                Abv.AddToD(_v);
                Abv.EmptyD();
                Assert.That(Abv.D, Is.Empty);
            }

            [Test]
            public void AddsAnotherDtoD()
            {
                _v.AddToD(Abv);
                _v.AddToD(_v);
                Abv.AddAnotherDtoD(_v.D);
                Assert.That(Abv.D, Has.Member(Abv));
                Assert.That(Abv.D, Has.Member(_v));
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

        public class TheIStackVertexUnitInterfaceMethods : ABVertexUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
            }

            [Test]
            public void NewStackVertexHasNoDescendants()
            {
                Assert.That(Abv.Descendants, Is.Empty);
            }

            [Test]
            public void ValueSelfReferences()
            {
                Assert.That(Abv.Value, Is.SameAs(Abv));
            }

            [Test]
            public void NewABVertexHasNoAncestor()
            {
                Assert.That(Abv.Ancestor, Is.Null);
            }

            [Test]
            public void AddsDescendant()
            {
                IStackableVertex v = new ABVertex(VertexType.A);
                Abv.AddDescendant(v);
                Assert.That(Abv.Descendants, Has.Member(v));
            }
        }

        public class TheArcRevertedMethod : ABVertexUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
            }

            [Test]
            public void RevertingArcRevertsArcInTwin()
            {
                var v1a = new ABVertex(VertexType.A, "1");
                var v1b = new ABVertex(VertexType.B, "1");
                var v2a = new ABVertex(VertexType.A, "2");
                var v2b = new ABVertex(VertexType.B, "2");

                v1a.SetTwin(v1b);
                v1b.SetTwin(v1a);
                v2a.SetTwin(v2b);
                v2b.SetTwin(v2a);

                var a1 = v1a.AddOutboundArc(v2b, false);
                var a2 = v2a.AddOutboundArc(v1b, false);

                a1.Revert(); 
                Assert.That(a1.Start, Is.SameAs(v2b));
                Assert.That(a1.End, Is.SameAs(v1a));
                Assert.That(a1.IsInMatching, Is.True);
                Assert.That(a2.Start, Is.SameAs(v1b));
                Assert.That(a2.End, Is.SameAs(v2a));
                Assert.That(a2.IsInMatching, Is.True);
            }
        }
    }
}