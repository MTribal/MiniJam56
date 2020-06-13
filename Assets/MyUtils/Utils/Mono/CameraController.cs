using System.Collections;
using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// In wich axis camera will follow.
    /// </summary>
    public enum FollowMode { X, Y, XandY };

    /// <summary>
    /// A enum of possible axis.
    /// </summary>
    public enum Axis { X, Y, XandY, None };

    /// <summary>
    /// A enum of the possible update modes.
    /// </summary>
    public enum UpdateMode { Update, LateUpdate, FixedUpdate };

    /// <summary>
    /// An easy way of make camera follow some object.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Tooltip("What camera will follow. Needs a Rigibody2D.")]
        public Transform follow;

        [Tooltip("Axis to follow.")]
        public FollowMode followMode;

        [Space]

        [Tooltip("When the position will be updated. If your follow movement happens in Update function, mark Update. The same applies to FixedUpdate.")]
        public UpdateMode updateMode;
        [Tooltip("Time that will take to get in target. (Recommended = 0.1f)")]
        public float smoothTime = 0.1f;
        /// <summary>
        /// Variable that stores currentVelocity of SmoothDamp function
        /// </summary>
        private Vector3 velocity = Vector3.zero;

        [Space]
        [Tooltip("Value that are added to camera position.")]
        public Vector3 offset;

        [Header("Confiner")]
        [Tooltip("If X position will be clamped.")]
        public bool confineX;
        [Tooltip("If Y position will be clamped.")]
        public bool confineY;

        [Space]
        [ConditionalShow("confineX", true)]
        [Tooltip("Min X pos that camera can go.")]
        public float minX;
        [Tooltip("Max X pos that camera can go.")]
        [ConditionalShow("confineX", true)]
        public float maxX;

        [ConditionalShow("confineY", true)]
        [Tooltip("Max Y pos that camera can go.")]
        public float maxY;
        [Tooltip("Min Y pos that camera can go.")]
        [ConditionalShow("confineY", true)]
        public float minY;

        [Header("DeadZone Y")]
        [Tooltip("Deadzone is a space where camera will not follow. (pos[0] == up, pos[1] == down)")]
        public float[] deadZoneBias = new float[2];

        private void Update()
        {
            if (updateMode == UpdateMode.Update)
            {
                AtualizePosition();
            }
        }

        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FixedUpdate)
            {
                AtualizePosition();
            }
        }

        private void LateUpdate()
        {
            if (updateMode == UpdateMode.LateUpdate)
            {
                AtualizePosition();
            }
        }

        /// <summary>
        /// Atualize camera position
        /// </summary>
        private void AtualizePosition()
        {
            if (follow != null)
            {
                Vector2 desiredPosition = GetDesiredPosition();
                Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
                transform.position = smoothedPosition;
            }
        }

        /// <summary>
        /// Get the next position and returns.
        /// </summary>
        private Vector2 GetDesiredPosition()
        {
            Vector2 desiredPos;
            switch (followMode)
            {
                case FollowMode.X:
                    desiredPos = new Vector2(follow.position.x, transform.position.y);
                    break;
                case FollowMode.Y:
                    desiredPos = new Vector2(transform.position.x, follow.position.y);
                    break;
                default: // FollowMode.XandY
                    desiredPos = new Vector2(follow.position.x, follow.position.y);
                    break;
            }

            Vector2 unclampedPos = desiredPos + (Vector2)offset;

            float clampedX = Mathf.Clamp(desiredPos.x + offset.x, minX, maxX);
            float clampedY;
            if (IsInDeadZoneY())
            {
                clampedY = transform.position.y + offset.y;
            }
            else
            {
                clampedY = Mathf.Clamp(desiredPos.y + offset.y, minY, maxY);
            }

            if (confineX && confineY)
            {
                return new Vector2(clampedX, clampedY);
            }
            else if (confineX)
            {
                return new Vector2(clampedX, unclampedPos.y);
            }
            else if (confineY)
            {
                return new Vector2(unclampedPos.x, clampedY);
            }
            else // Not confines
            {
                return unclampedPos;
            }

        }

        private bool IsInDeadZoneY()
        {
            return MyMath.IsBetween(follow.position.y, transform.position.y + deadZoneBias[0], transform.position.y - deadZoneBias[1]);
        }

        /// <summary>
        /// Tween confiner to a new value.
        /// </summary>
        /// <param name="axis">Confiner axis that will be changed.</param>
        /// <param name="newMin">The new min value of that axis in confiner.</param>
        /// <param name="newMax">The new max value of that axis in confiner.</param>
        /// <param name="duration">How much time this tween will durate.</param>
        /// <param name="easeType">The curve that tween will do.</param>
        public void SetConfiner(Axis axis, float newMin, float newMax, float duration, EaseType easeType)
        {
            StartCoroutine(Confining(axis, newMin, newMax, duration, easeType));
        }

        private IEnumerator Confining(Axis axis, float newMin, float newMax, float duration, EaseType easeType)
        {
            float startMin = 0;
            float startMax = 0;
            switch (axis)
            {
                case Axis.X:
                    startMin = minX;
                    startMax = maxX;
                    break;

                case Axis.Y:
                    startMin = minY;
                    startMax = maxY;
                    break;

            }

            if (duration > 0)
            {
                float percent = 0f;
                while (percent <= 1)
                {
                    percent += Time.deltaTime / duration;

                    AtualizeConfiner(axis, MyLerp.Lerp(startMin, newMin, percent, easeType), MyLerp.Lerp(startMax, newMax, percent, easeType));

                    yield return null;
                }
            }
            else
            {
                AtualizeConfiner(axis, MyLerp.Lerp(startMin, newMin, 1, easeType), MyLerp.Lerp(startMax, newMax, 1, easeType));
            }
        }

        /// <summary>
        /// Atualize confiner based on the new values.
        /// </summary>
        /// <param name="axis">Confiner axis that will be changed.</param>
        /// <param name="newMin">The new min value of that axis in confiner.</param>
        /// <param name="newMax">The new max value of that axis in confiner.</param>
        private void AtualizeConfiner(Axis axis, float newMin, float newMax)
        {
            switch (axis)
            {
                case Axis.X:
                    minX = newMin;
                    maxX = newMax;
                    break;

                case Axis.Y:
                    minY = newMin;
                    maxY = newMax;
                    break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            // Draw dead zone Y
            Vector2 from = new Vector3(transform.position.x, transform.position.y + deadZoneBias[0]);
            Vector2 to = new Vector3(transform.position.x, transform.position.y - deadZoneBias[1]);

            Gizmos.DrawLine(from, to);
        }
    }
}
