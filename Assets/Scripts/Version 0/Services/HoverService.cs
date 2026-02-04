using Model;
using UnityEngine;
using UtilityToolkit.Runtime;
using Object = UnityEngine.Object;

namespace Services
{
    public class HoverService
    {
        private readonly Structure _structure;
        private readonly SelectionService _selectionService;
        private Option<GameObject> _hoverObject;

        public HoverService(Structure structure, SelectionService selectionService)
        {
            _structure = structure;
            _selectionService = selectionService;
        }

        public void OnHover(Vector3Int position)
        {
            RemoveHoverObject();
            
            if (!_selectionService.Segment.IsSome(out var segmentConfig))
            {
                return;
            }

            if (!_structure.CanPlace(segmentConfig.Segment, position))
            {
                return;
            }

            var hoverObject = CreateHoverObject(segmentConfig.Prefab, position);

            _hoverObject = Option<GameObject>.Some(hoverObject);
        }

        private static GameObject CreateHoverObject(GameObject prefab, Vector3Int position)
        {
            var hoverObject = Object.Instantiate(prefab, position, Quaternion.identity);
            var colliders = hoverObject.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }

            return hoverObject;
        }

        public void OnHoverExit(Vector3Int position)
        {
            RemoveHoverObject();
        }

        private void RemoveHoverObject()
        {
            if (!_hoverObject.IsSome(out var hoverObject))
            {
                return;
            }
            
            Object.Destroy(hoverObject);
            _hoverObject = Option<GameObject>.None;
        }
    }
}