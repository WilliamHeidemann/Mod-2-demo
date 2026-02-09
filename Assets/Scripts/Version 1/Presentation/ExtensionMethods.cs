using System;
using UnityEngine;

namespace Version_1.Presentation
{
    public static class ExtensionMethods
    {
        public static Vector3 ToVector3(this Position position) =>
            new(position.X, position.Y, position.Z);

        public static Position ToPosition(this Vector3 vector3) =>
            new(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));

        public static Vector3Int ToVector3Int(this Vector3 vector3) =>
            new(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));

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

        public static Direction ToDirection(this SerializedDirection direction)
        {
            return direction switch
            {
                SerializedDirection.Left => Direction.Left,
                SerializedDirection.Right => Direction.Right,
                SerializedDirection.Up => Direction.Up,
                SerializedDirection.Down => Direction.Down,
                SerializedDirection.Front => Direction.Front,
                SerializedDirection.Back => Direction.Back,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static SerializedDirection ToSerializedDirection(this Direction direction)
        {
            return direction switch
            {
                _ when direction == Direction.Front => SerializedDirection.Front,
                _ when direction == Direction.Back => SerializedDirection.Back,
                _ when direction == Direction.Up => SerializedDirection.Up,
                _ when direction == Direction.Down => SerializedDirection.Down,
                _ when direction == Direction.Left => SerializedDirection.Left,
                _ when direction == Direction.Right => SerializedDirection.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Axis ToAxis(this Vector3 axis)
        {
            return axis switch
            {
                _ when axis == Vector3.forward => Axis.Z,
                _ when axis == Vector3.back => Axis.Z,
                _ when axis == Vector3.up => Axis.Y,
                _ when axis == Vector3.down => Axis.Y,
                _ when axis == Vector3.left => Axis.X,
                _ when axis == Vector3.right => Axis.X,
                _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
            };
        }
    }
}