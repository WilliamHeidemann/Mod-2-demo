using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using Version_1.Domain;

namespace Version_1.Presentation
{
    public class CameraPositioner
    {
        private readonly Transform _transform;
        private readonly Vector3 _offset;
    
        public CameraPositioner(Transform transform, Interactions interactions)
        {
            _transform = transform;
            interactions.OnHoverEnter += Move;
            _offset = transform.position;
        }

        private void Move(Position position)
        {
            LMotion
                .Create(_transform.position, position.ToVector3() + _offset, 0.3f)
                .WithEase(Ease.OutExpo)
                .BindToPosition(_transform);
        }
    }
}
