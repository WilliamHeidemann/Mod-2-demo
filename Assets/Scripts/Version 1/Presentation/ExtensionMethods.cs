
using System;
using UnityEngine;

namespace Version_1.Presentation
{
    public static class ExtensionMethods
    {
        public static Vector3 ToVector3(this Position position) => 
            new(position.X, position.Y, position.Z);

        public static Position ToPosition(this Vector3 vector3) => 
            new(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y),  Mathf.RoundToInt(vector3.z));

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
    }
}