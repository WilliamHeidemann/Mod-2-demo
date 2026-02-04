using NUnit.Framework;

namespace Version_1.Tests
{
    [TestFixture]
    public class Tests
    {
        private static Position Center => new(0, 0, 0);

        private static Segment CenterCube => new()
        {
            Positions = new[] { Center },
            Sockets = new[]
            {
                new Socket
                {
                    Position = Center,
                    Direction = Direction.Up,
                    Archetype = Archetype.Blue
                }
            }
        };

        [Test]
        public void Same_Segment_Cannot_Be_Added_Twice()
        {
            var segmentGrid = new SegmentGrid(CenterCube);
            Assert.AreEqual(1, segmentGrid.Count);

            segmentGrid.Add(CenterCube);
            Assert.AreEqual(1, segmentGrid.Count);
        }

        [Test]
        public void Can_Translate_Segment_One_Up()
        {
            var oneUpSegment = CenterCube.Translate(Direction.Up);
            
            Assert.AreEqual(oneUpSegment.Positions[0], Direction.Up.Value);
            
            Assert.AreEqual(oneUpSegment.Sockets[0].Position, Direction.Up.Value);
        }
    }
}