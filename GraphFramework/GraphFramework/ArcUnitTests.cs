using Moq;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ArcUnitTests
    {
        private IVertex _v1;
        private IVertex _v2;
        private Graph _g;
        private Arc _a;

        public void Init()
        {
            _v1 = new Vertex();
            _v2 = new Vertex();
            _g = new Graph();
        }

        public class TheConstructor3 : ArcUnitTests 
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
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
        }

        public class TheConstructor4 : ArcUnitTests
        {
            [Test]
            public void CanCreateArcInMatching()
            {
                _a = new Arc(_g, _v1, _v2, true);
                Assert.That(_a.IsInMatching, Is.True);
            }
        }

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
        
        public class TheRevertMethod : ArcUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
                _a = new Arc(null, _v1, _v2);
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
        }
    }
}
