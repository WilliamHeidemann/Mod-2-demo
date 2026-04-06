using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Version_1.Utility;

namespace Version_1.Domain.ExtensionMethods
{
    public static class PositionExtensions
    {
        public static Position Rotate(this Position position, Axis axis)
            => axis switch
            {
                Axis.X => new Position(position.X, position.Z, -position.Y),
                Axis.Y => new Position(-position.Z, position.Y, position.X),
                Axis.Z => new Position(position.Y, -position.X, position.Z),
                _ => throw new ArgumentOutOfRangeException(nameof(axis))
            };

        public static Position RotateAround(this Position position, Position pivot, Axis axis)
        {
            // move to origin
            Position translated = new Position(
                position.X - pivot.X,
                position.Y - pivot.Y,
                position.Z - pivot.Z);

            // rotate
            Position rotated = translated.Rotate(axis);

            // move back
            return new Position(
                rotated.X + pivot.X,
                rotated.Y + pivot.Y,
                rotated.Z + pivot.Z);
        }

        public static bool IsNextTo(this Position a, Position b) =>
            Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y) + Mathf.Abs(a.Z - b.Z) == 1;

        public static IEnumerable<(Position First, Position Second)> PairsOfNeighbors(
            this IEnumerable<Position> positions)
        {
            List<Position> list = positions.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].IsNextTo(list[j]))
                    {
                        yield return (list[i], list[j]);
                    }
                }
            }
        }
    }
}