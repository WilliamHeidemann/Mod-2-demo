using System;
using Model;
using Presentation;
using Services;
using UnityEngine;

namespace Initialization
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private SegmentConfig segmentConfig;
        private SelectionService _selectionService;
        private HoverService _hoverService;
        private BuildingService _buildingService;
        
        private void OnEnable()
        {
            var structure = new Structure();
            structure.AddSegment(segmentConfig.Segment, Vector3Int.zero);
            _selectionService = new SelectionService();
            _selectionService.Set(segmentConfig);
            _hoverService = new HoverService(structure, _selectionService);
            _buildingService = new BuildingService(structure, _selectionService);
            HoverSlot.OnHover += _hoverService.OnHover;
            HoverSlot.OnClick += _buildingService.Build;
            HoverSlot.OnHoverExit += _hoverService.OnHoverExit;
        }

        private void OnDisable()
        {
            HoverSlot.OnHover -= _hoverService.OnHover;
            HoverSlot.OnClick -= _buildingService.Build;
            HoverSlot.OnHoverExit -= _hoverService.OnHoverExit;
        }
    }
}