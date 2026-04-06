using System.Collections.Generic;
using System.Linq;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UIElements;
using Version_1.Domain;
using Version_1.Domain.ExtensionMethods;
using Position = Version_1.Domain.Position;

namespace Version_1.Presentation
{
    public class Builder
    {
        private readonly Factory _factory;
        private readonly SegmentGrid _grid = new();
        private Segment _current;
        private Segment[] _validSegmentStates;
        private int _stateIndex;
        private GameObject _ghost;
        private readonly SDFController _sdfController;
        
        public Builder(Interactions interactions, Factory factory, SDFController sdfController)
        {
            _factory = factory;
            _sdfController = sdfController;

            interactions.OnHoverEnter += SetHover;
            interactions.OnHoverExit += RemoveHover;
            interactions.OnTryBuild += TryBuild;
            interactions.OnRotate += Rotate;
        }

        public void Select(Segment segment)
        {
            _current = segment;
            _ghost = _factory.SegmentToGameObject(segment);
            _ghost.SetActive(false);
        }

        public void TryBuild(Position position)
        {
            Segment translatedSegment = _current.MoveTo(position);
            if (_grid.TryAdd(translatedSegment))
            {
                _ghost.SetActive(true);
                BuildSockets(translatedSegment);
                Select(Generator.Generate());
                _sdfController.Append(translatedSegment.Positions.Select(p => p.ToVector4()).ToArray());
            }
        }

        private void BuildSockets(Segment segment)
        {
            _factory.SegmentToMonoSockets(segment);
        }

        private void SetHover(Position position)
        {
            if (_ghost == null)
            {
                return;
            }
            
            _ghost.SetActive(true);
            
            Segment translatedSegment = _current.MoveTo(position);
            Segment[] validSegments = translatedSegment.GetAllStates().Where(_grid.Fits).ToArray();
            if (validSegments.Length > 0)
            {
                Segment validSegment = validSegments[0];
                _validSegmentStates = validSegments;
                _stateIndex = 0;
                _current = validSegment;
                Object.Destroy(_ghost);
                _ghost = _factory.SegmentToGameObject(validSegment);
                
                // LMotion
                //     .Create(_ghost.transform.position, position.ToVector3(), .3f)
                //     .WithEase(Ease.OutExpo)
                //     .BindToPosition(_ghost.transform);
            }
            else
            {
                Debug.LogWarning("No valid rotations found.");
            }
        }
        
        private void RemoveHover()
        {
            if (_ghost == null)
            {
                return;
            }
            
            // _ghost.SetActive(false);
        }
        
        private void Rotate()
        {
            if (_validSegmentStates == null || 0 == _validSegmentStates.Length)
            {
                return;
            }
            
            _stateIndex = (_stateIndex + 1) % _validSegmentStates.Length;
            _current = _validSegmentStates[_stateIndex];
            Object.Destroy(_ghost);
            _ghost = _factory.SegmentToGameObject(_validSegmentStates[_stateIndex]);
        }
    }
}