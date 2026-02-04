using System;
using Model;
using UnityEngine;

namespace Core
{
    public static class Extensions
    {
        public static Color AsColor(this Archetype archetype)
        {
            return archetype switch
            {
                Archetype.Blue => Color.dodgerBlue,
                Archetype.Red => Color.red,
                Archetype.Green => Color.forestGreen,
                Archetype.Yellow => Color.yellowNice,
                _ => throw new ArgumentOutOfRangeException(nameof(archetype), archetype, null)
            };
        }

        public static Vector3 AsVector3(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Vector3.up,
                Direction.Down => Vector3.down,
                Direction.Left => Vector3.left,
                Direction.Right => Vector3.right,
                Direction.Forward => Vector3.forward,
                Direction.Backward => Vector3.back,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Vector3Int AsVector3Int(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Vector3Int.up,
                Direction.Down => Vector3Int.down,
                Direction.Left => Vector3Int.left,
                Direction.Right => Vector3Int.right,
                Direction.Forward => Vector3Int.forward,
                Direction.Backward => Vector3Int.back,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Forward => Direction.Backward,
                Direction.Backward => Direction.Forward,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Vector3 WithAbsoluteValues(this Vector3 vector) => 
            new(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));

        public static Vector3Int AsVector3Int(this Vector3 vector) => 
            new(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }
}