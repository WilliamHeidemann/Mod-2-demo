using System.Collections.Generic;
using UnityEngine;

namespace Version_1.Presentation
{
    public class Factory
    {
        private readonly GameObject _cube;
        private readonly GameObject _socket;
        private readonly Material _material;
        private readonly MonoSocket _monoSocket;

        public Factory(SegmentFactoryData data)
        {
            _cube = data.Cube;
            _socket = data.Socket;
            _material = data.Material;
            _monoSocket = data.MonoSocket;
        }
        
        public GameObject SegmentToGameObject(Segment segment)
        {
            GameObject gameObject = new GameObject();

            foreach (Position position in segment.Positions)
            {
                Object.Instantiate(_cube, position.ToVector3(), Quaternion.identity, gameObject.transform);
            }

            foreach ((Position a, Position b) in segment.Positions.PairsOfNeighbors())
            {
                Vector3 halfwayPoint = (a.ToVector3() + b.ToVector3()) / 2f;
                Object.Instantiate(_cube, halfwayPoint, Quaternion.identity, gameObject.transform);
            }
            
            foreach (Socket socket in segment.Sockets)
            {
                Quaternion rotation = Quaternion.LookRotation(socket.Direction.Value.ToVector3());
                GameObject socketRender = Object.Instantiate(_socket, socket.Position.ToVector3(), rotation, gameObject.transform);
                socketRender.GetComponentInChildren<MeshRenderer>().material = _material;
            }

            return gameObject;
        }

        public void SegmentToMonoSockets(Segment segment)
        {
            foreach (Socket socket in segment.Sockets)
            {
                Position position = socket.Position + socket.Direction;
                MonoSocket monoSocket = Object.Instantiate(_monoSocket, position.ToVector3(), Quaternion.identity);
                monoSocket.Position = position;
            }
        }
    }
}