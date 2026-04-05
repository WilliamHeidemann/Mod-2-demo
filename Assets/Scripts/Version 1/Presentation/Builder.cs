using UnityEngine;
using Version_1.Domain;

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
            _ghost.transform.position = position.ToVector3();
        }
        
        private void RemoveHover()
        {
            if (_ghost == null)
            {
                return;
            }
            
            _ghost.SetActive(false);
        }
    }
}