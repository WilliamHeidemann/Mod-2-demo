using Core;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public class BuildingService
    {
        private readonly Structure _structure;
        private readonly SelectionService _selectionService;

        public BuildingService(Structure structure, SelectionService selectionService)
        {
            _structure = structure;
            _selectionService = selectionService;
        }

        public void Build(Vector3Int position)
        {
            if (!_selectionService.Segment.IsSome(out var segmentConfig))
            {
                return;
            }

            if (!_structure.CanPlace(segmentConfig.Segment, position))
            {
                return;
            }
            
            _structure.AddSegment(segmentConfig.Segment, position);
            
            var hoverObject = Object.Instantiate(segmentConfig.Prefab, position, Quaternion.identity);
        }
        
        private void ScreenShake()
        {
        }
    }
}