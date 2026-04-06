using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Version_1.Presentation
{
    public class SDFController : MonoBehaviour
    {
        private static readonly int PositionsBuffer = Shader.PropertyToID("_PositionsBuffer");
        private static readonly int PositionsCount = Shader.PropertyToID("_PositionsCount");
        [SerializeField] private Material _material;
        [SerializeField] private Vector4[] _positions;
        private ComputeBuffer _positionsBuffer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // private void Start()
        // {
        //     // 1. Initialize the buffer: (number of elements, size of one element in bytes)
        //     // Vector4 = 4 floats * 4 bytes = 16 bytes
        //     _positionsBuffer = new ComputeBuffer(_positions.Length, 4 * sizeof(float));
        //
        //     // 2. Upload data to GPU
        //     _positionsBuffer.SetData(_positions);
        //
        //     // 3. Bind the buffer to the material
        //     _material.SetBuffer(PositionsBuffer, _positionsBuffer);
        //
        //     // 4. Tell the shader how many items are in the buffer
        //     _material.SetInt(PositionsCount, _positions.Length);
        // }

        // Update is called once per frame
        // private void Update()
        // {
        //     _positionsBuffer.SetData(_positions);
        //     _material.SetInt(PositionsCount, _positions.Length);
        //     _material.SetBuffer(PositionsBuffer, _positionsBuffer);
        // }

        private void OnDestroy()
        {
            // CRITICAL: Buffers are not garbage collected. You must release them manually!
            _positionsBuffer?.Release();
            _material.SetInt(PositionsCount, 0);
        }

        public void Append(Vector4[] positions)
        {
            Vector4[] temp = new Vector4[_positions.Length + positions.Length];
            for (int i = 0; i < _positions.Length; i++)
            {
                temp[i] = _positions[i];
            }

            for (int i = 0; i < positions.Length; i++)
            {
                temp[i + _positions.Length] = positions[i];
            }

            _positions = new Vector4[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                _positions[i] = temp[i];
            }

            _positionsBuffer = new ComputeBuffer(temp.Length, 4 * sizeof(float));
            _positionsBuffer.SetData(_positions);
            _material.SetBuffer(PositionsBuffer, _positionsBuffer);
            _material.SetInt(PositionsCount, _positions.Length);
        }
    }
}