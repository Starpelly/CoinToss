using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessGames
{
    // known issues:
    // flicking with multiple fingers calls the OnFlick action multiple times
    // flicking sometimes calls the OnFlick action twice.
    // the flick action is called when you move your finger across the screen and let go, regardless if you actually "flicked" or not.
    public class Flicking : MonoBehaviour
    {
        private Vector2 fingerDownPos;
        private Vector3 lastFingerPos;
        private Vector2 fingerUpPos;
        public bool detectFlickOnlyAfterRelease = false;

        public float minDistanceForFlick = 20f;
        float minMoveSpeed = 8f;

        public static event Action<FlickData> OnFlick = delegate { };

        private void Update()
        {
#if UNITY_ANDROID
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUpPos = touch.position;
                    fingerDownPos = touch.position;
                }

                if (!detectFlickOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {
                    fingerDownPos = touch.position;
                    DetectFlick();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Vector3 mouseDelta = touch.position - (Vector2)lastFingerPos;

                    if (mouseDelta.x < -minMoveSpeed || mouseDelta.x > minMoveSpeed || mouseDelta.y < -minMoveSpeed || mouseDelta.y > minMoveSpeed)
                    {
                        fingerDownPos = touch.position;
                        DetectFlick();
                    }
                }

                lastFingerPos = touch.position;
            }
#else
            if (Input.GetMouseButtonDown(0))
            {
                fingerUpPos = Input.mousePosition;
                fingerDownPos = Input.mousePosition;
            }

            if (!detectFlickOnlyAfterRelease)
            {
                {
                    fingerDownPos = Input.mousePosition;
                    DetectFlick();   
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector3 mouseDelta = Input.mousePosition - lastFingerPos;

                if (mouseDelta.x < -minMoveSpeed || mouseDelta.x > minMoveSpeed || mouseDelta.y < -minMoveSpeed || mouseDelta.y > minMoveSpeed)
                {
                    fingerDownPos = Input.mousePosition;
                    DetectFlick();
                }
            }

            lastFingerPos = Input.mousePosition;
#endif
        }

        private void DetectFlick()
        {
            if (FlickDistanceCheckMet())
            {
                if (IsVerticalFlick())
                {
                    var direction = fingerDownPos.y - fingerUpPos.y > 0 ? FlickDirection.Up : FlickDirection.Down;
                    SendFlick(direction);
                }
                else
                {
                    var direction = fingerDownPos.x - fingerUpPos.x > 0 ? FlickDirection.Right : FlickDirection.Left;
                    SendFlick(direction);
                }
                fingerUpPos = fingerDownPos;
            }
        }

        private bool IsVerticalFlick()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private bool FlickDistanceCheckMet()
        {
            return VerticalMovementDistance() > minDistanceForFlick || HorizontalMovementDistance() > minDistanceForFlick;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
        }

        public void SendFlick(FlickDirection direction)
        {
            FlickData flickData = new FlickData()
            {
                Dir = direction,
                startPos = fingerDownPos,
                endPos = fingerUpPos
            };
            OnFlick(flickData);
        }
    }

    public struct FlickData
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public FlickDirection Dir;
    }

    public enum FlickDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}