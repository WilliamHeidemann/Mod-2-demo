using System;
using UnityEngine;

namespace Version_1.Presentation
{
    public class SegmentManager : MonoBehaviour
    {
        [SerializeField] private MonoSegment monoSegment;
        private SegmentGrid _segmentGrid;

        private void Awake()
        {
            _segmentGrid = new SegmentGrid();
            var startingSegment = monoSegment.Model;
            _segmentGrid.Add(startingSegment, forceAdd: true);
            Instantiate(monoSegment);
        }
    }
}