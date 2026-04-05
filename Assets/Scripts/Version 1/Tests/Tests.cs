using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Version_1.Domain;

namespace Version_1.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Same_Segment_Cannot_Be_Added_Twice()
        {
            SegmentGrid segmentGrid = new SegmentGrid();
            segmentGrid.TryAdd(Generator.CenterCube);
            Assert.AreEqual(1, segmentGrid.Count);

            segmentGrid.TryAdd(Generator.CenterCube);
            Assert.AreEqual(1, segmentGrid.Count);
        }

        [Test]
        public void Can_Translate_Segment_One_Up()
        {
            Segment oneUpSegment = Generator.CenterCube.Translate(Direction.Up);

            Assert.AreEqual(Direction.Up.Value, oneUpSegment.Positions[0]);

            Assert.AreEqual(Direction.Up.Value, oneUpSegment.Sockets[0].Position);
        }

        [Test]
        public void Can_Rotate_Segment_Around()
        {
            Segment right = Generator.CenterCube.Rotate(Axis.Z);
            Assert.AreEqual(Direction.Right.Value, right.Sockets[0].Direction.Value);

            Segment down = right.Rotate(Axis.Z);
            Assert.AreEqual(Direction.Down.Value, down.Sockets[0].Direction.Value);

            Segment left = down.Rotate(Axis.Z);
            Assert.AreEqual(Direction.Left.Value, left.Sockets[0].Direction.Value);

            Segment back = left.Rotate(Axis.Y);
            Assert.AreEqual(Direction.Back.Value, back.Sockets[0].Direction.Value);

            Segment front = back.Rotate(Axis.X).Rotate(Axis.X);
            Assert.AreEqual(Direction.Front.Value, front.Sockets[0].Direction.Value);
        }

        [Test]
        public void Can_Rotate_And_Translate_Segment()
        {
            Segment segment = Generator.CenterCube.Translate(Direction.Up);
            Assert.AreEqual(Direction.Up.Value, segment.Positions[0]);

            segment = segment.Translate(Direction.Right * 3);
            Assert.AreEqual(new Position(3, 1, 0), segment.Positions[0]);
        }

        [Test]
        public void Translate_1_000_000_Times()
        {
            Segment segment = Generator.CenterCube;
            for (int i = 0; i < 1_000_000; i++)
            {
                segment = segment.Translate(Direction.Up);
            }

            Assert.AreEqual(1_000_000, segment.Positions[0].Y);
        }

        [Test]
        public void StickMayRotateAndConnect()
        {
            Segment rotatedStick = Generator.Stick.Rotate(Axis.Z);

            Assert.True(rotatedStick.Positions[1] == new Position(0, -1, 0));
            Assert.True(rotatedStick.Sockets[0].Position == new Position(0, -1, 0));
            Assert.True(rotatedStick.Sockets[0].Direction == Direction.Right);

            Segment connection =
                Generator.CenterCube
                    .Rotate(Axis.Z)
                    .Rotate(Axis.Z)
                    .Rotate(Axis.Z)
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

        [Test]
        public void BoxSegmentHas24Rotations()
        {
            Segment segment = Generator.CenterCube;
            List<Segment> rotations = segment.GetStatesInPivot().ToList();
            Assert.AreEqual(24, rotations.Count);
        }

        [Test]
        public void StickSegmentHas48Rotations()
        {
            Segment segment = Generator.Stick;
            List<Segment> rotations = segment.GetAllStates().ToList();
            Assert.AreEqual(48, rotations.Count);
        }

        [Test]
        public void BoxAndStickFitIn4Ways()
        {
            SegmentGrid grid = new();
            grid.TryAdd(Generator.CenterCube);
            List<Segment> rotationsThatFitStick = Generator.Stick
                .Translate(Direction.Up)
                .GetAllStates()
                .Where(grid.Fits)
                .ToList();
            Assert.AreEqual(4, rotationsThatFitStick.Count);
        }

        [Test]
        public void BoxOneUpAlwaysContains010()
        {
            List<Segment> states = Generator.CenterCube
                .Translate(Direction.Up)
                .GetAllStates()
                .ToList();
            
            
            Assert.True(states.All(segment => segment.Positions.Contains(Direction.Up)));
        }
    }
}