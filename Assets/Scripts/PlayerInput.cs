using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessGames
{
    public class PlayerInput
    {
        public static bool Tapped()
        {
#if UNITY_ANDROID
            if (IsTouching())
            {
                return Input.touches[0].phase == TouchPhase.Began;   
            }
            else
            {
                return false;
            }
#else
            return Input.GetMouseButtonDown(0);
#endif
        }

        public static bool TappedRelease()
        {
#if UNITY_ANDROID
            if (IsTouching())
            {
                return Input.touches[0].phase == TouchPhase.Ended;   
            }
            else
            {
                return false;
            }
#else
            return Input.GetMouseButtonUp(0);
#endif
        }

        public static bool Touching()
        {
            return IsTouching();
        }

        private static bool IsTouching()
        {
#if UNITY_ANDROID
            return Input.touches.Length > 0;
#else
            return Input.GetMouseButton(0);
#endif
        }

        public static Vector2 TapPosition()
        {
#if UNITY_ANDROID
            if (Input.touches.Length > 0)
            {
                return Input.touches[0].position;
            }
            else
            {
                return new Vector2(0, 0);
            }
#else
            return Input.mousePosition;
#endif
        }
    }
}