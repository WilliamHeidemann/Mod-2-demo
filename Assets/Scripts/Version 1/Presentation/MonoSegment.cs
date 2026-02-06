using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Version_1.Presentation
{
    public class MonoSegment : MonoBehaviour
    {
        private Option<Segment> _model;
        public Segment Model => _model.IsSome(out var segment) ? segment : ReadSegment();

        private Segment ReadSegment()
        {
            var sockets = ReadSockets();
            var positions = ReadPositions();

            return new Segment
            {
                Sockets = sockets,
                Positions = positions.ToArray()
            };
        }

        private Position[] ReadPositions()
        {
            var cells = GetComponentsInChildren<Cell>();
            var positions = new Position[cells.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = cells[i].transform.position.ToPosition();
            }
            
            return positions;
        }

        private Socket[] ReadSockets()
        {
            var monoSockets = GetComponentsInChildren<MonoSocket>();
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