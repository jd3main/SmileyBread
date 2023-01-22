using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityExtensions;

namespace _SmileyBread
{
    // Manage child objects in a repeating space.
    // Children of children won't be modified directly.
    public class LoopedSpace : MonoBehaviour
    {
        private class Replica
        {
            public Dictionary<int, Transform> transforms;
            public Transform root;
            public Vector3 offset;

            public Replica(Transform baseRoot, Vector3 offset, string name=null)
            {
                root = new GameObject().transform;
                root.parent = baseRoot.parent;
                if (name != "")
                    root.name = name;

                LoopedSpace dupLoopSpace = root.GetComponent<LoopedSpace>();
                if (dupLoopSpace != null)
                    DestroyImmediate(dupLoopSpace);

                transforms = new Dictionary<int, Transform>();
                Track(baseRoot.Children());

                this.offset = offset;
            }

            public Transform GetCopyOf(Transform trans)
            {
                return GetCopyOf(trans.GetInstanceID());
            }

            public Transform GetCopyOf(int instanceID)
            {
                if (!transforms.ContainsKey(instanceID))
                    return null;
                return transforms[instanceID];
            }

            public void RemoveCopyOf(Transform transform)
            {
                int instanceID = transform.GetInstanceID();
                if (HasCopy(instanceID))
                {
                    if (transforms[instanceID] != null)
                        Destroy(transforms[instanceID]);
                    transforms.Remove(instanceID);
                }
            }

            public void Track(IEnumerable<Transform> baseTransforms)
            {
                foreach (var trans in baseTransforms)
                {
                    int instanceID = trans.GetInstanceID();
                    if (!HasCopy(instanceID))
                        MakeCopyOf(trans);
                    transforms[instanceID].position = trans.position + offset;
                    transforms[instanceID].rotation = trans.rotation;
                    transforms[instanceID].localScale = trans.localScale;
                }
            }

            public bool HasCopy(int instanceID)
            {
                return transforms.ContainsKey(instanceID);
            }

            private void MakeCopyOf(Transform transform)
            {
                int instanceID = transform.GetInstanceID();
                transforms[instanceID] = Instantiate(transform, root);
            }
        }

        public Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 10);

        // Determind how many copies should be generated to simulate the looping nature.
        // There will be (x*2+1)*(y*2+1)*(z*2+1) copies in the scene including the original one.
        // For example, if nCopies.x = 2, then each object will have 5 copies, 2 at the right, 2 at the left, and the other is the original one.
        [SerializeField] private Vector3Int nCopies = new Vector3Int(1,1,0);

        private IEnumerable<Transform> lastTransforms = null;
        
        private Dictionary<Vector3Int,Replica> replicas;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.2f, 1.0f, 0.3f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }

        private void Start()
        {
            InitReplicas();
            lastTransforms = this.transform.Children();
        }

        void InitReplicas()
        {
            replicas = new Dictionary<Vector3Int,Replica>();

            for (int ix = -nCopies.x; ix <= nCopies.x; ix++)
            {
                for (int iy = -nCopies.y; iy <= nCopies.y; iy++)
                {
                    for (int iz = -nCopies.z; iz <= nCopies.z; iz++)
                    {
                        if (ix == 0 && iy == 0 && iz == 0)
                            continue;
                        Vector3Int index = new Vector3Int(ix, iy, iz);
                        Vector3 offset = Vector3.Scale(bounds.size, new Vector3(ix, iy, iz));
                        replicas[index] = new Replica(this.transform, offset, index.ToString());
                    }
                }
            }
        }

        void Update()
        {
            Modular();
            SyncTransforms();
        }

        public void SyncTransforms()
        {
            // Remove destroyed transforms
            foreach (var trans in lastTransforms)
            {
                if (trans == null)
                {
                    RemoveAllCopiesOf(trans);
                }
            }

            // Add transforms
            IEnumerable<Transform> newTransforms = this.transform.Children();
            foreach (var replica in replicas.Values)
            {
                replica.Track(newTransforms);
            }

            lastTransforms = newTransforms;
        }

        private void RemoveAllCopiesOf(Transform trans)
        {
            foreach (var replica in replicas.Values)
            {
                replica.RemoveCopyOf(trans);
            }
        }

        public void Modular()
        {
            foreach (Transform trans in transform.Children())
            {
                if (!bounds.Contains(trans.position))
                {
                    Vector3 offset = trans.position - bounds.min;
                    offset.x = Mathf.Repeat(offset.x, bounds.size.x);
                    offset.y = Mathf.Repeat(offset.y, bounds.size.y);
                    offset.z = Mathf.Repeat(offset.z, bounds.size.z);
                    trans.position = bounds.min + offset;
                }
            }
        }
    }
}