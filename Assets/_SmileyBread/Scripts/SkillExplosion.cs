using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEditor;
using System;

namespace _SmileyBread
{
    public class SkillExplosion : MonoBehaviour
    {
        [SerializeField] private GameObject explosionVideo;
        [SerializeField] private Animator uiAnm;
        [SerializeField] private VideoPlayer videoPlayer;

        [SerializeField] private float videoTime = 1f;

        private bool seekCompleted = false;
        private bool videoStarted = false;
        private bool prepareCompleted = false;
        private bool isAnmEnd = false;

        private void OnValidate()
        {
            if (uiAnm == null)
            {
                uiAnm = explosionVideo.GetComponent<Animator>();
                if (uiAnm == null)
                    Debug.LogWarning("uiAnm no set");
            }

            if (videoPlayer == null)
            {
                videoPlayer = explosionVideo.GetComponent<VideoPlayer>();
                if (videoPlayer == null)
                    Debug.LogWarning("VideoPlayer not set");
            }
        }

        private void Start()
        {
            videoPlayer.frame = 0;
            videoPlayer.started += OnVideoStarted;
            videoPlayer.seekCompleted += OnSeekCompleted;
        }

        void Update()
        {
            if (InputManager.GetKeyDown(PlayerAction.Explode))
            {
                StartCoroutine(Explode());
            }
        }

        private IEnumerator Explode()
        {
            Debug.Log("Explode");
            TimeManager.Freeze();

            videoPlayer.Prepare();
            yield return StartCoroutine(WaitForPrepareCompleted());

            videoPlayer.frame = 0;
            yield return StartCoroutine(WaitForSeekComplete());

            Debug.Log("prepared");
            Debug.Log("Play Video");
            videoPlayer.Play();

            yield return StartCoroutine(WaitForVideoStarted());
            Debug.Log("Play Animation");
            yield return null;
            uiAnm.SetTrigger("Play");


            yield return new WaitForSecondsRealtime(videoTime);
            videoPlayer.Pause();


            yield return StartCoroutine(WaitForAnmEnd());

            TimeManager.Unfreeze();

        }

        private IEnumerator WaitForPrepareCompleted()
        {
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }
        }


        private IEnumerator WaitForSeekComplete()
        {
            while (seekCompleted == false)
            {
                yield return null;
            }
            seekCompleted = false;
        }

        private void OnSeekCompleted(VideoPlayer source)
        {
            Debug.Log("OnSeekCompleted");
            seekCompleted = true;
        }

        private IEnumerator WaitForVideoStarted()
        {
            while (!videoStarted)
            {
                yield return null;
            }
            videoStarted = false;
        }

        private void OnVideoStarted(VideoPlayer source)
        {
            videoStarted = true;
            Debug.Log("OnVideoStarted");
        }

        private IEnumerator WaitForAnmEnd()
        {
            while (!isAnmEnd)
            {
                yield return null;
            }
            isAnmEnd = false;
        }

        public void OnAnmEnd()
        {
            isAnmEnd = true;
        }
    }
}