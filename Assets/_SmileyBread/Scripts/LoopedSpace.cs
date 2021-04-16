using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace _SmileyBread
{
    // Manage child objects in a repeating space.
    // Children of children won't be modified directly.
    [ExecuteInEditMode]
    public class LoopedSpace : MonoBehaviour
    {
        public Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 10);

        // Determind how many copies should be generated to simulate the looping nature.
        // There will be (x*2+1)*(y*2+1)*(z*2+1) copies in the scene including the original one.
        // For example, if nCopies.x = 2, then each object will have 5 copies, 2 at the right, 2 at the left, and the other is the original one.
        [SerializeField] private Vector3Int nCopies = new Vector3Int(1,1,0);


        private Dictionary<Vector3Int, HashSet<Transform>> transforms;
        private HashSet<Transform> mainTransforms => transforms[Vector3Int.zero];

        // The second index should be instance ID of an object in main block
        private Dictionary<Vector3Int, Dictionary<int, Transform>> copies;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.2f, 1.0f, 0.3f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }

        private void Start()
        {
            MakeCopies();
        }

        void Update()
        {
            int n = transform.childCount;
            HashSet<Transform> newTransforms = new HashSet<Transform>(transform.Cast<Transform>());


        }

        void MakeCopies()
        {
            copies = new Dictionary<Vector3Int, Dictionary<int, Transform>>();

            for (int xid = -nCopies.x; xid <= nCopies.x; xid++)
            {
                for (int yid = -nCopies.y; yid <= nCopies.y; yid++)
                {
                    for (int zid = -nCopies.z; zid <= nCopies.z; zid++)
                    {
                        if (xid == 0 && yid == 0 && zid == 0)
                            continue;
                        Vector3Int index = new Vector3Int(xid, yid, zid);
                        Transform dupRoot = Instantiate(this.gameObject, this.transform.parent).transform;
                        copies[index] = new Dictionary<int, Transform>();
                        for (int i = 0; i < dupRoot.childCount; i++)
                        {
                            Transform mainTrans = transform.GetChild(i);
                            Transform dupTrans = dupRoot.GetChild(i);
                            copies[index][mainTrans.GetInstanceID()] = dupTrans;
                        }
                        transforms[index] = ChildrenAsSet(dupRoot);
                    }
                }
            }
        }

        void UpdateTransforms()
        {

        }


        private void UpdateLoop()
        {
            foreach (Transform trans in mainTransforms)
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

        private static HashSet<Transform> ChildrenAsSet(Transform trans)
        {
            return new HashSet<Transform>(trans.Cast<Transform>());
        }
    }
}