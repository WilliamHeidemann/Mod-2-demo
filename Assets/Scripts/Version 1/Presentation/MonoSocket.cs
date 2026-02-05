using System;
using UnityEngine;
using Version_1;
using Version_1.Presentation;

public class MonoSocket : MonoBehaviour
{
    public Archetype archetype;

    private void Awake()
    {
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = transform.InverseTransformPoint(transform.position + transform.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = archetype.AsColor();
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.DrawSphere(transform.position + transform.forward, 0.1f);
    }
}
