using System;
using System.Linq;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Version_1.Presentation
{
    public class SegmentManager : MonoBehaviour
    {
        [SerializeField] private MonoSegment _monoSegment;
        private SegmentGrid _segmentGrid;

        private Option<(MonoSegment, Position)> _hoverMonoSegment;

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

        public void SocketHovered(Position position)
        {
            if (_hoverMonoSegment.IsSome(out (MonoSegment segment, Position position) tuple))
            {
                if (position == tuple.position)
                {
                    return;
                }

                SocketUnHovered();
            }

            Segment originSegment = _monoSegment.Model.Translate(position);
            foreach (Socket socket in originSegment.Sockets)
            {
                Segment segment = _monoSegment.Model.Translate(socket.Position);
                if (_segmentGrid.Fits(segment))
                {
                    MonoSegment hover = Instantiate(_monoSegment, socket.Position.ToVector3(), Quaternion.identity);
                    _hoverMonoSegment = Option<(MonoSegment, Position)>.Some((hover, position));
                    return;
                }
            }
        }

        public void SocketUnHovered()
        {
            if (_hoverMonoSegment.IsSome(out (MonoSegment segment, Position position) tuple))
            {
                Destroy(tuple.segment.gameObject);
                _hoverMonoSegment = Option<(MonoSegment, Position)>.None;
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
                        Gizmos.DrawWireCube(connectingPosition.ToVector3(), Vector3.one * 0.9f);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(connectingPosition.ToVector3(), Vector3.one * 0.9f);
                    }
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(connectingPosition.ToVector3(), Vector3.one * 0.9f);
                }
            }
        }
    }
}