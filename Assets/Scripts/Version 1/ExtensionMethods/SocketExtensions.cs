namespace Version_1
{
    public static class SocketExtensions
    {
        public static bool ConnectsTo(this Socket first, Socket second)
        {
            if (first.Archetype != second.Archetype)
                return false;
            
            if (first.Position + first.Direction != second.Position)
                return false;
            
            if (second.Position + second.Direction != first.Position)
                return false;

            return true;
        }
    }
}