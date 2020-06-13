using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace My_Utils
{
    [CustomEditor(typeof(AlphaGroup))]
    public class AlphaGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AlphaGroup alphaGroup = (AlphaGroup)target;

            RendererType newRend = (RendererType)EditorGUILayout.EnumFlagsField(new GUIContent("Renderer Type", "Type of renderers accepted."),
                                                                                    alphaGroup.rendererType);
            if (newRend != alphaGroup.rendererType)
            {
                Undo.RecordObject(alphaGroup, "Changed renderer type.");
                alphaGroup.rendererType = newRend;
            }

            float newAlpha = EditorGUILayout.Slider(new GUIContent("Alpha", "Atual alpha value of the group."), alphaGroup.alpha, 0, 1);
            if (newAlpha != alphaGroup.alpha)
            {
                Undo.RecordObject(alphaGroup, "Changed alpha.");
                alphaGroup.alpha = newAlpha;
            }

            if (GUILayout.Button(new GUIContent("Reset max alphas")))
            {
                alphaGroup._AtualizeMaxAlphas();
            }

            EditorGUILayout.Separator();
            bool useRemote = EditorGUILayout.Toggle(
                    new GUIContent("Use Remote Sprites", "Mark to modify renderers that ins't children of this component. (Only for SpriteRenderers)"),
                    alphaGroup.useRemoteObjects);

            if (useRemote != alphaGroup.useRemoteObjects)
            {
                Undo.RecordObject(alphaGroup, "Changed UseRemoteObjects."); 
                alphaGroup.useRemoteObjects = useRemote;
            }

            if (alphaGroup.useRemoteObjects)
            {
                List<GameObject> remoteObjects = alphaGroup.remoteObjects;
                bool restartRenderers = false;

                for (int i = 0; i < remoteObjects.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    remoteObjects[i] = (GameObject)EditorGUILayout.ObjectField(remoteObjects[i], typeof(GameObject), true);

                    bool delete = GUILayout.Button(new GUIContent("Delete", "saas"));
                    if (delete)
                    {
                        remoteObjects.RemoveAt(i);
                        restartRenderers = true;
                    }
                    else if (remoteObjects[i] != null)
                    {
                        if (!RendererGroupUtils.IsValid(remoteObjects[i]))
                        {
                            Debug.LogWarning("That GameObject is not valid for AlphaGroup remoteObjects.");
                            remoteObjects.RemoveAt(i);
                        }
                        else
                        {
                            Undo.RecordObject(alphaGroup, "Added a remote object.");
                            restartRenderers = true;
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }

                remoteObjects.RemoveAll(x => x == null);
                remoteObjects.Add(null);

                alphaGroup.remoteObjects = remoteObjects;

                if (restartRenderers)
                {
                    Undo.RecordObject(alphaGroup, "Atualized renderers.");
                    Debug.Log("Custom editor restart renderers.");
                    alphaGroup.AtualizeRenderers();
                }
            }
        }
    }

    [CustomEditor(typeof(ColorGroup))]
    public class ColorGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ColorGroup colorGroup = (ColorGroup)target;

            RendererType newRendererType = (RendererType)EditorGUILayout.EnumFlagsField(new GUIContent("Renderer Type", "Type of renderers accepted."),
                                                                                    colorGroup.rendererType);
            if (newRendererType != colorGroup.rendererType)
            {
                Undo.RecordObject(colorGroup, "Changed RendererType");
                colorGroup.rendererType = newRendererType;
            }

            EditorGUILayout.Separator();
            Color newColor = EditorGUILayout.ColorField(new GUIContent("Color", "Atual color value of the group."), colorGroup.color);
            if (newColor != colorGroup.color)
            {
                Undo.RecordObject(colorGroup, "Changed color");
                colorGroup.color = newColor;
            }

            EditorGUILayout.Separator();
            bool useRemote = EditorGUILayout.Toggle(
                    new GUIContent("Use Remote Sprites", "Mark to modify renderers that ins't children of this component. (Only for SpriteRenderers)"),
                    colorGroup.useRemoteObjects);

            if (useRemote != colorGroup.useRemoteObjects)
            {
                Undo.RecordObject(colorGroup, "Changed UseRemoteObjects.");
                colorGroup.useRemoteObjects = useRemote;
            }

            if (colorGroup.useRemoteObjects)
            {
                List<GameObject> remoteObjects = colorGroup.remoteObjects;
                bool restartRenderers = false;

                for (int i = 0; i < remoteObjects.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    remoteObjects[i] = (GameObject)EditorGUILayout.ObjectField(remoteObjects[i], typeof(GameObject), true);

                    bool delete = GUILayout.Button(new GUIContent("Delete", ""));
                    if (delete)
                    {
                        remoteObjects.RemoveAt(i);
                        restartRenderers = true;
                    }
                    else if (remoteObjects[i] != null)
                    {
                        if (!RendererGroupUtils.IsValid(remoteObjects[i]))
                        {
                            Debug.LogWarning("That GameObject is not valid for ColorGroup remoteObjects.");
                            remoteObjects.RemoveAt(i);
                        }
                        else
                        {
                            Undo.RecordObject(colorGroup, "Added a remote object.");
                            restartRenderers = true;
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }

                remoteObjects.RemoveAll(x => x == null);
                remoteObjects.Add(null);

                colorGroup.remoteObjects = remoteObjects;

                if (restartRenderers)
                {
                    Undo.RecordObject(colorGroup, "Atualize renderers");
                    colorGroup.AtualizeRenderers();
                }
            }
            else
            {
                Undo.RecordObject(colorGroup, "Atualize renderers");
                colorGroup.AtualizeRenderers();
            }
        }
    }
}
