//-nullable:enable

using System.Collections.Generic;

namespace Version_1
{
    public sealed class Segment
    {
        public IReadOnlyList<Socket> Sockets { get; init; }
        public IReadOnlyList<Position> Positions { get; init; }
    }

    public sealed record Socket(Position Position, Direction Direction, Archetype Archetype);

    public sealed record Position(int X, int Y, int Z)
    {
        public static Position operator +(Position a, Position b) =>
            new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public sealed class Direction
    {
        public Position Value { get; }

        private Direction(int x, int y, int z)
        {
            Value = new Position(x, y, z);
        }

        public static implicit operator Position(Direction direction) => direction.Value;

        public static readonly Direction Up = new(0, 1, 0);
        public static readonly Direction Down = new(0, -1, 0);
        public static readonly Direction Front = new(0, 0, 1);
        public static readonly Direction Back = new(0, 0, -1);
        public static readonly Direction Right = new(1, 0, 0);
        public static readonly Direction Left = new(-1, 0, 0);

        public static IReadOnlyList<Direction> All { get; } =
            new[] { Up, Down, Front, Back, Right, Left };
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public enum Archetype
    {
        Blue,
        Red,
        Green,
        Yellow,
    }
}