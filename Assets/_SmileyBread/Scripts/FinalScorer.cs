using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace _SmileyBread
{
    [ExecuteInEditMode]
    public class FinalScorer : MonoBehaviour
    {
        [SerializeField] private string format;
        [SerializeField] private TextMeshProUGUI textUI;


        private void OnValidate()
        {
            UpdateTextUI();
        }

        void Start()
        {
            UpdateTextUI();
        }

        private void UpdateTextUI()
        {
            float score = FindObjectOfType<Score>().Value;
            textUI.text = string.Format(format, score);
        }
    }
}