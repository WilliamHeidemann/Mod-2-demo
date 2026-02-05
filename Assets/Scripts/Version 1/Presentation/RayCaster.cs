using System;
using UnityEngine;

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
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var hit))
            {
                // transform collider center to position
                // ask manager to try and validate and build at position 
            }
        }
    }
}