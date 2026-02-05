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
                new Socket(Center, Direction.Up, Archetype.Blue)
            }
        };

        [Test]
        public void Same_Segment_Cannot_Be_Added_Twice()
        {
            var segmentGrid = new SegmentGrid();
            segmentGrid.Add(CenterCube, forceAdd: true);
            Assert.AreEqual(1, segmentGrid.Count);

            segmentGrid.Add(CenterCube);
            Assert.AreEqual(1, segmentGrid.Count);
        }

        [Test]
        public void Can_Translate_Segment_One_Up()
        {
            var oneUpSegment = CenterCube.Translate(Direction.Up);

            Assert.AreEqual(Direction.Up.Value, oneUpSegment.Positions[0]);

            Assert.AreEqual(Direction.Up.Value, oneUpSegment.Sockets[0].Position);
        }

        [Test]
        public void Can_Rotate_Segment_Around()
        {
            var right = CenterCube.Rotate(Axis.Z, Center);
            Assert.AreEqual(Direction.Right.Value, right.Sockets[0].Direction.Value);

            var down = right.Rotate(Axis.Z, Center);
            Assert.AreEqual(Direction.Down.Value, down.Sockets[0].Direction.Value);

            var left = down.Rotate(Axis.Z, Center);
            Assert.AreEqual(Direction.Left.Value, left.Sockets[0].Direction.Value);

            var back = left.Rotate(Axis.Y, Center);
            Assert.AreEqual(Direction.Back.Value, back.Sockets[0].Direction.Value);

            var front = back.Rotate(Axis.X, Center).Rotate(Axis.X, Center);
            Assert.AreEqual(Direction.Front.Value, front.Sockets[0].Direction.Value);
        }

        [Test]
        public void Rotation_May_Change_Position()
        {
            var oneUpSegment = CenterCube.Translate(Direction.Up);
            var oneRightSegment = oneUpSegment.Rotate(Axis.Z, Center);
            Assert.AreEqual(Direction.Right.Value, oneRightSegment.Positions[0]);
        }
    }
}