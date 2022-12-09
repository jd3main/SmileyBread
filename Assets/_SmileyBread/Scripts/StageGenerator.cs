using Malee.List;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _SmileyBread
{
    [System.Serializable]
    public class StageSetting
    {
        public GameObject prefab = null;
        [HideInInspector] public Stage stageInfo;
        public float startTime = 0.0f;
        public float frequencyWeight = 1.0f;

        public StageSetting(GameObject gameObject)
        {
            prefab = gameObject;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (prefab == null)
                return;
            stageInfo = prefab.GetComponent<Stage>();
        }
    }

    [System.Serializable]
    public class StageSettingsList : ReorderableArray<StageSetting> { }

    public class StageGenerator : MonoBehaviour
    {
        public float screenLeftBound = -16;
        public float screenRightBound = 16;

        [Reorderable]
        [SerializeField]
        public StageSettingsList stagesPool = new StageSettingsList();

        private readonly List<Stage> stages = new List<Stage>();

        private float FirstRightBound => stages[0].transform.position.x + stages[0].rightBound;
        private float LastRightBound => stages.Last().transform.position.x + stages.Last().rightBound;


        private void Update()
        {
            GenEnoughStages();
            MoveStages(Time.deltaTime);
            ClearOldStages();
        }

        private void GenEnoughStages()
        {
            if (stages.Count == 0 || LastRightBound < screenRightBound)
            {
                AppendStage();
            }
        }

        private void ClearOldStages()
        {
            while (stages.Count > 0 && FirstRightBound < screenLeftBound)
            {
                Destroy(stages[0].gameObject);
                stages.RemoveAt(0);
            }
        }

        private void MoveStages(float dt)
        {
            float speed = Game.Speed;
            foreach (var stage in stages)
            {
                stage.transform.position += Vector3.left * speed * dt;
            }
        }

        private void AppendStage()
        {
            int i = GetNextStageIndex();
            GameObject newStage = Instantiate(stagesPool[i].prefab, this.transform);
            if (stages.Count == 0)
            {
                newStage.transform.position = new Vector3(screenRightBound, 0, 0) - stagesPool[i].stageInfo.offset;
            }
            else
            {
                newStage.transform.position = new Vector3(LastRightBound, 0, 0) - stagesPool[i].stageInfo.offset;
            }
            stages.Add(newStage.GetComponent<Stage>());
        }

        private int GetNextStageIndex()
        {
            float totalWeight = stagesPool.Sum(ss => ss.frequencyWeight);
            float rndValue = Random.Range(0, totalWeight);
            int index = -1;
            float weightPrefixSum = 0;
            while (weightPrefixSum < rndValue)
            {
                index++;
                weightPrefixSum += stagesPool[index].frequencyWeight;
            }
            return index;
        }
    }
}