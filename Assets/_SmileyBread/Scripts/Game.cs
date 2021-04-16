using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace _SmileyBread
{
    public class Game : MonoBehaviour
    {
        [SerializeField] static private Game instance;
        public static Game Instance 
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Game>();
                return instance;
            }
        }

        public float startSpeed = 1;
        public float maxSpeed = 10;
        public float accRate = 0.1f;
        public float startTime;

        public static UnityEvent OnGameEnd => Instance.onGameEnd;
        public UnityEvent onGameEnd;

        public static float GameTime => Instance?.gameTime ?? 0;
        public float gameTime => Time.time - startTime;

        public static float Speed => Instance.speed;
        private float speed
        {
            get
            {
                return Mathf.Min(startSpeed + Mathf.Log(gameTime * accRate + 1), maxSpeed);
            }
        }


        private void Awake()
        {
            if (Instance != this)
            {
                Debug.LogWarning("There is already a Game instance.");
            }
        }

        void Start()
        {
            startTime = Time.time;
        }

        public void TriggerDeath()
        {
            OnGameEnd.Invoke();
        }
    }
}