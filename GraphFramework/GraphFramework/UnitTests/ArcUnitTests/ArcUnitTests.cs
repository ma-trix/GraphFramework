using Moq;
using NUnit.Framework;

namespace GraphFramework.UnitTests.ArcUnitTests
{
    public abstract class ArcUnitTests
    {
        private IVertex _v1;
        private IVertex _v2;
        private Graph _g;
        private Arc _a;

        [SetUp]
        public void Init()
        {
            _v1 = new Vertex();
            _v2 = new Vertex();
            _g = new Graph();
        }

        [TestFixture]
        public class TheConstructor3 : ArcUnitTests 
        {
            [SetUp]
            public void DerivedInit()
            {
                _a = new Arc(_g, _v1, _v2);
            }

            [Test]
            public void SetsStartVertexOnCreate()
            {
                Assert.That(_a.Start, Is.SameAs(_v1));
            }

            [Test]
            public void SetsEndVertexOnCreate()
            {
                Assert.That(_a.End, Is.SameAs(_v2));
            }

            [Test]
            public void SetsGraphOnCreate()
            {
                Assert.AreSame(_g, _a.Graph);
            }

            [Test]
            public void WeightEqualsZeroOnCreate()
            {
                Assert.That(_a.Weight, Is.EqualTo(0.0));
            }

            [Test]
            public void DoubleWeightEqualsZeroOnCreate()
            {
                Assert.That(_a.DualWeight, Is.EqualTo(0.0));
            }

            [Test]
            public void EdgeSetIsEmptyOnCreate()
            {
                Assert.That(_a.EdgeSet, Is.Null);
            }
        }

        [TestFixture]
        public class TheConstructor4 : ArcUnitTests
        {
            [Test]
            public void CanCreateArcInMatching()
            {
                _a = new Arc(_g, _v1, _v2, true);
                Assert.That(_a.IsInMatching, Is.True);
            }
        }

        [TestFixture]
        public class TheAddToMatchingMethod : ArcUnitTests
        {
            [Test]
            public void KnowsItIsInMatching()
            {
                _a = new Arc(_g, _v1, _v2);
                _a.AddToMatching();
                Assert.AreEqual(true, _a.IsInMatching);
            }     
        }

        [TestFixture]
        public class TheRevertMethod : ArcUnitTests
        {
            private Mock<IVertex> _mock;

            [SetUp]
            public void DerivedInit()
            {
                _a = new Arc(null, _v1, _v2);
                _mock = new Mock<IVertex>();
            }

            [Test]
            public void RevertingSetsNewStartVertexToOldEndVertex()
            {
                var oldEnd = _a.End;
                _a.Revert();
                Assert.That(_a.Start, Is.SameAs(oldEnd));
            }

            [Test]
            public void RevertingSetsNewEndVertexToOldStartVertex()
            {
                var oldStart = _a.Start;
                _a.Revert();
                Assert.That(_a.End, Is.SameAs(oldStart));
            }

            [Test]
            public void RevertingRemovesInMatchingArcFromMatching()
            {
                _a.AddToMatching();
                _a.Revert();
                Assert.That(_a.IsInMatching, Is.False);
            }
            
            [Test]
            public void RevertingAddsNonMatchingArcToMatching()
            {
                _a.Revert();
                Assert.That(_a.IsInMatching, Is.True);
            }
            
            [Test]
            public void RevertingInformsOldStartVertex()
            {
                var a = new Arc(null, _mock.Object, _v2);
                _mock.Setup(m => m.ArcReverted(a)).Verifiable();
                a.Revert();
                _mock.Verify(m => m.ArcReverted(a));
            }
            
            [Test]
            public void RevertingInformsOldEndVertex()
            {
                var a = new Arc(null, _v1, _mock.Object);
                _mock.Setup(m => m.ArcReverted(a)).Verifiable();
                a.Revert();
                _mock.Verify(m => m.ArcReverted(a));
            }
        }

        [TestFixture]
        public class TheReducedWeightProperty : ArcUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                _a = new Arc(null, null, null);
                var mock = new Mock<IEdgeSet>();
                mock.Setup(m => m.Weight).Returns(1.0);
                _a.Weight = 1.0;
                _a.DualWeight = 2.0;
                _a.EdgeSet = mock.Object;
            }

            [Test]
            public void ReducedWeightEqualsToSumOfWieghtDualWeightMinusEdgeSetWeight()
            {
                Assert.That(_a.ReducedWeight, Is.EqualTo(2.0));
            }
        }
    }
}
