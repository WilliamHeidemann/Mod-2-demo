using NUnit.Framework;

namespace Version_1.Tests
{
    [TestFixture]
    public class Tests
    {
        private static Segment CenterCube => new()
        {
            Positions = new[] { Position.Center },
            Sockets = new[] { new Socket(Position.Center, Direction.Up, Archetype.Blue) }
        };

        private static Segment Stick => new()
        {
            Positions = new[]
            {
                Position.Center, 
                Position.Center + Direction.Right
            },
            Sockets = new[]
            {
                new Socket(Position.Center + Direction.Right, Direction.Up, Archetype.Blue)
            }
        };

        [Test]
        public void Same_Segment_Cannot_Be_Added_Twice()
        {
            SegmentGrid segmentGrid = new SegmentGrid();
            segmentGrid.TryAdd(CenterCube);
            Assert.AreEqual(1, segmentGrid.Count);

            segmentGrid.TryAdd(CenterCube);
            Assert.AreEqual(1, segmentGrid.Count);
        }

        [Test]
        public void Can_Translate_Segment_One_Up()
        {
            Segment oneUpSegment = CenterCube.Translate(Direction.Up);

            Assert.AreEqual(Direction.Up.Value, oneUpSegment.Positions[0]);

            Assert.AreEqual(Direction.Up.Value, oneUpSegment.Sockets[0].Position);
        }

        [Test]
        public void Can_Rotate_Segment_Around()
        {
            Segment right = CenterCube.Rotate(Axis.Z, Position.Center);
            Assert.AreEqual(Direction.Right.Value, right.Sockets[0].Direction.Value);

            Segment down = right.Rotate(Axis.Z, Position.Center);
            Assert.AreEqual(Direction.Down.Value, down.Sockets[0].Direction.Value);

            Segment left = down.Rotate(Axis.Z, Position.Center);
            Assert.AreEqual(Direction.Left.Value, left.Sockets[0].Direction.Value);

            Segment back = left.Rotate(Axis.Y, Position.Center);
            Assert.AreEqual(Direction.Back.Value, back.Sockets[0].Direction.Value);

            Segment front = back.Rotate(Axis.X, Position.Center).Rotate(Axis.X, Position.Center);
            Assert.AreEqual(Direction.Front.Value, front.Sockets[0].Direction.Value);
        }

        [Test]
        public void Can_Rotate_And_Translate_Segment()
        {
            Segment segment = CenterCube.Translate(Direction.Up);
            Assert.AreEqual(Direction.Up.Value, segment.Positions[0]);

            segment = segment.Translate(Direction.Right * 3);
            Assert.AreEqual(new Position(3, 1, 0), segment.Positions[0]);
        }

        [Test]
        public void Rotation_May_Change_Position()
        {
            Segment oneUpSegment = CenterCube.Translate(Direction.Up);
            Segment oneRightSegment = oneUpSegment.Rotate(Axis.Z, Position.Center);
            Assert.AreEqual(Direction.Right.Value, oneRightSegment.Positions[0]);
        }

        [Test]
        public void Translate_1_000_000_Times()
        {
            Segment segment = CenterCube;
            for (int i = 0; i < 1_000_000; i++)
            {
                segment = segment.Translate(Direction.Up);
            }

            Assert.AreEqual(1_000_000, segment.Positions[0].Y);
        }

        [Test]
        public void StickMayRotateAndConnect()
        {
            Segment rotatedStick = Stick.Rotate(Axis.Z, Position.Center);
            
            Assert.True(rotatedStick.Positions[1] == new Position(0, -1, 0));
            Assert.True(rotatedStick.Sockets[0].Position == new Position(0, -1, 0));
            Assert.True(rotatedStick.Sockets[0].Direction == Direction.Right);
            
            Segment connection =
                CenterCube
                    .Rotate(Axis.Z, Position.Center)
                    .Rotate(Axis.Z, Position.Center)
                    .Rotate(Axis.Z, Position.Center)
                    .Translate(Direction.Right)
                    .Translate(Direction.Down);
            
            Assert.True(connection.Positions[0] == new Position(1, -1, 0));
            Assert.True(connection.Sockets[0].Position == new Position(1, -1, 0));
            Assert.True(connection.Sockets[0].Direction == Direction.Left);
            
            Assert.True(rotatedStick.Sockets[0].ConnectsTo(connection.Sockets[0]));
            
            SegmentGrid grid = new();
            grid.TryAdd(rotatedStick);
            Assert.True(grid.Fits(connection));
        }
    }
}