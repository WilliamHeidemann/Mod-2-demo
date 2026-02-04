using System;

namespace Version_1
{
    public static class Math
    {
        public static Segment Rotate(Segment segment, Rotation rotation)
        {
            throw new NotImplementedException();
        }

        public static bool Connects(Socket first, Socket second)
        {
            if (first.Archetype != second.Archetype)
                return false;
            
            if (first.Position + first.Direction != second.Position)
                return false;
            
            if (second.Position + second.Direction != first.Position)
                return false;

            return true;
        }
        
        public static Segment ToWorldSegment(Position worldPosition)
        {
            throw new NotImplementedException();
        }
    }
}