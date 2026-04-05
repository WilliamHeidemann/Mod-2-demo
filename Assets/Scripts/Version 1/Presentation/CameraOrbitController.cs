using UnityEngine;
using UnityEngine.InputSystem;

namespace Version_1.Presentation
{
    public class CameraOrbitController : MonoBehaviour
    {
        [SerializeField] private float _orbitSpeed = 1f;
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private float _elevationSpeed = 1f;
        
        private void Update()
        {
            if (Keyboard.current.aKey.isPressed)
            {
                Rotate(left: true);
            } 
            else if (Keyboard.current.dKey.isPressed)
            {
                Rotate(left: false);
            }

            if (Keyboard.current.wKey.isPressed)
            {
                Zoom(@in: true);
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                Zoom(@in: false);
            }

            if (Keyboard.current.spaceKey.isPressed)
            {
                Elevate(up: true);
            } 
            else if (Keyboard.current.xKey.isPressed)
            {
                Elevate(up: false);
            }
        }
        
        private void Rotate(bool left)
        {
            float angle = left ? _orbitSpeed : -_orbitSpeed;
            transform.RotateAround(point: Vector3.zero, axis: Vector3.up, angle: Time.deltaTime * angle);
        }

        private void Zoom(bool @in)
        {
            Vector3 translation = _zoomSpeed * (@in ? Vector3.forward : Vector3.back);
            transform.Translate(translation: translation * Time.deltaTime, relativeTo: Space.Self);
        }
        
        private void Elevate(bool up)
        {
            Vector3 translation = _elevationSpeed * (up ? Vector3.up : Vector3.down);
            transform.Translate(translation: translation * Time.deltaTime, relativeTo: Space.World);
        }
    }
}
