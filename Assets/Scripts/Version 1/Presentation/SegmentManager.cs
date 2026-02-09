using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UtilityToolkit.Runtime;
using Version_1.Domain;

namespace Version_1.Presentation
{
    public class SegmentManager : MonoBehaviour
    {
        [SerializeField] private SegmentInstantiator _instantiator;

        private SegmentGrid _segmentGrid;
        private Segment _segment;
        private MonoSegment _segmentPresentation;
        private Quaternion _rotation = Quaternion.identity;
        private Position _translation = new(0, 0, 0);
        
        private void Start()
        {
            _segmentGrid = new SegmentGrid();

            _segment = Generator.Generate();
            _segmentPresentation = _instantiator.Instantiate(_segment);
            _segmentGrid.Add(_segment, forceAdd: true);
            _segmentPresentation.EnableColliders();
            
            _segment = Generator.Generate();
            _segmentPresentation = _instantiator.Instantiate(_segment);
            _segmentPresentation.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                Rotate(Vector3.right);
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Rotate(Vector3.forward);
            }
        }

        private void Rotate(Vector3 axis)
        {
            _segment = _segment.Rotate(axis.ToAxis(), Position.Center);
            _rotation *= Quaternion.AngleAxis(90f, axis);
            _segmentPresentation.transform.rotation = _rotation;
            _segmentPresentation.gameObject.SetActive(_segmentGrid.Fits(_segment));
        }

        public void TryBuild(Position position)
        {
            Segment segment = _segment.Translate(_translation);
            if (_segmentGrid.Fits(segment))
            {
                _segmentGrid.Add(segment);
                _segmentPresentation.EnableColliders();
                
                _segment = Generator.Generate();
                _segmentPresentation = _instantiator.Instantiate(_segment);
                _segmentPresentation.gameObject.SetActive(false);
            }
            else
            {
                print($"Failed to build segment {position}!");
            }
        }

        public void SocketHovered(Position position)
        {
            _segmentPresentation.gameObject.SetActive(true);

            Segment segmentAtHoverPosition = _segment.Translate(position);
            foreach (Socket hoverPositionSocket in segmentAtHoverPosition.Sockets)
            {
                Segment segmentAtSocketPosition = _segment.Translate(hoverPositionSocket.Position);
                if (_segmentGrid.Fits(segmentAtSocketPosition))
                {
                    _translation = hoverPositionSocket.Position;
                    _segmentPresentation.transform.position = hoverPositionSocket.Position.ToVector3();
                    return;
                }
            }
            
            _segmentPresentation.gameObject.SetActive(false);
        }

        public void SocketUnHovered()
        {
            _segmentPresentation.gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (_segmentGrid == null) return;

            var occupiedPositions = _segmentGrid.GetOccupiedPositions().ToHashSet();
            foreach (var socket in _segmentGrid.GetSockets().OrderByDescending(s => Vector3.SqrMagnitude((s.Position.ToVector3() + s.Direction.Value.ToVector3()) - Camera.main.transform.position)))
            {
                var connectingPosition = socket.Position + socket.Direction;
                if (!occupiedPositions.Contains(connectingPosition))
                {
                    var segment = _segment.Translate(connectingPosition);
                    if (_segmentGrid.Fits(segment))
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(connectingPosition.ToVector3(), 0.3f);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(connectingPosition.ToVector3(), 0.3f);
                    }
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(connectingPosition.ToVector3(), 0.3f);
                }
            }
        }
    }
}