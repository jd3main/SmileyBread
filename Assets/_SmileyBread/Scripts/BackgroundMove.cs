using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _SmileyBread
{
    public class BackgroundMove : MonoBehaviour
    {
        public float speedMultiplier = 1f;

        void Update()
        {
            transform.position += Vector3.left * Game.Speed * speedMultiplier * Time.deltaTime;
        }
    }
}