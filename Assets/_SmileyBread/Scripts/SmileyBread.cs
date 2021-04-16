using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

namespace _SmileyBread
{
    public class SmileyBread : MonoBehaviour
    {
        public enum MoveMode
        {
            Free,
            Rightward,
        }

        public float moveSpeed = 10f;
        public float segmentOffset = 1;
        public float burstSpeed = 2.0f;
        public int initialLength = 1;

        [SerializeField] private GameObject segmentPrefab;

        [SerializeField] private List<GameObject> segments = new List<GameObject>();
        public List<GameObject> Segments => segments;


        public GameObject Head => segments.First();
        public GameObject Tail => segments.Last();

        public int Power => segments.Count - 1;
        public int Length => segments.Count - 1;

        public float RefinedPower { get; private set; }

        public Animator anm;

        private Vector3 moveDirection;

        [SerializeField] private Transform root;

        public MoveMode moveMode = MoveMode.Rightward;

        public bool alive = true;

        [SerializeField] private TextMeshProUGUI lengthText;


        private void Start()
        {
            if (segments.Count == 0)
            {
                segments.Add(this.gameObject);
            }

            moveDirection = Head.transform.right.normalized;

            AddSegments(initialLength);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (alive)
            {
                if (collision.tag == "Bread")
                {
                    Eat(collision.gameObject);
                }
                if (collision.tag == "DeathZone")
                {
                    Death();
                }
            }
        }


        private void Update()
        {
            switch (moveMode)
            {
                case MoveMode.Free:
                    FreeMoveInput();
                    FreeMoveStep();
                    break;
                case MoveMode.Rightward:
                    RightWardMoveInput();
                    RightWardMoveStep();
                    break;
            }

            UpdateText();
        }


        private void FreeMoveInput()
        {
            if (!alive)
                return;

            Vector3 newDirection = Vector3.zero;
            if (InputManager.GetKey(PlayerAction.MoveUp))
            {
                newDirection.y += 1;
            }
            if (InputManager.GetKey(PlayerAction.MoveDown))
            {
                newDirection.y -= 1;
            }
            if (InputManager.GetKey(PlayerAction.MoveRight))
            {
                newDirection.x += 1;
            }
            if (InputManager.GetKey(PlayerAction.MoveLeft))
            {
                newDirection.x -= 1;
            }

            if (newDirection != Vector3.zero)
            {
                moveDirection = newDirection.normalized;
            }
        }

        private void RightWardMoveInput()
        {
            if (!alive)
                return;


            if (InputManager.GetKey(PlayerAction.MoveUp))
            {
                Head.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
            if (InputManager.GetKey(PlayerAction.MoveDown))
            {
                Head.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            }

            // Boost
            if (InputManager.GetKey(PlayerAction.Boost))
            {
                Boost(Time.deltaTime);
            }
            else
            {
                anm.SetBool("Boost", false);
            }
        }

        private void Eat(GameObject target)
        {
            anm.SetTrigger("Eat");
            AddSegment();
            Destroy(target);
        }

        private void AddSegments(int n)
        {
            for (int i = 0; i < n; i++)
            {
                AddSegment();
            }
        }

        private void AddSegment()
        {
            GameObject newSeg = Instantiate(segmentPrefab, root);
            newSeg.transform.position = Tail.transform.position - Tail.transform.right * segmentOffset + Vector3.forward;
            newSeg.transform.rotation = Tail.transform.rotation;
            segments.Add(newSeg);

            RefinedPower++;
        }

        private void FreeMoveStep()
        {
            for (int i = segments.Count - 1; i >= 1; i--)
            {
                Vector2 dir = (segments[i - 1].transform.position - segments[i].transform.position);
                dir.Normalize();
                Vector2 newXY = (Vector2)(segments[i - 1].transform.position) - dir * segmentOffset;
                float z = segments[i].transform.position.z;
                segments[i].transform.position = new Vector3(newXY.x, newXY.y, z);
                segments[i].transform.right = dir;
            }


            Head.transform.right = moveDirection;
            Head.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }


        private void RightWardMoveStep()
        {
            for (int i = 1; i < segments.Count; i++)
            {
                Vector3 dir = segments[i - 1].transform.position - segments[i].transform.position;
                float x = segments[i - 1].transform.position.x - segmentOffset;
                float y = Mathf.Lerp(segments[i].transform.position.y, segments[i - 1].transform.position.y, 0.1f);
                float z = segments[i - 1].transform.position.z + 0.01f;

                segments[i].transform.position = new Vector3(x, y, z);
                segments[i].transform.right = dir.normalized;
            }
        }

        private void Death()
        {
            Game.Instance.TriggerDeath();
            anm.SetTrigger("Death");
        }

        private bool Boost(float dt)
        {
            if (RefinedPower < 1)
            {
                anm.SetBool("Boost", false);
                return false;
            }

            Head.transform.position += Vector3.right * dt * burstSpeed;
            anm.SetBool("Boost", true);

            RefinedPower -= dt;
            if (RefinedPower <= Power)
                DropSegment();

            return true;
        }


        private void DropSegment()
        {
            segments.Last().transform.parent = null;
            segments.Last().GetComponent<Bread>().Drop();
            segments.RemoveAt(segments.Count - 1);
        }

        private void UpdateText()
        {
            lengthText.text = Power.ToString();
        }
    }
}
