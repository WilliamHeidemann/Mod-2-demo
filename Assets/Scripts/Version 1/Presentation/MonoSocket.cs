using System;
using UnityEngine;
using Version_1;
using Version_1.Presentation;

public class MonoSocket : MonoBehaviour
{
    [SerializeField] private Archetype _archetype;
    [SerializeField] private SerializedDirection _direction;
    public Archetype Archetype => _archetype;
    public Direction Direction => _direction.ToDirection();
    [SerializeField] private BoxCollider _boxCollider;

    public void Initialize(Archetype archetype, SerializedDirection direction)
    {
        _archetype = archetype;
        _direction = direction;
        PositionCollider();
    }
    
    public void EnableCollider() => _boxCollider.enabled = true;
    
    private void OnValidate()
    {
        PositionCollider();
    }

    private void Awake()
    {
        PositionCollider();
    }

    private void PositionCollider() => _boxCollider.center = Direction.Value.ToVector3();

    private void OnDrawGizmos()
    {
        Gizmos.color = _archetype.AsColor();
        Gizmos.DrawLine(transform.position, transform.position + Direction.Value.ToVector3());
        Gizmos.DrawSphere(transform.position + Direction.Value.ToVector3(), 0.1f);
    }
}
