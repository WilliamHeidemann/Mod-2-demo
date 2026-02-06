using System;
using System.Linq;
using UnityEngine;

namespace Version_1.Presentation
{
    public class SegmentManager : MonoBehaviour
    {
        [SerializeField] private MonoSegment _monoSegment;
        private SegmentGrid _segmentGrid;
        
        private void Start()
        {
            _segmentGrid = new SegmentGrid();
            var startingSegment = _monoSegment.Model;
            _segmentGrid.Add(startingSegment, forceAdd: true);
            Instantiate(_monoSegment);
        }

        public void TryBuild(Position position)
        {
            var segment = _monoSegment.Model.Translate(position);
            if (_segmentGrid.Fits(segment))
            {
                _segmentGrid.Add(segment);
                Instantiate(_monoSegment, position.ToVector3(), Quaternion.identity);
            }
            else
            {
                print($"Failed to build segment {position}!");
            }
        }

        private void OnDrawGizmos()
        {
            if (_segmentGrid == null) return;
            
            var occupiedPositions = _segmentGrid.GetOccupiedPositions().ToHashSet();
            foreach (var socket in _segmentGrid.GetSockets())
            {
                var connectingPosition = socket.Position + socket.Direction;
                if (!occupiedPositions.Contains(connectingPosition))
                {
                    var segment = _monoSegment.Model.Translate(connectingPosition);
                    if (_segmentGrid.Fits(segment))
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(connectingPosition.ToVector3(), Vector3.one);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(connectingPosition.ToVector3(), Vector3.one);
                    }
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(connectingPosition.ToVector3(), Vector3.one);
                }
            }
        }
    }
}