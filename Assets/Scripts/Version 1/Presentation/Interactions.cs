using System;

namespace Version_1.Presentation
{
    public class Interactions
    {
        public Action<Position> OnHoverEnter;
        public Action OnHoverExit;
        public Action<Position> OnTryBuild;
        
        public Action OnRotate;
    }
}