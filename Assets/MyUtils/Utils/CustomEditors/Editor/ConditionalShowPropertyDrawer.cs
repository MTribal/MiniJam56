using UnityEngine;
using UnityEditor;
using System.Linq;
using My_Utils;

[CustomPropertyDrawer(typeof(ConditionalShowAttribute))]
public class ConditionalShowPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalShowAttribute condHAtt = (ConditionalShowAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
        
        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalShowAttribute condHAtt = (ConditionalShowAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(ConditionalShowAttribute condHAtt, SerializedProperty property)
    {
        bool[] conditionsResults = new bool[condHAtt.ObjectsToCompare.Length];

        for (int i = 0; i < condHAtt.ObjectsToCompare.Length; i++)
        {
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSources[i]); //changes the path to the conditionalsource property path
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                switch (sourcePropertyValue.type)
                {
                    case "bool":
                        conditionsResults[i] = sourcePropertyValue.boolValue.Equals(condHAtt.ObjectsToCompare[i]);
                        break;
                    case "Enum":
                        conditionsResults[i] = sourcePropertyValue.enumValueIndex.Equals((int)condHAtt.ObjectsToCompare[i]);
                        break;
                    case "float":
                        conditionsResults[i] = sourcePropertyValue.floatValue.Equals(condHAtt.ObjectsToCompare[i]);
                        break;
                    case "string":
                        conditionsResults[i] = sourcePropertyValue.stringValue.Equals(condHAtt.ObjectsToCompare[i]);
                        break;
                    case "int":
                        conditionsResults[i] = sourcePropertyValue.intValue.Equals(condHAtt.ObjectsToCompare[i]);
                        break;
                    default:
                        Debug.LogError("Type " + sourcePropertyValue.type + " not supported to comparison.");
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSources[i]);
            }
        }
        // Check if all items is true

        bool[] checkResults = conditionsResults.Where(item => item == true).ToArray();
        return checkResults.Length == conditionsResults.Length;
    }
}
