using UnityEngine;

namespace My_Utils
{
    public enum EaseType
    {
        Linear,
        Exponential,
        Sinerp,
        Coserp,
        SmoothStep,
        SmootherStep,
        Hermite,
        Berp,
        Clerp,
        Bounce
    }

    /// <summary>
    /// A easy way to tween values.
    /// </summary>
    public static class MyLerp
    {
        public static float Lerp(float start, float end, float percent, EaseType easeType)
        {
            percent = Mathf.Clamp01(percent);

            float result = 0;
            switch (easeType)
            {
                case EaseType.Linear:
                    result = Linear(start, end, percent);
                    break;
                case EaseType.Exponential:
                    result = Exponential(start, end, percent);
                    break;
                case EaseType.Sinerp:
                    result = Sinerp(start, end, percent);
                    break;
                case EaseType.Coserp:
                    result = Coserp(start, end, percent);
                    break;
                case EaseType.SmoothStep:
                    result = SmoothStep(start, end, percent);
                    break;
                case EaseType.SmootherStep:
                    result = SmootherStep(start, end, percent);
                    break;
                case EaseType.Hermite:
                    result = Hermite(start, end, percent);
                    break;
                case EaseType.Berp:
                    result = Berp(start, end, percent);
                    break;
                case EaseType.Clerp:
                    result = Clerp(start, end, percent);
                    break;
                case EaseType.Bounce:
                    result = Mathf.Lerp(start, end, MyMath.Bounce(percent));
                    break;
            }

            return result;
        }

        public static Vector2 Lerp(Vector2 start, Vector2 end, float value, EaseType easeType)
        {
            return new Vector2(Lerp(start.x, end.x, value, easeType), Lerp(start.y, end.y, value, easeType));
        }

        public static Vector3 Lerp(Vector3 start, Vector3 end, float value, EaseType easeType)
        {
            return new Vector3(Lerp(start.x, end.x, value, easeType), Lerp(start.y, end.y, value, easeType), Lerp(start.z, end.z, value, easeType));
        }

        public static Color Lerp(Color start, Color end, float value, EaseType easeType)
        {
            return new Color(Lerp(start.r, end.r, value, easeType), Lerp(start.g, end.g, value, easeType),
                             Lerp(start.b, end.b, value, easeType), Lerp(start.a, end.a, value, easeType));
        }

        #region Lerps
        //Ease in out
        private static float Linear(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value);
        }

        private static float Hermite(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
        }

        //Ease out
        private static float Sinerp(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
        }

        //Ease in
        private static float Coserp(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
        }

        private static float Exponential(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, Mathf.Pow(value, 2));
        }

        private static float SmootherStep(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value * value * value * (value * (6f * value - 15f) + 10f));
        }

        //Boing
        private static float Berp(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        private static float SmoothStep(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, -2 * value * value * value + 3 * value * value);
        }

        private static float Clerp(float start, float end, float value)
        {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min) / 2.0f);//half the distance between min and max
            float retval;
            float diff;

            if ((end - start) < -half)
            {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;

            // Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
            return retval;
        }

        #endregion
    }
}
