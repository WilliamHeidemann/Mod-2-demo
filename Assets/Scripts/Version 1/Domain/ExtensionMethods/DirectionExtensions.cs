using System.Linq;

namespace Version_1
{
    public static class DirectionExtensions
    {
        public static Direction Rotate(this Direction direction, Axis axis)
        {
            var rotatedDirection = direction.Value.Rotate(axis);
            return Direction.All.Single(dir => dir.Value == rotatedDirection);
        }
    }
}