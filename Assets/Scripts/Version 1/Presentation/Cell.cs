using System;
using UnityEngine;

/// <summary>
/// Tag for MonoSegment to know which positions are occupied
/// </summary>
public class Cell : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
