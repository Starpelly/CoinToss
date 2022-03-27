using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace EndlessGames
{
    public class DSGuy : MonoBehaviour
    {
        public static DSGuy instance;

        [SerializeField] private GameObject Eyes;
        [SerializeField] private GameObject OuterCircle;
        public GameObject InnerCircle;
        private Tween outerCircleTween, eyesTween, flickTween;

        public Ease easeType;
        private float speed;
        private float flickSpeed = 0.75f;

        public Vector3 velocity;
        Vector3 previous;

        bool isFlicking;
        int flickTimes = 0;

        public new bool enabled;

        private void Start()
        {
            instance = this;
            Flicking.OnFlick += OnFlick;
        }

        private void OnFlick(FlickData obj)
        {
            speed = 0.58f;
            flickTimes = 0;
            isFlicking = true;
            // 15 seems to be the magic number:tm:
        }

        private void Update()
        {
            if (isFlicking)
            {
                velocity = (transform.position - previous) / Time.deltaTime;
                if (flickTimes == 0)
                {
                    flickTween = transform.DOMove(new Vector3(transform.position.x + (velocity.x / 21f), transform.position.y + (velocity.y / 21f)), flickSpeed).SetEase(Ease.OutExpo);
                }
                flickTimes++;
                isFlicking=false;
            }
        }

        private void LateUpdate()
        {
            previous = transform.position;

            if (PlayerInput.Touching())
            {
                var _pos = PlayerInput.TapPosition();
                var pos = Camera.main.ScreenToWorldPoint(_pos);
                pos = new Vector3(pos.x, pos.y, 0);
                transform.position = pos;
            }
            if (PlayerInput.Tapped())
            {
                if (enabled)
                {               
                    speed = 0.35f;

                    flickTween.Kill();
                    InnerCircle.SetActive(true);
                    outerCircleTween.Kill();
                    outerCircleTween = OuterCircle.transform.DOScale(1.85f, speed).SetEase(Ease.OutExpo);

                    Eyes.SetActive(true);
                    eyesTween.Kill();
                    eyesTween = Eyes.transform.DOLocalMoveY(0.96875f, speed).SetEase(Ease.OutExpo);
                }

            }
            else if (PlayerInput.TappedRelease())
            {
                InnerCircle.SetActive(false);
                outerCircleTween.Kill();
                outerCircleTween = OuterCircle.transform.DOScale(0, speed);

                eyesTween.Kill();
                eyesTween = Eyes.transform.DOLocalMoveY(0.125f, speed).OnComplete(delegate { Eyes.SetActive(false); });
            }
        }
    }

}