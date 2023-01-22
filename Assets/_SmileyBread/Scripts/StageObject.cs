using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _SmileyBread
{
    public class StageObject : MonoBehaviour
    {
        public Stage stage;
        public virtual void Init(Stage stage)
        {
            if (stage != null)
                this.stage = stage;
        }
    }
}