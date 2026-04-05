using UnityEngine;
using UnityEngine.InputSystem;

namespace Version_1.Presentation
{
    public class CameraOrbitController : MonoBehaviour
    {
        [SerializeField] private float _orbitSpeed = 1f;
        [SerializeField] private float _zoomSpeed = 1f;
    
        private void Update()
        {
            if (Keyboard.current.aKey.isPressed)
            {
                Rotate(true);
            } 
            else if (Keyboard.current.dKey.isPressed)
            {
                Rotate(false);
            }

            if (Keyboard.current.wKey.isPressed)
            {
                Zoom(true);
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                Zoom(false);
            }
        }

        private void Rotate(bool left)
        {
            float angle = left ? _orbitSpeed : -_orbitSpeed;
            transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * angle);
        }

        private void Zoom(bool @in)
        {
            Vector3 translation = _zoomSpeed * (@in ? Vector3.forward : Vector3.back);
            transform.Translate(translation * Time.deltaTime, Space.Self);
        }
    }
}
