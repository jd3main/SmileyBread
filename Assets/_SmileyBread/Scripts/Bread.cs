using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _SmileyBread
{
    public class Bread : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rigid;
        public float lifeTime = 3;

        private void Start()
        {
            rigid.simulated = false;
        }

        public void Drop()
        {
            rigid.simulated = true;
            rigid.angularVelocity = Random.Range(-3, 3);
            Destroy(this, lifeTime);
        }
    }
}