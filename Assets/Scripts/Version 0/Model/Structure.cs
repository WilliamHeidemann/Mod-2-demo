using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Model
{
    public class Structure
    {
        private readonly Dictionary<Vector3Int, Segment> _cells = new();

        public void AddSegment(Segment segment, Vector3Int origin)
        {
            foreach (var cell in segment.GetWorldCells(origin))
            {
                _cells[cell] = segment;
            }
        }
        
        public bool IsCellOccupied(Vector3Int pos) => _cells.ContainsKey(pos);
        
        public bool CanPlace(Segment segment, Vector3Int positionToPlace)
        {
            // 1. Check cell occupancy
            foreach (var cell in segment.GetWorldCells(positionToPlace))
            {
                if (IsCellOccupied(cell))
                    return false;
            }

            // 2. Check connection points
            foreach (var connectionPoint in segment.GetWorldConnections(positionToPlace))
            {
                var neighborPosition = connectionPoint.Position + connectionPoint.Direction.AsVector3Int() + positionToPlace;

                if (!_cells.TryGetValue(neighborPosition, out var neighbor))
                    continue;

                var option =
                    neighbor.ConnectionPoints.FirstOption(point => point.Position == neighborPosition);
                if (!option.IsSome(out var neighborConnectionPoint))
                    return false;
                
                if (!Connects(connectionPoint, neighborConnectionPoint))
                    return false;
            }
            
            // 3. Check 6 neighbor connection points of every cell of the new segment
            foreach (var worldCellPosition in segment.GetWorldCells(positionToPlace))
            {
                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    if (direction == Direction.Left)
                    {
                        Debug.Log("Left");
                    }
                    var neighborPosition = worldCellPosition + direction.AsVector3Int();

                    if (!_cells.TryGetValue(neighborPosition, out var neighbor))
                        continue;
                    
                    var option = neighbor.ConnectionPoints.FirstOption(point => neighborPosition + point.Direction.AsVector3Int() == worldCellPosition);
                
                    if (!option.IsSome(out var neighborConnectionPoint))
                        return false;

                    var connects = segment.GetWorldConnections(positionToPlace + worldCellPosition)
                        .Any(cp => Connects(cp, neighborConnectionPoint));
                    if (!connects)
                        return false;
                }
            }

            return true;
        }

        private static bool Connects(ConnectionPoint a, ConnectionPoint b) => 
            a.Archetype == b.Archetype && a.Direction == b.Direction.Opposite();
    }
}