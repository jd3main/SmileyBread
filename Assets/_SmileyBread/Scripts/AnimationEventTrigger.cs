using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _SmileyBread
{
    public class AnimationEventTrigger : MonoBehaviour
    {
        public new string name;
        public UnityEvent onTrigger;

        public void TriggerEvent()
        {
            onTrigger.Invoke();
        }
    }
}