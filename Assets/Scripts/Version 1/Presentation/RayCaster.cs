using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Version_1.Presentation
{
    public class RayCaster : MonoBehaviour
    {
        private Camera _camera;
        private Interactions _interactions;
        private Position _lastHover;
        private bool _isHovering;

        public void Initialize(Interactions interactions)
        {
            _camera = Camera.main;
            _interactions = interactions;
        }

        private void LateUpdate()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Position position = hit.collider.bounds.center.ToPosition();
                if (position != _lastHover || !_isHovering)
                {
                    _interactions.OnHoverEnter?.Invoke(position);
                    _lastHover = position;
                    _isHovering = true;
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    _interactions.OnTryBuild?.Invoke(position);
                }
            }
            else
            {
                if (_isHovering)
                {
                    _interactions.OnHoverExit?.Invoke();
                    _isHovering = false;
                }
            }
        }
    }
}