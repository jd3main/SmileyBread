using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace _SmileyBread
{
    public class InitAnimatorParameters : MonoBehaviour
    {
        [System.Serializable]
        public class FloatField
        {
            public string name;
            public float value;
        }

        [SerializeField] private List<FloatField> floatFields;


        void Start()
        {
            Animator animator = this.GetComponent<Animator>();

            foreach (var field in floatFields)
            {
                animator.SetFloat(field.name, field.value);
            }

            Destroy(this);
        }
    }
}
