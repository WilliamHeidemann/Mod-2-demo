using System;
using Core;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Presentation
{
    public class HoverSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public static Action<Vector3Int> OnHover;
        public static Action<Vector3Int> OnHoverExit;
        public static Action<Vector3Int> OnClick;

        private Vector3Int CellPosition => transform.position.AsVector3Int();

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHover?.Invoke(CellPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverExit?.Invoke(CellPosition);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(CellPosition);
        }
    }
}