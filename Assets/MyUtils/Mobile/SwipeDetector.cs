using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// A simple class that detects mobile screen swipes.
    /// </summary>
    public class SwipeDetector : MonoBehaviour
    {
        private Vector2 fingerDown;
        private Vector2 fingerUp;

        [Tooltip("If swipe will be detected only after release.")]
        public bool detectSwipeOnlyAfterRelease = false;

        [Tooltip("The minimum distance to be considered a swipe.")]
        public float minDistanceForSwipe = 20f;

        /// <summary>
        /// Here you put methods that are called when swipe is dete
        /// </summary>
        public static List<Action<SwipeData>> OnSwipe = new List<Action<SwipeData>>();

        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects Swipe while finger is still moving
                if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {
                    fingerDown = touch.position;
                    CheckSwipe();
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    CheckSwipe();
                }
            }
        }

        private void CheckSwipe()
        {
            //Check if Vertical swipe
            if (VerticalMove() > minDistanceForSwipe && VerticalMove() >= HorizontalValMove())
            {
                if (fingerDown.y - fingerUp.y > 0)//up swipe
                {
                    TriggerSwipe(Vector2.up);
                }
                else if (fingerDown.y - fingerUp.y < 0)//Down swipe
                {
                    TriggerSwipe(Vector2.down);
                }
                fingerUp = fingerDown;
            }

            //Check if Horizontal swipe
            else if (HorizontalValMove() > minDistanceForSwipe && HorizontalValMove() >= VerticalMove())
            {
                //Debug.Log("Horizontal");
                if (fingerDown.x - fingerUp.x > 0)//Right swipe
                {
                    TriggerSwipe(Vector2.right);
                }
                else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                {
                    TriggerSwipe(Vector2.left);
                }
                fingerUp = fingerDown;
            }
        }

        private float VerticalMove()
        {
            return Mathf.Abs(fingerDown.y - fingerUp.y);
        }

        private float HorizontalValMove()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }

        private void TriggerSwipe(Vector2 direction)
        {
            SwipeData swipeData = new SwipeData(direction, fingerDown, fingerUp);

            for (int i = 0; i < OnSwipe.Count; i++)
            {
                OnSwipe[i].Invoke(swipeData);
            }
        }
    }

    public struct SwipeData
    {
        public Vector2 Direction { get; private set; }
        public Vector2 StartPosition { get; private set; }
        public Vector2 EndPosition { get; private set; }


        public SwipeData(Vector2 direction, Vector2 startPos, Vector2 endPos)
        {
            Direction = direction;
            StartPosition = startPos;
            EndPosition = endPos;
        }
    }
}
