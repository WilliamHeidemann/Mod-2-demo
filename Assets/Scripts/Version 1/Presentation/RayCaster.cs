using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Version_1.Presentation
{
    public class RayCaster : MonoSegment
    {
        private Camera _camera;
        [SerializeField] private SegmentManager segmentManager;
        
        private void Awake()
        {
            _camera = Camera.main;
        }
        
        private void LateUpdate()
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (Physics.Raycast(ray, out var hit))
            {
                var position = ToPosition(hit.collider.bounds.center); 

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    segmentManager.TryBuild(position);
                }
            }
        }

        private Position ToPosition(Vector3 position) => 
            new(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
    }
}