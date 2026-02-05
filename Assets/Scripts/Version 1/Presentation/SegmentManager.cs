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
            var startingSegment = monoSegment.Model;
            _segmentGrid = new SegmentGrid(startingSegment);
            Instantiate(monoSegment);
        }
    }
}