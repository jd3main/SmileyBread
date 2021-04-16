using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace _SmileyBread
{
    [ExecuteInEditMode]
    public class Score : MonoBehaviour
    {
        public string format;
        public const float UnitFox = 16 * 60;
        public bool final = false;


        public float Value => value;
        [SerializeField] private float value;

        [SerializeField] private TextMeshProUGUI textUI;


        private void OnValidate()
        {
            UpdateTextUI();
        }

        private void Start()
        {
            Game.OnGameEnd.AddListener(StopCountingScore);
        }

        private void Update()
        {
            if (!final)
            {
                UpdateValue();
                UpdateTextUI();
            }
        }

        private void UpdateValue()
        {
            value = Game.GameTime / UnitFox;
        }

        private void UpdateTextUI()
        {
            textUI.text = string.Format(format, value);
        }

        private void StopCountingScore()
        {
            final = true;
        }
    }
}
