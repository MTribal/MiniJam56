using UnityEditor;
using UnityEngine;

namespace My_Utils.Shooting
{
    [CustomEditor(typeof(BaseGun))]
    public class GunEditor : Editor
    {
        private BaseGun _target;
        private SerializedProperty projectilePrefabProp, uniqueTagProp, poolTagProp, automaticSizeProp, shootPosProp, timeBetweenShotsProp, poolSizeProp;

        private void OnEnable()
        {
            _target = (BaseGun)target;

            projectilePrefabProp = serializedObject.FindProperty("projectilePrefab");
            poolTagProp = serializedObject.FindProperty("poolTag");
            automaticSizeProp = serializedObject.FindProperty("automaticSize");
            poolSizeProp = serializedObject.FindProperty("poolSize");
            shootPosProp = serializedObject.FindProperty("shootPos");
            timeBetweenShotsProp = serializedObject.FindProperty("timeBetweenShots");
            uniqueTagProp = serializedObject.FindProperty("_uniqueTag");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(projectilePrefabProp);
            EditorGUILayout.PropertyField(uniqueTagProp);
            EditorGUILayout.PropertyField(poolTagProp);

            bool isSetToAutomatic = automaticSizeProp.boolValue;

            if (isSetToAutomatic)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(automaticSizeProp);
                if (_target.projectilePrefab != null)
                {
                    GUI.enabled = false;
                    EditorGUILayout.LabelField("Preview - " + _target._CalculatePoolSize());
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.PropertyField(automaticSizeProp);
                EditorGUILayout.PropertyField(poolSizeProp);
            }

            EditorGUILayout.PropertyField(shootPosProp);
            EditorGUILayout.PropertyField(timeBetweenShotsProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
