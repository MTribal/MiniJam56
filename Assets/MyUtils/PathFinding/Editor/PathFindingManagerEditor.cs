using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace My_Utils.PathFinding
{
    [CustomEditor(typeof(PathFindingManager))]
    public class PathFindingManagerEditor : Editor
    {
        private PathFindingManager pathFinder;
        private SerializedProperty pathFindingTypeProp, platformerFoldoutProp, topdownFoldoutProp, topdownObstaclesProp;


        private List<string> topdownKeys;
        private SerializedProperty topdownGridKeyProp, topdownCurrentKeyIndexProp, topdownGridTypeProp;


        #region Map Props
        private SerializedProperty drawGridProp, drawLinesProp, editModeProp, visualizeOnceProp, visualizeIdProp, centerProp, gridSizeProp, nodeSizeProp, toleranceProp,
                                    obstaclesProp;
        #endregion


        #region Edges Props
        private List<string> platformerKeys;
        private string selectedKey;

        private PlatformerGraph<Vector2> savedGraph;
        private List<PlatformerEdge<Vector2>> allEdges;
        private List<PlatformerEdge<Vector2>> atualPlatformEdges;

        private string lastSaveKey;
        private bool selectAllEdges;
        private bool selectSpecificEdge;

        private int selectedEdgeIndex;
        private bool confirmDeletion;

        private readonly Color unselectedEdgeColor = Color.yellow;
        private readonly Color selectedEdgeColor = Color.red;

        private SerializedProperty selectPlatformProp, platformIndexProp, allowFromProp, allowToProp;
        private int lastPlatformIndex;
        private bool lastAllowFrom, lastAllowTo;
        #endregion


        private void OnEnable()
        {
            pathFinder = (PathFindingManager)target;

            pathFindingTypeProp = serializedObject.FindProperty("pathFindingType");
            platformerFoldoutProp = serializedObject.FindProperty("platformerFoldout");
            topdownFoldoutProp = serializedObject.FindProperty("topdownFoldout");

            GetTopdownProps();
            AtualizeMapProps();
            GetEdgesProps();
        }

        private void OnSceneGUI()
        {
            DrawEdges();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(pathFindingTypeProp, new GUIContent("Path Finding Type", "The type of path finding that you will use."));
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();


            GUI.enabled = pathFindingTypeProp.enumValueIndex == 1 || pathFindingTypeProp.enumValueIndex == 2; // Platformer || Both
            if (GUILayout.Button(new GUIContent("Platformer", "Click to expand platformer options.")))
            {
                platformerFoldoutProp.boolValue = !platformerFoldoutProp.boolValue;
                if (platformerFoldoutProp.boolValue)
                    topdownFoldoutProp.boolValue = false;
                AtualizeMapProps();
            }
            if (!GUI.enabled)
                platformerFoldoutProp.boolValue = false;
            else if (platformerFoldoutProp.boolValue)
                PlatformerInspector();


            GUI.enabled = pathFindingTypeProp.enumValueIndex == 0 || pathFindingTypeProp.enumValueIndex == 2; // Topdown || Both
            EditorGUILayout.Separator();
            if (GUILayout.Button(new GUIContent("Topdown", "Click to expand topdown options.")))
            {
                topdownFoldoutProp.boolValue = !topdownFoldoutProp.boolValue;
                if (topdownFoldoutProp.boolValue)
                    platformerFoldoutProp.boolValue = false;
                AtualizeMapProps();
            }

            if (!GUI.enabled)
                topdownFoldoutProp.boolValue = false;
            else if (topdownFoldoutProp.boolValue)
                TopdownInspector();

            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }


        [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Selected)]
        private static void DrawMap(PathFindingManager src, GizmoType gizmoType)
        {
            if (src.platformerFoldout)
            {
                #region VerifyIfNeedAtualize


                if (src._platformerGrid == null)
                {
                    src._CreatePlatformGrid();
                }
                if (src.platformerEditMode)
                {
                    src.AtualizeGridNodes();
                }


                #endregion


                #region DrawGridNodes
                for (int y = 0; y < src.platformerGridSize.y; y++)
                {
                    for (int x = 0; x < src.platformerGridSize.x; x++)
                    {
                        GridPathNode gridNode = src._platformerGrid.GetValue(x, y);

                        if (!gridNode.isWalkable)
                        {
                            continue;
                        }

                        bool isSelectedNodeId = src.selectedNodeId == gridNode.PlatformId && src.selectPlatform;

                        if (!src.platformerDrawGrid && !isSelectedNodeId)
                            continue;

                        if (gridNode.PlatformId != src.visualizeId && src.visualizeOnce && !isSelectedNodeId)
                            continue;

                        if (isSelectedNodeId)
                            Gizmos.color = src._nodeSelectedColor;
                        else
                            Gizmos.color = src._nodeColor;

                        Vector2 center = src._platformerGrid.GetWorldPosition(x, y, CellWorldPositionType.Center);
                        Vector2 size = new Vector2(src.platformerNodeSize, src.platformerNodeSize);
                        Gizmos.DrawCube(center, size);
                    }
                }

                #endregion


                #region DrawGridLines
                if (src.platformerDrawLines)
                {
                    Gizmos.color = src._gridLineColor;
                    float2 center = new float2(src.platformerCenter.x, src.platformerCenter.y);
                    float halfWidth = src.platformerGridSize.x * src.platformerNodeSize / 2;
                    float halfHeight = src.platformerGridSize.y * src.platformerNodeSize / 2;

                    // Draw Y Lines
                    float xPos = center.x - halfWidth;
                    for (int i = 0; i <= src.platformerGridSize.x; i++)
                    {
                        Vector2 lineStart = new Vector2(xPos, center.y - halfHeight);
                        Vector2 lineEnd = new Vector2(xPos, center.y + halfHeight);

                        Gizmos.DrawLine(lineStart, lineEnd);
                        xPos += src.platformerNodeSize;
                    }

                    // Draw X Lines
                    float yPos = center.y - halfHeight;
                    for (int i = 0; i <= src.platformerGridSize.y; i++)
                    {
                        Vector2 lineStart = new Vector2(center.x - halfWidth, yPos);
                        Vector2 lineEnd = new Vector2(center.x + halfWidth, yPos);

                        Gizmos.DrawLine(lineStart, lineEnd);
                        yPos += src.platformerNodeSize;
                    }
                }
                #endregion
            }
            else if (src.topdownFoldout)
            {
                #region VerifyIfNeedAtualize


                if (src._topdownGridDynamic == null)
                {
                    if (src.topdownEditMode || src.topdownGridKey == "")
                    {
                        src._CreateTopdownGrid();

                        if (src.topdownGridKey != "")
                        {
                            TopdownData topdownData = new TopdownData(src.topdownDrawGrid, src.topdownDrawLines, src.topdownEditMode, src.topdownTolerance, src.topdownObstacles.value, src._topdownGridDynamic);
                            SaveSystem.SaveDataIn(topdownData, src.topdownGridKey);
                        }
                    }
                    else
                    {
                        src._topdownGridDynamic = SaveSystem.LoadData<TopdownData>(src.topdownGridKey).grid;
                    }
                }
                if (src.topdownEditMode && src.topdownGridKey != "")
                {
                    src.AtualizeGridNodes();
                    TopdownData topdownData = new TopdownData(src.topdownDrawGrid, src.topdownDrawLines, src.topdownEditMode, src.topdownTolerance, src.topdownObstacles.value, src._topdownGridDynamic);
                    SaveSystem.SaveDataIn(topdownData, src.topdownGridKey);
                }


                #endregion

                if (!src.topdownDrawGrid)
                    return;

                #region DrawGridNodes
                Gizmos.color = src._nodeColor;
                for (int y = 0; y < src.topdownGridSize.y; y++)
                {
                    for (int x = 0; x < src.topdownGridSize.x; x++)
                    {
                        GridPathNode gridNode = src._topdownGridDynamic.GetValue(x, y);

                        if (!gridNode.isWalkable)
                            continue;

                        Vector2 center = src._topdownGridDynamic.GetWorldPosition(x, y, CellWorldPositionType.Center);
                        Vector2 size = new Vector2(src.topdownNodeSize, src.topdownNodeSize);
                        Gizmos.DrawCube(center, size);
                    }
                }

                #endregion

                #region DrawGridLines
                if (src.topdownDrawLines)
                {
                    Gizmos.color = src._gridLineColor;
                    float2 center = new float2(src.topdownCenter.x, src.topdownCenter.y);
                    float halfWidth = src.topdownGridSize.x * src.topdownNodeSize / 2;
                    float halfHeight = src.topdownGridSize.y * src.topdownNodeSize / 2;

                    // Draw Y Lines
                    float xPos = center.x - halfWidth;
                    for (int i = 0; i <= src.topdownGridSize.x; i++)
                    {
                        Vector2 lineStart = new Vector2(xPos, center.y - halfHeight);
                        Vector2 lineEnd = new Vector2(xPos, center.y + halfHeight);

                        Gizmos.DrawLine(lineStart, lineEnd);
                        xPos += src.topdownNodeSize;
                    }

                    // Draw X Lines
                    float yPos = center.y - halfHeight;
                    for (int i = 0; i <= src.topdownGridSize.y; i++)
                    {
                        Vector2 lineStart = new Vector2(center.x - halfWidth, yPos);
                        Vector2 lineEnd = new Vector2(center.x + halfWidth, yPos);

                        Gizmos.DrawLine(lineStart, lineEnd);
                        yPos += src.topdownNodeSize;
                    }
                }
                #endregion
            }
        }

        private void BasicInspector()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(drawGridProp, new GUIContent("Draw Grid", "If the grid will be draw."));

            if (!drawGridProp.boolValue) GUI.enabled = false;
            EditorGUILayout.PropertyField(drawLinesProp, new GUIContent("Draw Lines", "If the lines of the grid will be draw."));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = true;
            EditorGUILayout.PropertyField(editModeProp, new GUIContent("Edit Mode", "In this mode, the nodes will be atualizing every frame. Use when editing the map."));
            if (!drawGridProp.boolValue) GUI.enabled = false;

            if (platformerFoldoutProp.boolValue)
                EditorGUILayout.PropertyField(visualizeOnceProp, new GUIContent("Visualize Once", "Mark to visualize only one platform at time."));

            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            if (visualizeOnceProp.boolValue && platformerFoldoutProp.boolValue)
            {
                int sliderMax = pathFinder.PlatformerBaseGraph != null ? (pathFinder.PlatformerBaseGraph.VerticesCount / 2) - 1 : 0;
                visualizeIdProp.intValue = EditorGUILayout.IntSlider(new GUIContent("Visualize Id", "The ID of the platform that you want to visualize."), visualizeIdProp.intValue, 0, sliderMax);
            }

            EditorGUILayout.Separator();
            Vector2 lastCenterPos = centerProp.vector2Value;
            EditorGUILayout.PropertyField(centerProp, new GUIContent("Center Position", "The center of your grid"));

            Vector2Int lastGridSize = gridSizeProp.vector2IntValue;
            EditorGUILayout.PropertyField(gridSizeProp, new GUIContent("Grid Size", "The size of the grid in cells."));

            float lastNodeSize = nodeSizeProp.floatValue;
            EditorGUILayout.PropertyField(nodeSizeProp, new GUIContent("Node Size", "The size of each node cell."));

            float lastToleranceValue = toleranceProp.floatValue;
            EditorGUILayout.Slider(toleranceProp, 0, 1, new GUIContent("Tolerance", "The tolerance to consider a node as a obstacle."));

            int lastPlatformObstaclesValue = pathFinder.platformerObstacles.value;
            int lastTopdownObstaclesValue = pathFinder.topdownObstacles.value;
            EditorGUILayout.PropertyField(obstaclesProp, new GUIContent("Obstacles", "What will be considered as obstacles."));

            serializedObject.ApplyModifiedProperties();

            #region Atualize Map
            bool atualized = false;

            if (lastToleranceValue != toleranceProp.floatValue)
            {
                pathFinder.AtualizeGridNodes();
                atualized = true;
            }

            if (lastGridSize != gridSizeProp.vector2IntValue)
            {
                if (pathFinder.platformerFoldout)
                    pathFinder._CreatePlatformGrid();
                else
                    pathFinder._CreateTopdownGrid();
                atualized = true;
            }

            if (lastNodeSize != nodeSizeProp.floatValue)
            {
                Vector2 pos = centerProp.vector2Value;
                if (pathFinder.platformerFoldout)
                {
                    Vector2 originPosition = new Vector2(pos.x - (pathFinder.platformerGridSize.x / 2 * pathFinder.platformerNodeSize),
                                                      pos.y - (pathFinder.platformerGridSize.y / 2 * pathFinder.platformerNodeSize));
                    pathFinder._platformerGrid.SetOriginPosition(originPosition);
                    pathFinder._platformerGrid.CellSize = pathFinder.platformerNodeSize;
                }
                else
                {
                    Vector2 originPosition = new Vector2(pos.x - (gridSizeProp.vector2IntValue.x / 2 * nodeSizeProp.floatValue),
                                                      pos.y - (gridSizeProp.vector2IntValue.y / 2 * nodeSizeProp.floatValue));
                    pathFinder._topdownGridDynamic.SetOriginPosition(originPosition);
                    pathFinder._topdownGridDynamic.CellSize = pathFinder.topdownNodeSize;
                }
                pathFinder.AtualizeGridNodes();
                atualized = true;
            }
            if ((platformerFoldoutProp.boolValue && lastPlatformObstaclesValue != pathFinder.platformerObstacles.value) ||
                (topdownFoldoutProp.boolValue && lastTopdownObstaclesValue != pathFinder.topdownObstacles.value))
            {
                pathFinder.AtualizeGridNodes();
                atualized = true;
            }

            if (lastCenterPos != centerProp.vector2Value)
            {
                Vector2 pos = centerProp.vector2Value;
                if (pathFinder.platformerFoldout)
                {
                    Vector2 originPosition = new Vector2(pos.x - (pathFinder.platformerGridSize.x / 2 * pathFinder.platformerNodeSize), pos.y - (pathFinder.platformerGridSize.y / 2 * pathFinder.platformerNodeSize));
                    pathFinder._platformerGrid.SetOriginPosition(originPosition);
                    pathFinder.AtualizeGridNodes();
                }
                else
                {
                    Vector2 originPosition = new Vector2(pos.x - (pathFinder.topdownGridSize.x / 2 * pathFinder.topdownNodeSize), pos.y - (pathFinder.topdownGridSize.y / 2 * pathFinder.topdownNodeSize));
                    pathFinder._topdownGridDynamic.SetOriginPosition(originPosition);
                    pathFinder.AtualizeGridNodes();
                }
                atualized = true;
            }
            if (atualized && topdownFoldoutProp.boolValue)
            {
                TopdownData topdownData = new TopdownData(drawGridProp.boolValue, drawLinesProp.boolValue, editModeProp.boolValue, pathFinder.topdownTolerance, topdownObstaclesProp.intValue, pathFinder._topdownGridDynamic);
                SaveSystem.SaveDataIn(topdownData, pathFinder.topdownGridKey);
            }
            #endregion
        }

        private void AtualizeMapProps()
        {
            if (platformerFoldoutProp.boolValue)
            {
                drawGridProp = serializedObject.FindProperty("platformerDrawGrid");
                drawLinesProp = serializedObject.FindProperty("platformerDrawLines");
                editModeProp = serializedObject.FindProperty("platformerEditMode");
                visualizeOnceProp = serializedObject.FindProperty("visualizeOnce");
                visualizeIdProp = serializedObject.FindProperty("visualizeId");
                centerProp = serializedObject.FindProperty("platformerCenter");
                gridSizeProp = serializedObject.FindProperty("platformerGridSize");
                nodeSizeProp = serializedObject.FindProperty("platformerNodeSize");
                toleranceProp = serializedObject.FindProperty("platformerTolerance");
                obstaclesProp = serializedObject.FindProperty("platformerObstacles");
            }
            else if (topdownFoldoutProp.boolValue)
            {
                drawGridProp = serializedObject.FindProperty("topdownDrawGrid");
                drawLinesProp = serializedObject.FindProperty("topdownDrawLines");
                editModeProp = serializedObject.FindProperty("topdownEditMode");
                centerProp = serializedObject.FindProperty("topdownCenter");
                gridSizeProp = serializedObject.FindProperty("topdownGridSize");
                nodeSizeProp = serializedObject.FindProperty("topdownNodeSize");
                toleranceProp = serializedObject.FindProperty("topdownTolerance");
                obstaclesProp = serializedObject.FindProperty("topdownObstacles");

                // Is not relative to topdown but whe need a reference
                visualizeOnceProp = serializedObject.FindProperty("visualizeOnce");
                visualizeIdProp = serializedObject.FindProperty("visualizeId");
            }

        }


        #region Platformer
        private void PlatformerInspector()
        {
            BasicInspector();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EdgesInspector();
        }

        private void GetEdgesProps()
        {
            if (!SaveSystem.ContainsKey(PathFindingManager.PLATFORMER_KEYS_KEY))
            {
                platformerKeys = new List<string>();
                SaveSystem.SaveDataIn(platformerKeys, PathFindingManager.PLATFORMER_KEYS_KEY);
            }
            else
            {
                platformerKeys = SaveSystem.LoadData<List<string>>(PathFindingManager.PLATFORMER_KEYS_KEY);
            }

            selectPlatformProp = serializedObject.FindProperty("selectPlatform");
            platformIndexProp = serializedObject.FindProperty("platformIndex");
            allowFromProp = serializedObject.FindProperty("allowFrom");
            allowToProp = serializedObject.FindProperty("allowTo");

            AtualizeGraph();
            if (savedGraph != null)
            {
                AtualizeAllEdges();
                AtualizeAtualPlatformEdges();
            }
            lastSaveKey = pathFinder.savedGraphKey;
        }

        private void GetTopdownProps()
        {
            topdownGridKeyProp = serializedObject.FindProperty("topdownGridKey");
            topdownObstaclesProp = serializedObject.FindProperty("topdownObstacles");
            topdownCurrentKeyIndexProp = serializedObject.FindProperty("topdownCurrentKeyIndex");
            topdownGridTypeProp = serializedObject.FindProperty("topdownGridType");
            if (!SaveSystem.ContainsKey(PathFindingManager.TOPDOWN_KEYS_KEY))
            {
                topdownKeys = new List<string>();
                SaveSystem.SaveDataIn(topdownKeys, PathFindingManager.TOPDOWN_KEYS_KEY);
            }
            else
            {
                topdownKeys = SaveSystem.LoadData<List<string>>(PathFindingManager.TOPDOWN_KEYS_KEY);
            }
        }

        #region Edges Editor
        private void EdgesInspector()
        {
            if (lastSaveKey != pathFinder.savedGraphKey)
            {
                lastSaveKey = pathFinder.savedGraphKey;
                AtualizeGraph();
                if (savedGraph != null)
                    AtualizeAllEdges();
            }
            if (savedGraph != null)
            {
                if (lastPlatformIndex != platformIndexProp.intValue)
                {
                    lastPlatformIndex = platformIndexProp.intValue;
                    AtualizeAtualPlatformEdges();
                }
                if (lastAllowFrom != allowFromProp.boolValue)
                {
                    lastAllowFrom = allowFromProp.boolValue;
                    AtualizeAtualPlatformEdges();
                }
                if (lastAllowTo != allowToProp.boolValue)
                {
                    lastAllowTo = allowToProp.boolValue;
                    AtualizeAtualPlatformEdges();
                }
            }

            pathFinder.platformerCurrentKeyIndex = EditorGUILayout.Popup(new GUIContent("Key To Edit", "The key that you want to edit edges."), pathFinder.platformerCurrentKeyIndex, platformerKeys.ToArray());

            pathFinder.platformerCurrentKeyIndex = MyUtils.ClampIndex(pathFinder.platformerCurrentKeyIndex, platformerKeys.Count);
            if (pathFinder.platformerCurrentKeyIndex >= 0)
                pathFinder.savedGraphKey = platformerKeys[pathFinder.platformerCurrentKeyIndex];
            else
                pathFinder.savedGraphKey = "";


            EditorGUILayout.BeginHorizontal();
            selectedKey = EditorGUILayout.TextField(new GUIContent(
                "Select",
                "Type a new key name to add. Type a existing key name to delete. You CANNOT UNDO this actions."), selectedKey);

            if (GUILayout.Button(new GUIContent("Add", "Click to add a new key with the name given.")))
            {
                if (selectedKey != null && selectedKey != "" && !SaveSystem.ContainsKey(selectedKey))
                {
                    // Add key
                    platformerKeys.Add(selectedKey);
                    SaveSystem.SaveDataIn(platformerKeys, PathFindingManager.PLATFORMER_KEYS_KEY);
                    SaveSystem.SaveDataIn(new GraphData(new PlatformerGraph<Vector2>()), selectedKey);
                    selectedKey = "";
                }
            }
            if (GUILayout.Button(new GUIContent("Delete", "Click to delete all edges of the key with the name given. You CANNOT UNDO this action!")))
            {
                platformerKeys.Remove(selectedKey);
                if (SaveSystem.ContainsKey(selectedKey))
                {
                    SaveSystem.SaveDataIn(platformerKeys, PathFindingManager.PLATFORMER_KEYS_KEY);
                    SaveSystem.DeleteKey(selectedKey);
                    selectedKey = "";
                    pathFinder.platformerCurrentKeyIndex = MyUtils.ClampIndex(pathFinder.platformerCurrentKeyIndex, platformerKeys.Count);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (savedGraph != null && savedGraph.EdgesCount != 0)
            {
                EditorGUILayout.BeginHorizontal();
                pathFinder.drawEdges = EditorGUILayout.Toggle(new GUIContent("Draw Edges", "Mark to visualize the recorded edges."), pathFinder.drawEdges);

                if (!pathFinder.drawEdges)
                    GUI.enabled = false;

                bool lastSelectPlatform = selectPlatformProp.boolValue;
                EditorGUILayout.PropertyField(selectPlatformProp, new GUIContent("Select Platform", "Mark if you want to edit a specif platform"));
                EditorGUILayout.EndHorizontal();

                if (!lastSelectPlatform && selectPlatformProp.boolValue)
                    AtualizeAtualPlatformEdges();

                if (selectPlatformProp.boolValue)
                {
                    int sliderMaxValue = savedGraph != null ? savedGraph.PlatformsCount - 1 : 0;
                    EditorGUILayout.IntSlider(platformIndexProp, 0, sliderMaxValue, new GUIContent("Platform ID", "The ID of the platform that you want to edit."));

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(allowFromProp, new GUIContent("Allow From", "If you want to visualize edges starting from the platform ID selected."));
                    EditorGUILayout.PropertyField(allowToProp, new GUIContent("Allow To", "If you want to visualize edges finishing in the platform ID selected."));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                }

                EditorGUILayout.BeginHorizontal();
                bool delSpecific = EditorGUILayout.Toggle(new GUIContent("Select Specific Edge", "Mark to delete a specific edge. You cannot undo this action."), selectSpecificEdge);
                bool delAll = EditorGUILayout.Toggle(new GUIContent("Select All Edges", "Mark to delete all edges. You cannot undo this action."), selectAllEdges);

                if (delAll && delSpecific)
                {
                    if (!selectSpecificEdge)
                        delAll = false;
                    else if (!selectAllEdges)
                        delSpecific = false;
                }
                selectSpecificEdge = delSpecific;
                selectAllEdges = delAll;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            if (savedGraph == null || savedGraph.EdgesCount == 0)
            {
                EditorGUILayout.HelpBox("Graph has no recorded edge yet.", MessageType.Info);
            }
            else
            {
                if (selectSpecificEdge)
                {
                    int[] sliderRange;
                    if (selectPlatformProp.boolValue)
                    {
                        sliderRange = new int[] { 0, atualPlatformEdges.Count - 1 };
                        selectedEdgeIndex = Mathf.Clamp(selectedEdgeIndex, sliderRange[0], sliderRange[1]);
                    }
                    else
                    {
                        int max = savedGraph != null ? savedGraph.EdgesCount - 1 : 0;
                        sliderRange = new int[] { 0, max };
                    }

                    selectedEdgeIndex = EditorGUILayout.IntSlider(new GUIContent("Edge Index", "Index of the edge that you want to delete."), selectedEdgeIndex, sliderRange[0], sliderRange[1]);
                }

                PlatformerEdge<Vector2> selectedEdge;
                if (selectPlatformProp.boolValue)
                {
                    if (atualPlatformEdges == null)
                        AtualizeAtualPlatformEdges();

                    if (selectedEdgeIndex < atualPlatformEdges.Count && selectedEdgeIndex >= 0)
                    {
                        selectedEdge = atualPlatformEdges[selectedEdgeIndex];
                    }
                    else
                        selectedEdge = null;
                }
                else
                {
                    selectedEdge = allEdges[selectedEdgeIndex];
                }
                if (selectSpecificEdge || selectAllEdges)
                {
                    if (selectedEdge != null)
                    {
                        if (selectSpecificEdge)
                        {
                            EditorGUILayout.BeginHorizontal();
                            confirmDeletion = EditorGUILayout.Toggle(new GUIContent("Confirm deletion", "Mark this to confirm the deletion of the selected edge."), confirmDeletion);
                            EditorGUILayout.LabelField(new GUIContent($"  Weight:    {selectedEdge.wheight}", "The wheight of the current edge."));
                            EditorGUILayout.EndHorizontal();
                        }
                        else
                        {
                            confirmDeletion = EditorGUILayout.Toggle(new GUIContent("Confirm deletion", "Mark this to confirm the deletion of the selected edge."), confirmDeletion);
                        }

                        if (GUILayout.Button(new GUIContent("Delete Edge", "Click to delete the selected edge. You cannot undo this action.")))
                        {
                            if (!confirmDeletion)
                            {
                                Debug.LogWarning("Please confirm deletion before click to delete.");
                            }
                            else if (selectSpecificEdge)
                            {
                                savedGraph.RemoveEdge(selectedEdge);
                                SaveSystem.SaveDataIn(new GraphData(savedGraph), pathFinder.savedGraphKey);
                                AtualizeAllEdges();
                                AtualizeAtualPlatformEdges();
                                confirmDeletion = false;
                            }
                            else // selectAllEdges
                            {
                                if (selectPlatformProp.boolValue)
                                {
                                    foreach (PlatformerEdge<Vector2> edge in atualPlatformEdges)
                                    {
                                        savedGraph.RemoveEdge(edge);
                                    }
                                }
                                else
                                {
                                    savedGraph.ClearEdges();
                                }

                                SaveSystem.SaveDataIn(new GraphData(savedGraph), pathFinder.savedGraphKey);
                                confirmDeletion = false;
                                selectAllEdges = false;

                                if (selectPlatformProp.boolValue)
                                {
                                    AtualizeAllEdges();
                                    AtualizeAtualPlatformEdges();
                                }
                            }
                        }
                    }
                }
                if (selectedEdge == null)
                {
                    string message;
                    if (allowFromProp.boolValue && allowToProp.boolValue)
                        message = "The selected platform has no edges.";
                    else if (allowFromProp.boolValue)
                        message = "The selected platform has no 'From' edges.";
                    else if (allowToProp.boolValue)
                        message = "The selected platform has no 'To' edges.";
                    else
                        message = "Cannot find edges becase you have not allowed neither 'From' or 'To' options.";

                    EditorGUILayout.HelpBox(message, MessageType.Info);
                }

                GUI.enabled = true;
            }

            serializedObject.ApplyModifiedProperties();

            pathFinder.selectedNodeId = platformIndexProp.intValue;
        }

        private void DrawEdges()
        {
            if (savedGraph != null && platformerFoldoutProp.boolValue)
            {
                List<PlatformerEdge<Vector2>> allEdges = savedGraph.GetAllEdges();
                float ca_size = 0.5f;
                float angle = 27;
                if (pathFinder.drawEdges)
                {
                    if (selectSpecificEdge)
                    {
                        if (selectPlatformProp.boolValue)
                        {
                            for (int i = 0; i < atualPlatformEdges.Count; i++)
                            {
                                Handles.color = i == selectedEdgeIndex ? selectedEdgeColor : unselectedEdgeColor;
                                MyUtils.DrawArrow(atualPlatformEdges[i].fromPosition, atualPlatformEdges[i].toPosition, ca_size, angle, DrawType.Handles);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < allEdges.Count; i++)
                            {
                                Handles.color = i == selectedEdgeIndex ? selectedEdgeColor : unselectedEdgeColor;
                                MyUtils.DrawArrow(allEdges[i].fromPosition, allEdges[i].toPosition, ca_size, angle, DrawType.Handles);
                            }
                        }
                    }
                    else if (selectAllEdges)
                    {
                        Handles.color = selectedEdgeColor;
                        if (selectPlatformProp.boolValue)
                        {
                            for (int i = 0; i < atualPlatformEdges.Count; i++)
                            {
                                MyUtils.DrawArrow(atualPlatformEdges[i].fromPosition, atualPlatformEdges[i].toPosition, ca_size, angle, DrawType.Handles);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < allEdges.Count; i++)
                            {
                                MyUtils.DrawArrow(allEdges[i].fromPosition, allEdges[i].toPosition, ca_size, angle, DrawType.Handles);
                            }
                        }
                    }
                    else
                    {
                        Handles.color = unselectedEdgeColor;
                        if (selectPlatformProp.boolValue)
                        {
                            for (int i = 0; i < atualPlatformEdges.Count; i++)
                            {
                                MyUtils.DrawArrow(atualPlatformEdges[i].fromPosition, atualPlatformEdges[i].toPosition, ca_size, angle, DrawType.Handles);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < allEdges.Count; i++)
                            {
                                MyUtils.DrawArrow(allEdges[i].fromPosition, allEdges[i].toPosition, ca_size, angle, DrawType.Handles);
                            }
                        }
                    }
                }
            }
        }

        private void AtualizeGraph()
        {
            if (SaveSystem.ContainsKey(pathFinder.savedGraphKey) && pathFinder.savedGraphKey != "")
            {
                GraphData graphData = SaveSystem.LoadData<GraphData>(pathFinder.savedGraphKey);
                if (graphData != null && graphData.serializableGraph.AdjacencyList != null)
                    savedGraph = graphData.Load();
                else
                    savedGraph = null;
            }
            else
            {
                savedGraph = null;
            }
        }

        private void AtualizeAllEdges()
        {
            allEdges = savedGraph.GetAllEdges();
        }

        private void AtualizeAtualPlatformEdges()
        {
            if (selectPlatformProp.boolValue)
            {
                int platformId = platformIndexProp.intValue;
                bool allowFrom = allowFromProp.boolValue;
                bool allowTo = allowToProp.boolValue;
                atualPlatformEdges = allEdges.FindAll(edge => ((int)edge.from.platformId == platformId && allowFrom) || ((int)edge.to.platformId == platformId && allowTo));
            }
            else
            {
                atualPlatformEdges = new List<PlatformerEdge<Vector2>>();
            }
        }
        #endregion

        #endregion


        #region Topdown

        private void AtualizeTopdownGrid(string key)
        {
            if (SaveSystem.ContainsKey(key))
            {
                TopdownData topdownData = SaveSystem.LoadData<TopdownData>(key);
                pathFinder._topdownGridDynamic = topdownData.grid;
                drawGridProp.boolValue = topdownData.drawGrid;
                drawLinesProp.boolValue = topdownData.drawLines;
                editModeProp.boolValue = topdownData.editMode;
                topdownObstaclesProp.intValue = topdownData.obstaclesLayerMask;

                Vector2 realOrigin = topdownData.grid.OriginPosition.ToVector2();
                float centerX = -realOrigin.x - topdownData.grid.Width / 2 * topdownData.grid.CellSize;
                float centerY = -realOrigin.y - topdownData.grid.Height / 2 * topdownData.grid.CellSize;
                centerProp.vector2Value = new Vector2(-centerX, -centerY);

                gridSizeProp.vector2IntValue = new Vector2Int(topdownData.grid.Width, topdownData.grid.Height);
                nodeSizeProp.floatValue = topdownData.grid.CellSize;
                toleranceProp.floatValue = topdownData.tolerance;
            }
            else
            {
                pathFinder._topdownGridDynamic = null;
            }
        }

        private void TopdownInspector()
        {
            BasicInspector();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(topdownGridTypeProp, new GUIContent("Grid Type", "The type of grid that you want to use. Dynamic for grid createds in the Start() or Managed to grids stored in keys."));

            string lastTopdownKey = topdownGridKeyProp.stringValue;
            if (topdownGridTypeProp.enumValueIndex == (int)TopdownGridType.Managed)
            {
                topdownCurrentKeyIndexProp.intValue = EditorGUILayout.Popup(new GUIContent("Key To Edit", "The topdown key that you want to edit."), topdownCurrentKeyIndexProp.intValue, topdownKeys.ToArray());

                topdownCurrentKeyIndexProp.intValue = MyUtils.ClampIndex(topdownCurrentKeyIndexProp.intValue, topdownKeys.Count);
                if (topdownCurrentKeyIndexProp.intValue >= 0)
                    topdownGridKeyProp.stringValue = topdownKeys[topdownCurrentKeyIndexProp.intValue];
                else
                    topdownGridKeyProp.stringValue = "";

                EditorGUILayout.BeginHorizontal();
                selectedKey = EditorGUILayout.TextField(new GUIContent(
                    "Select",
                    "Type a new key name to add. Type a existing key name to delete. You CANNOT UNDO this actions."), selectedKey);

                if (GUILayout.Button(new GUIContent("Add", "Click to add a new key with the name given.")))
                {
                    if (selectedKey != null && selectedKey != "" && !SaveSystem.ContainsKey(selectedKey))
                    {
                        // Add key
                        topdownKeys.Add(selectedKey);
                        SaveSystem.SaveDataIn(topdownKeys, PathFindingManager.TOPDOWN_KEYS_KEY);

                        TopdownData topdownData = new TopdownData(true, true, false, 0, 0, new MyGrid<GridPathNode>(0, 0, 1, new Vector2(0, 0)));
                        SaveSystem.SaveDataIn(topdownData, selectedKey);
                        selectedKey = "";
                    }
                }
                if (GUILayout.Button(new GUIContent("Delete", "Click to delete all edges of the key with the name given. You CANNOT UNDO this action!")))
                {
                    topdownKeys.Remove(selectedKey);
                    SaveSystem.SaveDataIn(topdownKeys, PathFindingManager.TOPDOWN_KEYS_KEY);

                    if (SaveSystem.ContainsKey(selectedKey))
                    {
                        SaveSystem.DeleteKey(selectedKey);
                        selectedKey = "";
                        topdownCurrentKeyIndexProp.intValue = MyUtils.ClampIndex(topdownCurrentKeyIndexProp.intValue, topdownKeys.Count);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();

            if (lastTopdownKey != topdownGridKeyProp.stringValue)
            {
                AtualizeTopdownGrid(topdownGridKeyProp.stringValue);
            }
        }

        #endregion
    }
}
