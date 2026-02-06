using System;
using UnityEngine;
using UnityEngine.Serialization;
using Version_1;
using Version_1.Presentation;

public class MonoSocket : MonoBehaviour
{
    [SerializeField] private Archetype _archetype;
    [SerializeField] private SerializedDirection _direction;
    public Archetype Archetype => _archetype;
    public Direction Direction => ToDirection(_direction);
    [SerializeField] private BoxCollider _boxCollider;

    private void OnValidate()
    {
        _boxCollider.center = Direction.Value.ToVector3();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _archetype.AsColor();
        Gizmos.DrawLine(transform.position, transform.position + Direction.Value.ToVector3());
        Gizmos.DrawSphere(transform.position + Direction.Value.ToVector3(), 0.1f);
    }

    private enum SerializedDirection
    {
        Left,
        Right,
        Up,
        Down,
        Front,
        Back
    }

    private static Direction ToDirection(SerializedDirection direction)
    {
        return direction switch
        {
            SerializedDirection.Left => Direction.Left,
            SerializedDirection.Right => Direction.Right,
            SerializedDirection.Up => Direction.Up,
            SerializedDirection.Down => Direction.Down,
            SerializedDirection.Front => Direction.Front,
            SerializedDirection.Back => Direction.Back,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
