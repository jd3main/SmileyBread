using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityExtensions
{
    public static class TransformExtension
    {
        public static IEnumerable<Transform> Children(this Transform transform)
        {
            return transform.Cast<Transform>();
        }
    }
}