using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// A simple static class that detects touches.
    /// </summary>
    public static class TouchDetector
    {
        public static bool IsTouching(TouchPhase acceptedPhase)
        {
            bool isTouching = false;

            for (int i = 0; i < Input.touchCount; i++)
            {
                isTouching = Input.GetTouch(i).phase == acceptedPhase;
            }

            return isTouching;
        }
    }
}
