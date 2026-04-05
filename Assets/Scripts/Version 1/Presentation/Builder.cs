using System.Collections.Generic;
using System.Linq;
using LitMotion;
using UnityEngine;
using Version_1.Domain;
using LitMotion.Extensions;

namespace Version_1.Presentation
{
    public class Builder
    {
        private readonly Factory _factory;
        private readonly SegmentGrid _grid = new();
        private Segment _current;
        private GameObject _ghost;
        
        public Builder(Interactions interactions, Factory factory)
        {
            _factory = factory;
            
            interactions.OnHoverEnter += SetHover;
            interactions.OnHoverExit += RemoveHover;
            interactions.OnTryBuild += TryBuild;
        }

        public void Select(Segment segment)
        {
            _current = segment;
            _ghost = _factory.SegmentToGameObject(segment);
            _ghost.SetActive(false);
        }

        public void TryBuild(Position position)
        {
            Segment translatedSegment = _current.Translate(position);
            if (_grid.TryAdd(translatedSegment))
            {
                _ghost.SetActive(true);
                BuildSockets(translatedSegment);
                Select(Generator.Generate());
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
            
            Segment translatedSegment = _current.Translate(position);
            List<Segment> validSegments = translatedSegment.GetAllStates().Where(_grid.Fits).ToList();
            if (validSegments.Count > 0)
            {
                Segment validSegment = validSegments[0];
                _current = validSegment;//.Translate(-position);
                Object.Destroy(_ghost);
                _ghost = _factory.SegmentToGameObject(validSegment);
                
                LMotion
                    .Create(_ghost.transform.position, position.ToVector3(), .3f)
                    .WithEase(Ease.OutExpo)
                    .BindToPosition(_ghost.transform);
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
    }
}