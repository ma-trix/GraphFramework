﻿using System;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ABVertexTests
    {
        protected ABVertex Abv;

        public void Init()
        {
            Abv = new ABVertex(VertexType.A);
        }

        public class TheConstructor1 : ABVertexTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
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

        public class ThePushedMethod : ABVertexTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
            }

            [Test]
            public void PushingABVertexMarksItAsPushed()
            {
                Abv.Pushed();
                Assert.That(Abv.IsPushed, Is.True);
            }
        }

        public class TheAddToDERMethods : ABVertexTests
        {
            private ABVertex _v;
            private Arc _arc;
            private Connection _connection;

            [SetUp]
            public void DerivedInit()
            {
                base.Init();
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

        public class TheOtherMethods : ABVertexTests
        {
            private ABVertex _v;

            [SetUp]
            public void DerivedInit()
            {
                base.Init();
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
            public void AddsAnotherDToD()
            {
                _v.AddToD(Abv);
                _v.AddToD(_v);
                Abv.AddAnotherDToD(_v.D);
                Assert.That(Abv.D, Has.Member(Abv));
                Assert.That(Abv.D, Has.Member(_v));
            }
            
            [Test]
            public void TwinsHaveNameOfPrecursorWithTypeAppended()
            {
                const string name = "badaboom";
                Vertex v = new Vertex(name);
                TwinVertex tv = new TwinVertex(v, null);
                Assert.That(tv.A.Name, Is.EqualTo(name + ".A"));
                Assert.That(tv.B.Name, Is.EqualTo(name + ".B"));
            }
        }

        public class TheIStackVertexInterfaceMethods : ABVertexTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
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
                IStackVertex v = new ABVertex(VertexType.A);
                Abv.AddDescendant(v);
                Assert.That(Abv.Descendants, Has.Member(v));
            }
        }
    }
}