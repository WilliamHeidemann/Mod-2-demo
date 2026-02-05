using System.Linq;

namespace Version_1
{
    public static class DirectionExtensions
    {
        public static Direction Rotate(this Direction direction, Axis axis)
        {
            var rotatedDirection = direction.Value.Rotate(axis);
            return rotatedDirection.AsDirection();
        }

        public static Direction AsDirection(this Position position)
        {
            return Direction.All.Single(dir => dir.Value == position);
        }
    }
}