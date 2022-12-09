using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _SmileyBread
{
    [ExecuteInEditMode]
    public class StageObjectGenerator : MonoBehaviour
    {
        public int minCount = 5;
        public int maxCount = 20;
        public int increaseFromTime = 0;
        public float secondsToMax = 30;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);
        public List<GameObject> prefabs;
        
        public Stage stage;

        public float count { get; private set; }

        private void Start()
        {
            if (!Application.isPlaying)
            {
                stage = GetComponentInParent<Stage>();
                return;
            }

            float t = Mathf.Max(Mathf.Min((stage.time - increaseFromTime) / secondsToMax, 1), 0);
            count = Mathf.RoundToInt(minCount + (maxCount - minCount) * curve.Evaluate(t));

            Generate();
        }

        private void Generate()
        {
            for (int i = 0; i < count; i++)
            {
                int index = MathUtils.RandInt(0, prefabs.Count);
                GameObject obj = Instantiate(prefabs[index]);
                StageObject stageObject = obj.GetComponent<StageObject>();
                if (stageObject != null)
                {
                    stageObject.Init(stage);
                }
            }
        }
    }
}