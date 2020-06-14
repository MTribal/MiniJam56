using UnityEditor;
using UnityEngine;

public class CanonGunEditor : Editor
{
    [CustomEditor(typeof(CanonGun))]
    public class GunEditor : Editor
    {
        private CanonGun _target;
        private SerializedProperty projectilePrefabProp, uniqueTagProp, poolTagProp, automaticSizeProp, shootPosProp, timeBetweenShotsProp, poolSizeProp, yPosCurveProp;

        private void OnEnable()
        {
            _target = (CanonGun)target;

            projectilePrefabProp = serializedObject.FindProperty("projectilePrefab");
            poolTagProp = serializedObject.FindProperty("poolTag");
            automaticSizeProp = serializedObject.FindProperty("automaticSize");
            poolSizeProp = serializedObject.FindProperty("poolSize");
            shootPosProp = serializedObject.FindProperty("shootPos");
            timeBetweenShotsProp = serializedObject.FindProperty("timeBetweenShots");
            uniqueTagProp = serializedObject.FindProperty("_uniqueTag");
            yPosCurveProp = serializedObject.FindProperty("_yPosCurve");
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

            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(yPosCurveProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
