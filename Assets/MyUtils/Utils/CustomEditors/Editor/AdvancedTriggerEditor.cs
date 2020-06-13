using UnityEngine;
using UnityEditor;
using System.Linq;

namespace My_Utils
{
    [CustomEditor(typeof(AdvancedTrigger))]
    [CanEditMultipleObjects]
    public class AdvancedTriggerEditor : Editor
    {
        private AdvancedTrigger advancedTrigger;

        private void OnEnable()
        {
            advancedTrigger = (AdvancedTrigger)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (advancedTrigger.triggerType == TriggerType.TriggerCollider)
            {
                Collider2D[] collider2D = advancedTrigger.GetComponents<Collider2D>();
                
                if (collider2D == null)
                {
                    ShowTriggerWarningMessage();
                }
                else
                {
                    bool[] isTriggers = collider2D.Select(collider => collider.isTrigger).ToArray();

                    if (isTriggers.Count(isTrigger => isTrigger == false) == isTriggers.Length)
                    {
                        ShowTriggerWarningMessage();
                    }
                }
            }
        }

        private void ShowTriggerWarningMessage()
        {
            string warningTriggerMessage = "Trigger will not work because trigger type is set to collider and gameObject has no trigger collider." +
                " Should attach a trigger collider2D to this gameObject.";

            EditorGUILayout.HelpBox(warningTriggerMessage, MessageType.Warning);
        }
    }
}
