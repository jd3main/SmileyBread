using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageInfo : MonoBehaviour
{
    public Vector3 offset;
    public float width;
    public float height = 20;
    public Vector3 center => offset + new Vector3(width/2, 0, 0);
    public float rightBound => offset.x + width;
    public float leftBound => offset.x;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, 1f);
        Gizmos.DrawWireCube(transform.position + center, new Vector3(width, height, 0));
    }

    private void Reset()
    {
        Bounds bounds = GetBounds();
        width = bounds.size.x;
        offset = new Vector3(bounds.min.x, 0, 0);
    }

    private Bounds GetBounds()
    {
        Bounds bounds = new Bounds();
        Transform[] transforms = this.GetComponentsInChildren<Transform>();
        foreach (var trans in transforms)
        {
            bounds.Encapsulate(trans.position);
        }

        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }
}
