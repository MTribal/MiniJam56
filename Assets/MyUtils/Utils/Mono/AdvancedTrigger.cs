using UnityEngine;
using UnityEngine.Events;

namespace My_Utils
{
    /// <summary>
    /// How something will be triggered, by a trigger collider of a function.
    /// </summary>
    public enum TriggerType { TriggerCollider, Function };

    /// <summary>
    /// A way of trigger something by inspector, withou touch code.
    /// </summary>
    public class AdvancedTrigger : MonoBehaviour
    {
        [Tooltip("How the event will be triggered.")]
        public TriggerType triggerType;

        [Tooltip("If trigger collision will be filtered by layer.")]
        [ConditionalShow("triggerType", TriggerType.TriggerCollider)]
        public bool filterByLayer;

        [Tooltip("If trigger collision will be filtered by tag.")]
        [ConditionalShow("triggerType", TriggerType.TriggerCollider)]
        public bool filterByTag;

        [Tooltip("Layers accepted to start trigger.")]
        [ConditionalShow("triggerType", TriggerType.TriggerCollider, "filterByLayer", true)]
        public LayerMask acceptedLayers;

        [Tooltip("Tag accepted to start trigger.")]
        [ConditionalShow("triggerType", TriggerType.TriggerCollider, "filterByTag", true)]
        public string acceptedTag;

        [Space]
        [Space]

        public UnityEvent whenTriggered;

        [HideInInspector]
        public bool hasTriggered;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (triggerType == TriggerType.TriggerCollider)
            {
                if (collision.gameObject.layer == acceptedLayers.value || !filterByLayer)
                {
                    if (filterByTag)
                    {
                        if (collision.tag == acceptedTag)
                        {
                            Trigger();
                        }
                    }
                    else
                    {
                        Trigger();
                    }
                }
            }
        }

        public void Trigger()
        {
            hasTriggered = true;
            whenTriggered.Invoke();
        }
    }
}
