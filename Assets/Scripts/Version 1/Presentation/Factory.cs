using System.Collections.Generic;
using UnityEngine;
using Version_1.Domain;
using Version_1.Domain.ExtensionMethods;

namespace Version_1.Presentation
{
    public class Factory
    {
        private readonly GameObject _cube;
        private readonly GameObject _socket;
        private readonly MonoSocket _monoSocket;
        private readonly Dictionary<Archetype, Material> _materials;

        public Factory(SegmentFactoryData data)
        {
            _cube = data.Cube;
            _socket = data.Socket;
            _monoSocket = data.MonoSocket;
            _materials = data.Materials.Build();
        }
        
        public GameObject SegmentToGameObject(Segment segment)
        {
            GameObject gameObject = new GameObject();

            foreach (Position position in segment.Positions)
            {
                Object.Instantiate(_cube, position.ToVector3(), Quaternion.identity, gameObject.transform);
            }

            foreach (Vector3 halfwayPoint in segment.Positions.PairsOfNeighbors().HalfwayPoints())
            {
                Object.Instantiate(_cube, halfwayPoint, Quaternion.identity, gameObject.transform);
            }
            
            foreach (Socket socket in segment.Sockets)
            {
                Quaternion rotation = Quaternion.LookRotation(socket.Direction.Value.ToVector3());
                GameObject socketRender = Object.Instantiate(_socket, socket.Position.ToVector3(), rotation, gameObject.transform);
                socketRender.GetComponentInChildren<MeshRenderer>().material = _materials[socket.Archetype];
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