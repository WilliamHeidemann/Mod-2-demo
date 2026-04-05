using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Version_1.Presentation
{
    public class RotationInputReader : MonoBehaviour
    {
        private Interactions _interactions;

        public void Initialize(Interactions interactions)
        {
            _interactions = interactions;
        }
        
        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                _interactions.OnRotate?.Invoke();
            }
        }
    }
}