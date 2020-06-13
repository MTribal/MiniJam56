using System.Collections;
using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// An easy way of make cool fade and vibrant color effects.
    /// </summary>
    [ExecuteAlways]
    public class ColorFade : MonoBehaviour
    {
        public enum FadeType { Loop, Once };
        public enum ColorEffectType { Smooth, Vibrant };

        [Tooltip("Mark to simulate the effect in the editor.")]
        public bool simulate;
        [Space]
        [Space]

        [Tooltip("Colors that will change one to another in order. Only used if not looping all colors.")]
        public Color[] colorsToChange = new Color[2];
        [Space]

        // Start Mode
        [Tooltip("The mode that effect will start, by trigger or by seconds. (Trigger need be activated by code, calling 'StartEffect')")]
        public StartStopMode startMode = StartStopMode.Seconds;
        [Tooltip("Mark to start the effect when a AdvancedTrigger be triggered.")]
        [ConditionalShow("startMode", StartStopMode.Trigger)]
        public bool waitTriggerToStart;
        [Tooltip("Effect will be activated when this trigger has been triggered.")]
        [ConditionalShow("startMode", StartStopMode.Trigger, "waitTriggerToStart", true)]
        public AdvancedTrigger triggerToStart;

        [Tooltip("Time in seconds until start effect.")]
        [ConditionalShow("startMode", StartStopMode.Seconds)]
        public float timeToStart;
        [Space]

        // Stop Mode
        [Tooltip("Mode that effect will stop. (Trigger need be activated by code, calling 'StopEffect')")]
        public StartStopMode stopMode = StartStopMode.Trigger;
        [Tooltip("Mark to stop the effect when a AdvancedTrigger be triggered.")]
        [ConditionalShow("stopMode", StartStopMode.Trigger)]
        public bool waitTriggerToStop;
        [Tooltip("Effect will be stoped when this trigger has been triggered.")]
        [ConditionalShow("stopMode", StartStopMode.Trigger, "waitTriggerToStop", true)]
        public AdvancedTrigger triggerToStop;

        [Tooltip("Time in seconds until effect stop.")]
        [ConditionalShow("stopMode", StartStopMode.Seconds)]
        public float timeToStop;
        [Space]

        [Tooltip("Type of color fade effect.")]
        public FadeType fadeType;
        [Tooltip("Mark if you want to change a color group component. ('Smart' will get what isn't null. Don't use it if a gameObject has more than one renderer).")]
        public RendererType rendererType;
        [Tooltip("The type of color effect.")]
        public ColorEffectType effectType;

        [Tooltip("Is the time between one loop effect and the next loop effect.")]
        [ConditionalShow("fadeType", FadeType.Loop, "effectType", ColorEffectType.Vibrant)]
        public float timeBetweenLoops = 0;

        [Tooltip("Mark if you want all colors looping. If you want especif colors looping, don't mark it.")]
        [ConditionalShow("effectType", ColorEffectType.Smooth, "fadeType", FadeType.Loop)]
        public bool loopAllColors;

        [Space]

        // Random Colors
        [Tooltip("Mark if you want the first color to be random.")]
        public bool randomFirstColor;

        // Fade Effect
        [Tooltip("Determines the smoothness of the transition. Smaller values are smoother.")]
        [ConditionalShow("effectType", ColorEffectType.Smooth)]
        public float smootheness = 0.02f;

        [ConditionalShow("effectType", ColorEffectType.Smooth)]
        [Tooltip("Duration of a transition in seconds.")]
        public float duration = 1;

        // Vibrant Effect
        [Tooltip("Smaller value are faster. It's really the time between color updates.")]
        [ConditionalShow("effectType", ColorEffectType.Vibrant)]
        public float vibrantSpeed = 0.05f;

        private GenericRenderer genericRenderer;

        private Color colorBeforeEffect; // Stores how color was before effect starts
        private Color currentColor; // Stores the current color state
        private Color[] rgb;
        private int atualColorIndex = 0;

        private bool hasStarted;
        private bool isSimulating;
        private Color colorBeforeSimulate;

        private void Start()
        {
            genericRenderer = transform.GetAllRenderers(-1)[0];

            // Get colorBeforeEffect
            colorBeforeEffect = genericRenderer.Color;

            rgb = new Color[3] { Color.red, Color.green, Color.blue };
            rgb.Shuffle();

            if (Application.isPlaying)
            {
                if (isSimulating)
                {
                    FinishSimulation();
                }

            }
            if (startMode == StartStopMode.Seconds)
            {
                Invoke("StartEffect", timeToStart);
            }

            if (stopMode == StartStopMode.Seconds)
            {
                Invoke("StopEffect", timeToStop);
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                EditorUpdate();
            }
            if (isSimulating || Application.isPlaying)
            {
                if (hasStarted)
                {
                    AtualizeColor();
                }
            }

            WaitTriggers();
        }

        private void WaitTriggers()
        {
            // Wait trigger to start
            if (!hasStarted && waitTriggerToStart && Application.isPlaying && triggerToStart != null && startMode == StartStopMode.Trigger)
            {
                if (triggerToStart.hasTriggered)
                {
                    StartEffect();
                }
            }

            if (hasStarted && waitTriggerToStop && Application.isPlaying && triggerToStop != null && stopMode == StartStopMode.Trigger)
            {
                if (triggerToStop.hasTriggered)
                {
                    StopEffect();
                }
            }
        }

        private void EditorUpdate()
        {
            if (simulate && !isSimulating)
            {
                StartSimulation();
            }
            if (!simulate && isSimulating)
            {
                FinishSimulation();
            }
        }

        private void StartSimulation()
        {
            isSimulating = true;
            Start();
            colorBeforeSimulate = genericRenderer.Color;
            StartEffect();
        }

        private void FinishSimulation()
        {
            isSimulating = false;
            hasStarted = false;
            atualColorIndex = 0;
            StopAllCoroutines();
            genericRenderer.Color = colorBeforeSimulate;
        }

        public void StartEffect()
        {
            if (!hasStarted)
            {
                hasStarted = true;
                currentColor = GetStartColor();
                if (effectType == ColorEffectType.Vibrant)
                {
                    StartCoroutine(VibrantEffect());
                }
                else if (effectType == ColorEffectType.Smooth)
                {
                    StartCoroutine(SmoothEffect());
                }
            }
        }

        public void StopEffect()
        {
            atualColorIndex = 0;
            StopAllCoroutines();
            FinishEffect();
        }

        private Color GetStartColor()
        {
            Color startColor;

            if (loopAllColors)
            {
                if (randomFirstColor)
                {
                    startColor = new Color(MyRandom.NextFloat(), MyRandom.NextFloat(), MyRandom.NextFloat(), 1);
                }
                else
                {
                    startColor = genericRenderer.Color;
                }
            }
            else
            {
                if (colorsToChange.Length == 0)
                {
                    Debug.LogError("ColorsToChange need be greather than 1");
                }

                startColor = colorsToChange[0];
                atualColorIndex = 1;
            }

            return startColor;
        }

        private void AtualizeColor()
        {
            genericRenderer.Color = currentColor;
        }

        private IEnumerator VibrantEffect()
        {
            for (float r = 0; r <= 1; r += 0.392f)
            {
                for (float g = 0; g <= 1; g += 0.39f)
                {
                    for (float b = 0; b <= 1; b += 0.39f)
                    {
                        float[] basicColors = new float[3] { r, g, b };
                        basicColors.Shuffle();

                        Color nextColor = new Color(basicColors[0], basicColors[1], basicColors[2], 1f);
                        currentColor = nextColor;

                        yield return new WaitForSeconds(vibrantSpeed);
                    }
                }
            }

            if (fadeType == FadeType.Loop)
            {
                yield return new WaitForSeconds(timeBetweenLoops);
                StartCoroutine(VibrantEffect());
            }
            else
            {
                FinishEffect();
            }
        }

        private IEnumerator SmoothEffect()
        {
            Color startColor = currentColor;
            Color targetColor;

            if (loopAllColors)
            {
                targetColor = rgb[atualColorIndex];
                atualColorIndex = atualColorIndex >= 2 ? 0 : atualColorIndex + 1;
            }
            else
            {
                targetColor = colorsToChange[atualColorIndex];
                atualColorIndex = atualColorIndex >= colorsToChange.Length - 1 ? 0 : atualColorIndex + 1;
            }

            float progress = 0; // This will work as the 3rd parameter of lerp function
            float increment = smootheness / duration;

            while (progress < 1)
            {
                currentColor = Color.Lerp(startColor, targetColor, progress);
                progress += increment;

                yield return new WaitForSeconds(smootheness);
            }

            if (fadeType == FadeType.Loop)
            {
                StartCoroutine(SmoothEffect());
            }
            else
            {
                FinishEffect();
            }
        }

        private void FinishEffect()
        {
            currentColor = colorBeforeEffect;
            if (simulate && !Application.isPlaying)
            {
                simulate = false;
                FinishSimulation();
            }
        }
    }
}
