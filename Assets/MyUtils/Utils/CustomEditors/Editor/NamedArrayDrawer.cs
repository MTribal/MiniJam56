using UnityEngine;
using UnityEditor;

namespace My_Utils
{
    [CustomPropertyDrawer(typeof(NamedArrayAttribute))]
    public class NamedArrayPropertyDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty list = property.FindPropertyRelative("Values");

            int max = list.arraySize - 1;
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < max; i++)
            {
                NamedArrayAttribute namedArrayAttribute = (NamedArrayAttribute)attribute;
                string displayName = namedArrayAttribute.name + " " + (i + 1);
                EditorGUI.PropertyField(position, list.GetArrayElementAtIndex(i), new GUIContent(displayName));
            }
            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }
    }
}