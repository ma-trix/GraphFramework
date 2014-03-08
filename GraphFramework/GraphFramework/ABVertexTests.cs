using System;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ABVertexTests
    {
        [Test]
        public void NewTypedVertexIsNotPushed()
        {
            var abv = new ABVertex(VertexType.A);
            Assert.That(abv.IsPushed, Is.False);
        }

        [Test]
        public void PushingABVertexMarksItAsPushed()
        {
            var abv = new ABVertex(VertexType.A);
            abv.Pushed();
            Assert.That(abv.IsPushed, Is.True);
        }

        [Test]
        public void NewVertexHasEmptyE()
        {
            var abv = new ABVertex(VertexType.A);
            Assert.That(abv.E, Is.Empty);
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
        public void NewVertexHasEmptyR()
        {
            var abv = new ABVertex(VertexType.A);
            Assert.That(abv.R, Is.Empty);
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
        public void NewVertexHasEmptyD()
        {
            var abv = new ABVertex(VertexType.A);
            Assert.That(abv.D, Is.Empty);
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
    }
}