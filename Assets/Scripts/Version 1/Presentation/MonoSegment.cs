using System.Linq;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Version_1.Presentation
{
    public class MonoSegment : MonoBehaviour
    {
        private Option<Segment> _model;
        private MonoSocket[] _sockets;
        // public Segment Model => _model.IsSome(out var segment) ? segment : ReadSegment();
        public Segment Model => ReadSegment();
        
        public void EnableColliders()
        {
            if (_sockets == null) _sockets = GetComponentsInChildren<MonoSocket>();
            
            foreach (var socket in _sockets)
            {
                socket.EnableCollider();
            }
        }

        private Segment ReadSegment()
        {
            Socket[] sockets = ReadSockets();
            Position[] positions = ReadPositions();

            return new Segment
            {
                Sockets = sockets,
                Positions = positions.ToArray()
            };
        }

        private Position[] ReadPositions()
        {
            Cell[] cells = GetComponentsInChildren<Cell>();
            Position[] positions = new Position[cells.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = cells[i].transform.position.ToPosition();
            }
            
            return positions;
        }

        private Socket[] ReadSockets()
        {
            var monoSockets = GetComponentsInChildren<MonoSocket>();
            _sockets = monoSockets;
            var sockets = new Socket[monoSockets.Length];
            for (var i = 0; i < monoSockets.Length; i++)
            {
                var monoSocket = monoSockets[i];
                sockets[i] = new Socket(monoSocket.transform.position.ToPosition(), 
                    monoSocket.Direction, monoSocket.Archetype);
            }

            return sockets;
        }
    }
}