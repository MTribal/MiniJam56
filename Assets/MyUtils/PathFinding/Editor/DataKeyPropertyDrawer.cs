using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace My_Utils.PathFinding
{
    [CustomPropertyDrawer(typeof(DataKeyAttribute))]
    public class DataKeyPropertyDrawer : PropertyDrawer
    {
        private string[] keyArray;
        private GUIContent[] contentArray;

        private bool initialized = false;
        private int currentIndex;

        private void OnEnable()
        {
            initialized = true;

            DataKeyAttribute dataKeyAttribute = (DataKeyAttribute)attribute;
            if (dataKeyAttribute.KeyType == DataKeyType.Platformer)
            {
                if (SaveSystem.ContainsKey(PathFindingManager.PLATFORMER_KEYS_KEY))
                    keyArray = SaveSystem.LoadData<List<string>>(PathFindingManager.PLATFORMER_KEYS_KEY).ToArray();
                else
                {
                    keyArray = new string[0];
                    SaveSystem.SaveDataIn(new List<string>(), PathFindingManager.PLATFORMER_KEYS_KEY);
                }

            }
            else
            {
                if (SaveSystem.ContainsKey(PathFindingManager.TOPDOWN_KEYS_KEY))
                    keyArray = SaveSystem.LoadData<List<string>>(PathFindingManager.TOPDOWN_KEYS_KEY).ToArray();
                else
                {
                    keyArray = new string[0];
                    SaveSystem.SaveDataIn(new List<string>(), PathFindingManager.TOPDOWN_KEYS_KEY);
                }
            }

            contentArray = ToGuiContentArray(keyArray);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!initialized)
            {
                OnEnable();
                currentIndex = keyArray.IndexOf(property.stringValue);
                if (currentIndex == -1)
                    currentIndex = 0;
                if (keyArray.Length > 0)
                    property.stringValue = keyArray[0];
            }

            if (keyArray.Length == 0)
            {
                GUIContent[] noneContent = new GUIContent[] { new GUIContent("None Key Exist. To add a new key, add a PathFindingManager script to a empty" +
                                                                                "gameObject and then add a new key in the PathFinding script.")};
                EditorGUI.Popup(position, label, 0, noneContent);
            }
            else
            {
                currentIndex = EditorGUI.Popup(position, label, currentIndex, contentArray);

                property.stringValue = keyArray[currentIndex];
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        private GUIContent[] ToGuiContentArray(string[] stringArray)
        {
            GUIContent[] contents = new GUIContent[stringArray.Length];

            for (int i = 0; i < contents.Length; i++)
            {
                contents[i] = new GUIContent(stringArray[i]);
            }

            return contents;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
