using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityToolkit.Editor;

namespace Version_1.Presentation
{
    public class SDFController : MonoBehaviour
    {
        private static readonly int PositionsBuffer = Shader.PropertyToID("_PositionsBuffer");
        private static readonly int PositionsCount = Shader.PropertyToID("_PositionsCount");
        private static readonly int SocketsBuffer = Shader.PropertyToID("_SocketsBuffer");
        private static readonly int SocketsCount = Shader.PropertyToID("_SocketsCount");
        [SerializeField] private Material _material;
        [SerializeField] private Vector4[] _positions;
        [SerializeField] private Vector4[] _sockets;
        private ComputeBuffer _positionsBuffer;
        private ComputeBuffer _socketsBuffer;
        
        private void OnDestroy()
        {
            // CRITICAL: Buffers are not garbage collected. You must release them manually!
            _positionsBuffer?.Release();
            _material.SetInt(PositionsCount, 0);
        }

        public void AppendPositions(Vector4[] positions)
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

        public void AppendSockets(Vector4[] sockets)
        {
            Vector4[] temp = new Vector4[_sockets.Length + sockets.Length];
            for (int i = 0; i < _sockets.Length; i++)
            {
                temp[i] = _sockets[i];
            }

            for (int i = 0; i < sockets.Length; i++)
            {
                temp[i + _sockets.Length] = sockets[i];
            }

            _sockets = new Vector4[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                _sockets[i] = temp[i];
            }

            _socketsBuffer = new ComputeBuffer(temp.Length, 4 * sizeof(float));
            _socketsBuffer.SetData(_sockets);
            _material.SetBuffer(SocketsBuffer, _socketsBuffer);
            _material.SetInt(SocketsCount, _sockets.Length);
        }

        [Button]
        public void UpdatePositions()
        {
            AppendPositions(Array.Empty<Vector4>());
            AppendSockets(Array.Empty<Vector4>());
        }
    }
}