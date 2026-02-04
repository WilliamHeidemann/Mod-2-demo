using System.Collections.Generic;
using Core;
using Model;
using UnityEditor;
using UnityEngine;
using UtilityToolkit.Editor;

namespace Presentation
{
    public class Connections : MonoBehaviour
    {
        [SerializeField] private List<ConnectionPoint> points;
        [SerializeField] private List<Vector3Int> cells;
        [SerializeField] private SegmentConfig segmentConfig;

        private static readonly Vector3 WireSize = new(0.5f, 0.5f, 0.5f);

        private void OnDrawGizmos()
        {
            // const int checkRange = 3;
            //
            // Gizmos.color = Color.blueViolet;
            // for (int z = -checkRange; z < checkRange; z++)
            // {
            //     for (int y = -checkRange; y < checkRange; y++)
            //     {
            //         for (int x = -checkRange; x < checkRange; x++)
            //         {
            //             var volumeCenter = new Vector3(x, y, z);
            //             var halfExtents = Vector3.one;
            //             Gizmos.DrawWireCube(volumeCenter, halfExtents);
            //         }
            //     }
            // }

            if (points == null)
            {
                return;
            }

            foreach (var connectionPoint in points)
            {
                Gizmos.color = connectionPoint.Archetype.AsColor();

                var position = transform.position + connectionPoint.Position
                                                  + connectionPoint.Direction.AsVector3() / 2f;

                Gizmos.DrawWireCube(position, WireSize);
            }

            if (cells == null)
            {
                return;
            }

            foreach (var cell in cells)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(cell, new Vector3(0.9f, 0.9f, 0.9f));
            }
        }

        [Button]
        public void Bake()
        {
            DestroyChildren();
            BakeCells();
            BakeColliders();
        }

        [Button]
        public void Write()
        {
            var segmentDefinition = new Segment(cells, points);
            segmentConfig.Write(transform.root.gameObject, segmentDefinition);
        }

        private void DestroyChildren()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        private void BakeCells()
        {
            const int checkRange = 3;

            cells = new List<Vector3Int>();
            var slotMask = LayerMask.GetMask("Slot");

            for (int z = -checkRange; z < checkRange; z++)
            {
                for (int y = -checkRange; y < checkRange; y++)
                {
                    for (int x = -checkRange; x < checkRange; x++)
                    {
                        var volumeCenter = new Vector3(x, y, z);
                        var halfExtents = Vector3.one * 0.5f;
                        var hits = Physics.OverlapBox(volumeCenter, halfExtents, Quaternion.identity, ~slotMask);
                        bool intersects = hits.Length > 0;
                        if (intersects)
                        {
                            print($"Found {hits.Length} hits for {x} {y} {z}");
                            foreach (var cell in hits)
                            {
                                print(cell.name);
                            }

                            cells.Add(new Vector3Int(x, y, z));
                        }
                    }
                }
            }

            EditorUtility.SetDirty(gameObject);
        }

        private void BakeColliders()
        {
            if (points == null)
            {
                return;
            }

            var hoverSlotPrefab = Resources.Load<HoverSlot>("Prefabs/HoverSlot");

            foreach (var connectionPoint in points)
            {
                var slotPosition = connectionPoint.Position + connectionPoint.Direction.AsVector3();
                var hoverSlot = Instantiate(hoverSlotPrefab, slotPosition, Quaternion.identity, transform);
                var newCollider = hoverSlot.GetComponent<BoxCollider>();
                var colliderPosition = connectionPoint.Direction.Opposite().AsVector3() / 2f;
                newCollider.center = colliderPosition;
                // flatten
                newCollider.size -= connectionPoint.Direction.AsVector3().WithAbsoluteValues() * 0.9f;
            }

            EditorUtility.SetDirty(gameObject);
        }
    }
}