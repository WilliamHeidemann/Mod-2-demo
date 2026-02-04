using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class Segment
    {
        [SerializeField] private List<Vector3Int> occupiedCells;
        [SerializeField] private List<ConnectionPoint> connectionPoints;

        public List<Vector3Int> OccupiedCells => occupiedCells;
        public List<ConnectionPoint> ConnectionPoints => connectionPoints;

        public Segment(List<Vector3Int> occupiedCells, List<ConnectionPoint> connectionPoints)
        {
            this.occupiedCells = occupiedCells;
            this.connectionPoints = connectionPoints;
        }
        
        public IEnumerable<Vector3Int> GetWorldCells(Vector3Int origin)
        {
            foreach (var position in OccupiedCells)
            {
                yield return new Vector3Int(
                    position.x + origin.x,
                    position.y + origin.y,
                    position.z + origin.z);
            }
        }

        public IEnumerable<ConnectionPoint> GetWorldConnections(Vector3Int origin)
        {
            foreach (var cp in ConnectionPoints)
            {
                yield return new ConnectionPoint(
                    new Vector3Int(
                        cp.Position.x + origin.x,
                        cp.Position.y + origin.y,
                        cp.Position.z + origin.z),
                    cp.Direction,
                    cp.Archetype);
            }
        }
    }
}