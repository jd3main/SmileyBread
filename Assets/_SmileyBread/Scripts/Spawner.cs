using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> items;
    public List<GameObject> Items => items;

    public float rate = 1.0f;

    [SerializeField] Bounds bounds;

    [SerializeField] private int _nextIndex = 0;
    private int nextIndex
    {
        get
        {
            int ret = _nextIndex;
            _nextIndex = (_nextIndex + 1) % items.Count;
            return ret;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.2f, 1.0f, 0.3f);
        Gizmos.DrawCube(bounds.center, bounds.size);
    }

    private void Start()
    {
        InvokeRepeating("Spawn", 0, 1 / rate);
    }

    private void Spawn()
    {
        int i = nextIndex;
        GameObject obj = Instantiate(items[i], this.transform);
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        obj.transform.position = new Vector3(x, y, transform.position.z);
    }
}
