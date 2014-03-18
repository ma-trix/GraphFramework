using System;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ABVertexTests
    {
        [SetUp]
        public void Init()
        {
            
        }

        public class TheConstructor1 : ABVertexTests
        {
            private ABVertex _abv;

            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _abv = new ABVertex(VertexType.A);
            }
            
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

        public class TheConstructor2 : ABVertexTests
        {
            private ABVertex _abvA;
            private ABVertex _abvB;
            private const string Name = "bazinga";

            [SetUp]
            public void DerivedInit()
            {
                base.Init();
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

        public class TheOtherMethods : ABVertexTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
            }
            
            [Test]
            public void PushingABVertexMarksItAsPushed()
            {
                var abv = new ABVertex(VertexType.A);
                abv.Pushed();
                Assert.That(abv.IsPushed, Is.True);
            }
            
            [Test]
            public void AddsToE()
            {
                var abv = new ABVertex(VertexType.A);
                var v1 = new ABVertex(VertexType.B);
                var arc = new Arc(null, v1, abv);
                var connection = new Tuple<Arc, StackVertex>(arc, null);
                abv.AddToE(connection);
                Assert.That(ArcHelper.DoesConnectionExist(v1, abv, abv.E));
            }

            [Test]
            public void AddsToR()
            {
                var abv = new ABVertex(VertexType.A);
                var v1 = new ABVertex(VertexType.B);
                var arc = new Arc(null, v1, abv);
                var connection = new Tuple<Arc, StackVertex>(arc, null);
                abv.AddToR(connection);
                Assert.That(ArcHelper.DoesConnectionExist(v1, abv, abv.R));
            }

            [Test]
            public void AddsToD()
            {
                var abv = new ABVertex(VertexType.A);
                var v1 = new ABVertex(VertexType.B);
                abv.AddToD(v1);
                Assert.That(abv.D, Contains.Item(v1));
            }

            [Test]
            public void EmptiesD()
            {
                var abv = new ABVertex(VertexType.A);
                var v1 = new ABVertex(VertexType.B);
                abv.AddToD(v1);
                abv.EmptyD();
                Assert.That(abv.D, Is.Empty);
            }

            [Test]
            public void AddsAnotherDToD()
            {
                var abv = new ABVertex(VertexType.A);
                var v1 = new ABVertex(VertexType.B);
                v1.AddToD(abv);
                v1.AddToD(v1);
                abv.AddAnotherDToD(v1.D);
                Assert.That(abv.D, Contains.Item(abv));
                Assert.That(abv.D, Contains.Item(v1));
            }
            
            [Test]
            public void TwinsHaveNameOfPrecursorWithTypeAppended()
            {
                var name = "badaboom";
                Vertex v = new Vertex(name);
                TwinVertex tv = new TwinVertex(v, null);
                Assert.That(tv.A.Name, Is.EqualTo(name + ".A"));
                Assert.That(tv.B.Name, Is.EqualTo(name + ".B"));
            }
        }
    }
}