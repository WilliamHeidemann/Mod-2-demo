using System;
using UnityEngine;

namespace Version_1.Presentation
{
    public class SegmentManager : MonoBehaviour
    {
        [SerializeField] private MonoSegment monoSegment;
        private SegmentGrid _segmentGrid;
        
        private void Start()
        {
            _segmentGrid = new SegmentGrid();
            var startingSegment = monoSegment.Model;
            _segmentGrid.Add(startingSegment, forceAdd: true);
            Instantiate(monoSegment);
        }

        public void TryBuild(Position position)
        {
            var segment = monoSegment.Model.Translate(position);
            if (_segmentGrid.Fits(segment))
            {
                _segmentGrid.Add(segment);
                Instantiate(monoSegment, position.ToVector3(), Quaternion.identity);
            }
            else
            {
                print($"Failed to build segment {position}!");
            }
        }
    }
}