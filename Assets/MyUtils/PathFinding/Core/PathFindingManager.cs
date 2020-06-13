using My_Utils.Collections;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace My_Utils.PathFinding
{
    public delegate void OnTopdownPathComplete(TopdownPath path);

    public class PathFindingManager : MonoBehaviour
    {
        //private readonly Color[] colors = new Color[] { Color.white, Color.black, Color.green, Color.yellow, Color.red, Color.blue, Color.cyan, Color.magenta };
        public readonly Color _gridLineColor = new Color(0.75f, 0.75f, 0.75f, 0.5f);
        public readonly Color _nodeColor = new Color(0, 0, 1, 0.3f);
        public readonly Color _nodeSelectedColor = new Color(1f, 0.7f, 0f, 0.4f);

        [Tooltip("The type of path finding that that you will use.")]
        public PathFindingType pathFindingType = PathFindingType.Topdown;
        public bool platformerFoldout, topdownFoldout;
        private bool hasTopdownPathsToFind;
        private readonly List<FindPathData> topdownPathsToFind = new List<FindPathData>();
        private readonly List<OnTopdownPathComplete> topdownCompleteFunctions = new List<OnTopdownPathComplete>();


        #region Platformer Props
        public bool platformerDrawGrid;
        public bool platformerDrawLines;
        public bool platformerEditMode;
        public bool visualizeOnce;
        public int visualizeId;
        public Vector2 platformerCenter;
        public Vector2Int platformerGridSize;
        public float platformerNodeSize;
        public float platformerTolerance = 0.1f;
        public LayerMask platformerObstacles;


        private readonly Dictionary<string, PlatformerGraph<Vector2>> platformerSavedGraphs = new Dictionary<string, PlatformerGraph<Vector2>>();
        public MyGrid<GridPathNode> _platformerGrid;
        public PlatformerGraph<Vector2> PlatformerBaseGraph { get; private set; }

        // Edges
        public const string PLATFORMER_KEYS_KEY = "graphKeysArray";
        public string savedGraphKey;

        public int platformerCurrentKeyIndex;
        public int platformIndex;
        public int selectedNodeId;

        public bool drawEdges;
        public bool selectPlatform;
        public bool allowFrom, allowTo = true;

        #endregion


        #region Topdown Props
        private readonly Dictionary<string, MyGrid<GridPathNode>> topdownSavedGrids = new Dictionary<string, MyGrid<GridPathNode>>();
        public const string TOPDOWN_KEYS_KEY = "topdownKeys";
        public string topdownGridKey;
        public int topdownCurrentKeyIndex;

        public bool topdownDrawGrid;
        public bool topdownDrawLines;
        public bool topdownEditMode;
        public Vector2 topdownCenter;
        public Vector2Int topdownGridSize;
        public float topdownNodeSize;
        public float topdownTolerance = 0.1f;
        public LayerMask topdownObstacles;

        public MyGrid<GridPathNode> _topdownGridDynamic;

        public TopdownGridType topdownGridType;
        #endregion


        private void Awake()
        {
            platformerEditMode = false;
            topdownEditMode = false;

            if (pathFindingType != PathFindingType.Topdown)
                _CreatePlatformGrid();
            else
                _platformerGrid = null;

            if (pathFindingType != PathFindingType.Platformer && topdownGridType == TopdownGridType.Dynamic)
                _CreateTopdownGrid();
            else
                _topdownGridDynamic = null;
        }

        private void LateUpdate()
        {
            if (hasTopdownPathsToFind)
            {
                FindAllTopdownPaths();
            }
        }


        #region Utilities
        public bool IsValid(Vector2 worldPos, CellWorldPositionType cellWorldPositionType, PathFindingType pathFindingType, string key = "")
        {
            if (pathFindingType == PathFindingType.Platformer)
            {
                return _platformerGrid.IsInsideGrid(worldPos, cellWorldPositionType);
            }
            else
            {
                return _topdownGridDynamic.IsInsideGrid(worldPos, cellWorldPositionType);
            }
        }


        /// <summary>
        /// Return the equivalent world position in the path finding grid.
        /// </summary>
        public int2 GetGridPosition(Vector2 worldPosition, PathFindingType pathFindingType)
        {
            if (pathFindingType == PathFindingType.Platformer)
            {
                return _platformerGrid.GetGridPosition(worldPosition);
            }
            else
            {
                return _topdownGridDynamic.GetGridPosition(worldPosition);
            }
        }


        /// <summary>
        /// Return the equivalent wolrd position from a path finding's grid position.
        /// </summary>
        /// <param name="x">Grid position X.</param>
        /// <param name="y">Grid position Y.</param>
        /// <param name="cellWorldPositionType">Type of wolrd position wanted.</param>
        public Vector2 GetWorldPosition(int x, int y, CellWorldPositionType cellWorldPositionType, PathFindingType pathFindingType)
        {
            if (pathFindingType == PathFindingType.Platformer)
            {
                return _platformerGrid.GetWorldPosition(x, y, cellWorldPositionType);
            }
            else
            {
                return _topdownGridDynamic.GetWorldPosition(x, y, cellWorldPositionType);
            }
        }


        /// <summary>
        /// Return the grid node in a world position. (Not necesseraily a valid position)
        /// </summary>
        /// <param name="gridPosition">The grid position.</param>
        /// <returns></returns>
        public GridPathNode GetNode(int2 gridPosition, PathFindingType pathFindingType)
        {
            if (pathFindingType == PathFindingType.Platformer)
            {
                return _platformerGrid.GetValue(gridPosition);
            }
            else
            {
                return _topdownGridDynamic.GetValue(gridPosition);
            }
        }


        /// <summary>
        /// Return the grid node in a world position. (Not necesseraily a valid position)
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns></returns>
        public GridPathNode GetNode(Vector2 worldPosition, CellWorldPositionType cellWorldPositionType, PathFindingType pathFindingType)
        {
            if (pathFindingType == PathFindingType.Platformer)
            {
                return _platformerGrid.GetValue(worldPosition, cellWorldPositionType);
            }
            else
            {
                return _topdownGridDynamic.GetValue(worldPosition, cellWorldPositionType);
            }
        }
        #endregion


        #region AtualizeMap
        /// <summary>
        /// Fill all pathfindGrind with atualized GridNodes.
        /// </summary>
        public void AtualizeGridNodes()
        {
            if (platformerFoldout)
                AtualizePlatformGridNodes();
            else if (topdownFoldout)
                AtualizeTopdownGridNodes();
        }

        private void AtualizePlatformGridNodes()
        {
            void DividePlatformIds(int firstX, int y, int cellCount, int id)
            {
                if (id < 0)
                    return;

                PlatformerBaseGraph.AddVertex(new PlatformerNode(id, false));
                PlatformerBaseGraph.AddVertex(new PlatformerNode(id, true));

                int half = firstX + cellCount / 2;
                int last = firstX + cellCount;
                for (int x = firstX; x < last; x++)
                {
                    bool atualSection = x > half;

                    int2 position = new int2(x, y);
                    _platformerGrid.SetValue(x, y, new GridPathNode(position, true, id, atualSection));
                }
            }

            int atualId = -1;
            bool lastIsWalkable = false;
            int lastYPos = 0;
            int lastWalkablesCount = 0;
            int startX = 0;

            for (int y = 0; y < platformerGridSize.y; y++)
            {
                for (int x = 0; x < platformerGridSize.x; x++)
                {
                    bool isWall = OverlapWithObstacle(x, y, _platformerGrid, platformerNodeSize, platformerTolerance, platformerObstacles);
                    bool isWalkable = !isWall && _platformerGrid.IsInsideGrid(x, y - 1) && OverlapWithObstacle(x, y - 1, _platformerGrid, platformerNodeSize, platformerTolerance, platformerObstacles);

                    if (isWalkable)
                    {
                        if (!lastIsWalkable || lastYPos != y)
                        {
                            atualId++;
                            lastWalkablesCount = 0;
                            startX = x;
                        }
                        lastWalkablesCount++;
                    }
                    else
                    {
                        if (lastIsWalkable)
                            DividePlatformIds(startX, lastYPos, lastWalkablesCount, atualId);

                        lastWalkablesCount = 0;
                    }

                    if (!isWalkable)
                    {
                        int2 position = new int2(x, y);
                        _platformerGrid.SetValue(position, new GridPathNode(position, isWalkable, atualId));
                    }

                    lastIsWalkable = isWalkable;
                    lastYPos = y;
                }
            }
        }

        private void AtualizeTopdownGridNodes()
        {
            for (int y = 0; y < topdownGridSize.y; y++)
            {
                for (int x = 0; x < topdownGridSize.x; x++)
                {
                    bool isWall = OverlapWithObstacle(x, y, _topdownGridDynamic, topdownNodeSize, topdownTolerance, topdownObstacles);
                    bool isWalkable = !isWall;

                    int2 position = new int2(x, y);
                    _topdownGridDynamic.SetValue(position, new GridPathNode(position, isWalkable, -1));
                }
            }
        }

        public void _CreatePlatformGrid()
        {
            _platformerGrid = null;
            PlatformerBaseGraph = new PlatformerGraph<Vector2>();
            Vector2 originPosition = new Vector2(platformerCenter.x - (platformerGridSize.x / 2 * platformerNodeSize), platformerCenter.y - (platformerGridSize.y / 2 * platformerNodeSize));
            _platformerGrid = new MyGrid<GridPathNode>(platformerGridSize.x, platformerGridSize.y, platformerNodeSize, originPosition);
            AtualizePlatformGridNodes();
        }

        public void _CreateTopdownGrid()
        {
            _topdownGridDynamic = null;
            Vector2 originPosition = new Vector2(topdownCenter.x - (topdownGridSize.x / 2 * topdownNodeSize), topdownCenter.y - (topdownGridSize.y / 2 * topdownNodeSize));
            _topdownGridDynamic = new MyGrid<GridPathNode>(topdownGridSize.x, topdownGridSize.y, topdownNodeSize, originPosition);
            AtualizeTopdownGridNodes();
        }

        private static bool OverlapWithObstacle(int x, int y, MyGrid<GridPathNode> grid, float nodeSize, float tolerance, LayerMask obstacles)
        {
            Vector2 nodeCenter = grid.GetWorldPosition(x, y, CellWorldPositionType.Center);
            Vector2 size = new Vector2(nodeSize - (tolerance * nodeSize), nodeSize - (tolerance * nodeSize));
            return Physics2D.OverlapBox(nodeCenter, size, 0, obstacles);
        }
        #endregion


        #region FindTopdownPath
        /// <summary>
        /// Deserialize a topdown saved grid, so later its already loaded. Use this function when loading the scene.
        /// </summary>
        /// <param name="topdownDataKey">The key that you want to load.</param>
        /// <returns>True if loaded, false if not found or already loaded.</returns>
        public bool WarmUpTopdownKey(string topdownDataKey)
        {
            if (SaveSystem.ContainsKey(topdownDataKey) && !topdownSavedGrids.ContainsKey(topdownDataKey))
            {
                TopdownData topdownData = SaveSystem.LoadData<TopdownData>(topdownDataKey);
                topdownSavedGrids[topdownDataKey] = topdownData.grid;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Find the shortest path to a point based in a FindPathData. This executes instatly, but is slower. Use this function over 'AddPathToFind' when you need a path instantly.
        /// </summary>
        public TopdownPath FindTopdownPathNow(FindPathData findPathData)
        {
            NativeList<Vector2> shortestPath = new NativeList<Vector2>(Allocator.TempJob);
            FindPathJob findPathJob = CreateFindPathJob(findPathData, shortestPath);
            JobHandle jobHandle = findPathJob.Schedule();
            jobHandle.Complete();

            findPathJob.pathNodeArray.Dispose();

            return NativeListOfVector2ToPath(findPathJob.pathResult, findPathData.vectorPathType);
        }


        /// <summary>
        /// Schedule a path to be finded. Used only in TopDown path finding.
        /// </summary>
        /// <param name="gridDataKey">The key of the grid that you want to use.</param>
        /// <param name="findPathData">The data of the path that you want to find.</param>
        public bool AddPathToFind(FindPathData findPathData, OnTopdownPathComplete onPathComplete)
        {
            if (findPathData.useKey && !SaveSystem.ContainsKey(findPathData.gridDataKey))
            {
                throw new Exception("SaveSytem not contains key " + findPathData.gridDataKey);
            }
            MyGrid<GridPathNode> grid = GetGrid(findPathData);

            if (grid.IsInsideGrid(findPathData.startPos, CellWorldPositionType.Center) && grid.IsInsideGrid(findPathData.targetPos, CellWorldPositionType.Center))
            {
                topdownPathsToFind.Add(findPathData);
                topdownCompleteFunctions.Add(onPathComplete);
                hasTopdownPathsToFind = true;

                return true;
            }

            return false;
        }


        /// <summary>
        /// Return a topdown grid saved in a key.
        /// </summary>
        /// <returns></returns>
        public MyGrid<GridPathNode> GetTopdownGrid(string key, SaveKeyType saveKeyType)
        {
            MyGrid<GridPathNode> grid;
            if (topdownSavedGrids.ContainsKey(key))
            {
                grid = topdownSavedGrids[key];
            }
            else
            {
                grid = SaveSystem.LoadData<TopdownData>(key).grid;
                if (saveKeyType == SaveKeyType.Persistent)
                {
                    topdownSavedGrids[key] = grid;
                }
            }

            return grid;
        }


        /// <summary>
        /// Return the atual topdown grid. (Not a saved one).
        /// </summary>
        public MyGrid<GridPathNode> GetAtualTopdownGrid()
        {
            return _topdownGridDynamic;
        }

        public MyGrid<GridPathNode> GetGrid(FindPathData findPathData)
        {
            return findPathData.useKey ? GetTopdownGrid(findPathData.gridDataKey, findPathData.saveKeyType) : GetAtualTopdownGrid();
        }

        private FindPathJob CreateFindPathJob(FindPathData findPathData, NativeList<Vector2> result)
        {
            MyGrid<GridPathNode> savedGrid = GetGrid(findPathData);

            int2 gridSizeInt2 = new int2(savedGrid.Width, savedGrid.Height);

            // Continue from here after game jam
            int2 startPos = GetWalkableNode(savedGrid, findPathData.startPos, findPathData.boxColliderX, CellWorldPositionType.Center, RoundNodeType.SearchInAllDirections).gridPosition.ToInt2();
            int2 targetPos = savedGrid.GetGridPosition(findPathData.targetPos, CellWorldPositionType.Center);

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(savedGrid.Width * savedGrid.Height, Allocator.TempJob);
            for (int x = 0; x < savedGrid.Width; x++)
            {
                for (int y = 0; y < savedGrid.Height; y++)
                {
                    PathNode pathNode = new PathNode
                    {
                        x = x,
                        y = y,
                        index = x + y * savedGrid.Width,
                        gCost = int.MaxValue,
                        hCost = FindPathJob.CalculateDistanceCost(new int2(x, y), targetPos),
                        worldPos = savedGrid.GetWorldPosition(x, y, CellWorldPositionType.Center)
                    };
                    pathNode.fCost = int.MaxValue;

                    pathNode.cameFromIndex = -1;

                    bool isWalkable = savedGrid.GetValue(x, y).isWalkable;
                    pathNode.isWalkable = isWalkable;

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            FindPathJob outFindPathJob = new FindPathJob
            {
                findPathType = findPathData.findPathType,
                gridSize = gridSizeInt2,
                startPos = startPos,
                targetPos = targetPos,
                pathResult = result,
                pathNodeArray = pathNodeArray
            };

            return outFindPathJob;
        }


        private void FindAllTopdownPaths()
        {
            NativeList<Vector2>[] results = new NativeList<Vector2>[topdownPathsToFind.Count];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new NativeList<Vector2>(Allocator.TempJob);
            }

            // Define non walkable areas


            // Create Jobs
            FindPathJob[] findPathJobs = new FindPathJob[topdownPathsToFind.Count];
            for (int i = 0; i < topdownPathsToFind.Count; i++)
            {
                findPathJobs[i] = CreateFindPathJob(topdownPathsToFind[i], results[i]);
            }

            // Schedule and complete jobs
            NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(topdownPathsToFind.Count, Allocator.Temp);
            for (int i = 0; i < findPathJobs.Length; i++)
            {
                jobHandles[i] = findPathJobs[i].Schedule();
            }
            JobHandle.CompleteAll(jobHandles);

            // Get the output from the job
            for (int i = 0; i < topdownPathsToFind.Count; i++)
            {
                findPathJobs[i].pathNodeArray.Dispose();
                topdownCompleteFunctions[i].Invoke(NativeListOfVector2ToPath(findPathJobs[i].pathResult, topdownPathsToFind[i].vectorPathType));
            }

            jobHandles.Dispose();

            // Remove from pathsToFind
            topdownPathsToFind.Clear();
            topdownCompleteFunctions.Clear();
            hasTopdownPathsToFind = false;
        }


        /// <summary>
        /// Convert a NativeList'Vector2' into a TopdownPath. Also deallocate the NativeList'Vector2'.
        /// </summary>
        private TopdownPath NativeListOfVector2ToPath(NativeList<Vector2> vectorList, VectorPathType vectorPathType)
        {
            List<Vector2> pathList = new List<Vector2>();
            for (int j = 0; j < vectorList.Length; j++)
            {
                pathList.Add(vectorList[j]);
            }
            vectorList.Dispose();

            if (vectorPathType == VectorPathType.OptimizeSpace)
            {
                // Reduce repetitive points
                int xRepCount, yRepCount, upDiagRepCount, downDiagRepCount;
                xRepCount = yRepCount = upDiagRepCount = downDiagRepCount = 0;
                int i = 1;
                while (i < pathList.Count)
                {
                    Vector2 atual = pathList[i];
                    Vector2 previous = pathList[i - 1];
                    if (previous.y == atual.y)
                    {
                        xRepCount++;
                        yRepCount = 0;
                        upDiagRepCount = 0;
                        downDiagRepCount = 0;
                        if (xRepCount > 1)
                        {
                            pathList.RemoveAt(i - 1);
                            xRepCount = 1;
                            continue;
                        }
                    }
                    else if (previous.x == atual.x)
                    {
                        yRepCount++;
                        xRepCount = 0;
                        upDiagRepCount = 0;
                        downDiagRepCount = 0;
                        if (yRepCount > 1)
                        {
                            pathList.RemoveAt(i - 1);
                            yRepCount = 1;
                            continue;
                        }
                    }
                    else
                    {
                        Vector2 dir = previous - atual;
                        if (dir.x == dir.y)
                        {
                            upDiagRepCount++;
                            downDiagRepCount = 0;
                            xRepCount = 0;
                            yRepCount = 0;
                            if (upDiagRepCount > 1)
                            {
                                pathList.RemoveAt(i - 1);
                                upDiagRepCount = 1;
                                continue;
                            }
                        }
                        else
                        {
                            downDiagRepCount++;
                            upDiagRepCount = 0;
                            xRepCount = 0;
                            yRepCount = 0;
                            if (downDiagRepCount > 1)
                            {
                                pathList.RemoveAt(i - 1);
                                downDiagRepCount = 1;
                                continue;
                            }
                        }
                    }

                    i++;
                }
            }

            return new TopdownPath(pathList.ToArray());
        }
        #endregion


        #region FindPlatformerPath
        private PlatformerGraph<Vector2> GetPlatformGraph(string key, SaveKeyType saveKeyType)
        {
            PlatformerGraph<Vector2> graph;
            if (platformerSavedGraphs.ContainsKey(key))
            {
                graph = platformerSavedGraphs[key];
            }
            else
            {
                graph = SaveSystem.LoadData<GraphData>(key).Load();
                if (saveKeyType == SaveKeyType.Persistent)
                {
                    platformerSavedGraphs[key] = graph;
                }
            }

            return graph;
        }


        /// <summary>
        /// Deserialize a platformer saved graph, so later its already loaded. Use this function when loading the scene.
        /// </summary>
        /// <param name="platformerDataKey">The key that you want to load.</param>
        /// <returns>True if loaded, false if not found or already loaded.</returns>
        public bool WarmUpPlatformerKey(string platformerDataKey)
        {
            if (SaveSystem.ContainsKey(platformerDataKey) && !platformerSavedGraphs.ContainsKey(platformerDataKey))
            {
                GraphData graphData = SaveSystem.LoadData<GraphData>(platformerDataKey);
                platformerSavedGraphs[platformerDataKey] = graphData.Load();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Find the shortest path between platforms of a given graph.
        /// </summary>
        /// <param name="graphKey">The graph key that will be used to find a path.</param>
        /// <param name="startPos">The From position.</param>
        /// <param name="targetPos">The Target position.</param>
        /// <param name="colliderSizeX">The size of the gameObject collider in units. Should be multiplied by scale.</param>
        /// <param name="saveKeyType">The type of allocation of the graph used to find a path. Persistent will save the graph to use later if needed. Once will                                    not.</param>
        /// <param name="roundNodeType">The type of rounding to apply to the start and target position. Useful when a position is not valid.</param>
        /// <returns></returns>
        public PlatformerPath FindPlatfornerPath(string graphKey, Vector2 startPos, Vector2 targetPos, float colliderSizeX,
                                        SaveKeyType saveKeyType,
                                        RoundNodeType roundNodeType = RoundNodeType.None)
        {
            if (pathFindingType == PathFindingType.Topdown)
                throw new Exception("PathFindinType is set to Topdown but you are trying to find a Platformer path.");

            PlatformerGraph<Vector2> graph = GetPlatformGraph(graphKey, saveKeyType);

            if (graph.EdgesCount == 0)
            {
                Debug.Log("Graph has 0 edges.");
                return null;
            }

            GridPathNode startNode = GetWalkableNode(_platformerGrid, startPos, colliderSizeX, CellWorldPositionType.Center, roundNodeType);
            GridPathNode targetNode = GetWalkableNode(_platformerGrid, targetPos, colliderSizeX, CellWorldPositionType.Center, roundNodeType);


            int ArrayPos(PlatformerNode platformNode)
            {
                if (platformNode.section)
                    return platformNode.platformId * 2 + 1;
                else
                    return platformNode.platformId * 2;
            }


            PlatformerNode startVertex = new PlatformerNode(startNode.PlatformId, startNode.Section);
            PlatformerNode targetVertex = new PlatformerNode(targetNode.PlatformId, targetNode.Section);

            if (startVertex.IsSamePlatform(targetVertex))
            {
                return null;
            }

            int verticesCount = graph.VerticesCount;

            float[] pathCosts = new float[verticesCount];
            PlatformerNode[] previousVertices = new PlatformerNode[verticesCount];
            HashSet<PlatformerNode> visitedVertices = new HashSet<PlatformerNode>();
            FastPriorityQueue<FindPathQueueNode> verticesQueue = new FastPriorityQueue<FindPathQueueNode>(verticesCount);

            PlatformerNode undefined = new PlatformerNode(-1, false); // Represent an undefined node. Like "null".
            for (int i = 0; i < verticesCount; i++)
            {
                pathCosts[i] = float.MaxValue;
                previousVertices[i] = undefined;
            }
            pathCosts[ArrayPos(startVertex)] = 0;
            verticesQueue.Enqueue(new FindPathQueueNode(startVertex), 0);
            visitedVertices.Add(startVertex);

            while (verticesQueue.Count != 0)
            {
                PlatformerNode atualVertex = verticesQueue.Dequeue().vertex;

                int arrayPosAV = ArrayPos(atualVertex);
                foreach (PlatformerEdge<Vector2> edge in graph.AdjacencyList[atualVertex])
                {
                    float tempPathCost = pathCosts[arrayPosAV] + edge.wheight + (edge.fromPosition - edge.toPosition).magnitude + (edge.toPosition - targetPos).magnitude; // F cost
                    //float tempPathCost = pathCosts[arrayPosAV] + edge.wheight + edge.fromPosition.ManhattanDistanceTo(edge.toPosition);

                    PlatformerNode toVertex = edge.to;
                    int arrayPosTV = ArrayPos(toVertex);
                    if (pathCosts[arrayPosTV] > tempPathCost)
                    {
                        pathCosts[arrayPosTV] = tempPathCost;
                        previousVertices[ArrayPos(toVertex)] = atualVertex;

                        if (!visitedVertices.Contains(toVertex))
                        {
                            verticesQueue.Enqueue(new FindPathQueueNode(toVertex), tempPathCost);
                            visitedVertices.Add(toVertex);
                        }
                    }
                }

                PlatformerNode vertex2 = atualVertex.GetOtherSection();
                float tempPathCostVertex2 = pathCosts[arrayPosAV];

                if (graph.AdjacencyList[vertex2].Count == 0 || graph.AdjacencyList[atualVertex].Count == 0)
                {
                    if (vertex2.IsSamePlatform(targetVertex))
                        tempPathCostVertex2 += 1f; // Increasing just a little.
                }
                else
                {
                    //tempPathCostVertex2 += (graph.AdjacencyList[atualVertex][0].fromPosition - graph.AdjacencyList[vertex2][0].fromPosition).magnitude;
                    tempPathCostVertex2 += graph.AdjacencyList[atualVertex][0].fromPosition.ManhattanDistanceTo(graph.AdjacencyList[vertex2][0].fromPosition);
                }

                int arrayPosV2 = ArrayPos(vertex2);
                if (pathCosts[arrayPosV2] > tempPathCostVertex2)
                {
                    pathCosts[arrayPosV2] = tempPathCostVertex2;
                    previousVertices[arrayPosV2] = atualVertex;

                    if (!visitedVertices.Contains(vertex2))
                    {
                        verticesQueue.Enqueue(new FindPathQueueNode(vertex2), tempPathCostVertex2);
                        visitedVertices.Add(vertex2);
                    }
                }

                if (atualVertex.IsSamePlatform(targetVertex))
                    break;
            }

            if (previousVertices[ArrayPos(targetVertex)].IsSameSection(undefined))
            {
                // Not found a path
                Debug.Log("Not found a path.");
                return null;
            }

            // BackTrack in the path
            List<PlatformerEdge<Vector2>> path = new List<PlatformerEdge<Vector2>>();

            PlatformerNode currentVertex = targetVertex;
            PlatformerNode previousVertex = previousVertices[ArrayPos(targetVertex)];
            while (!previousVertex.IsSameSection(undefined))
            {
                if (!previousVertex.IsSamePlatform(currentVertex))
                {// Edge not exist because is the same platform
                    path.Insert(0, graph.FindEdge(previousVertex, currentVertex));
                }

                currentVertex = previousVertex;
                previousVertex = previousVertices[ArrayPos(currentVertex)];
            }

            return new PlatformerPath(path.ToArray());
        }

        #endregion


        #region GetWalkableNode
        /// <summary>
        /// Get a walkable node. Handles wolrdPos imprecision by looking into the nodes in the left and in the right.
        /// </summary>
        private GridPathNode GetWalkableNode(MyGrid<GridPathNode> grid, Vector2 position, float colliderSizeX, CellWorldPositionType cellWorldPositionType, RoundNodeType roundNodeType)
        {
            int2 centerGridPos = grid.GetGridPosition(position, cellWorldPositionType);
            GridPathNode centerNode = grid.GetValue(centerGridPos);
            if (centerNode.isWalkable)
                return centerNode;

            float colliderSizeXHalf = colliderSizeX / 2f;
            Vector2 posA = new Vector2(position.x + colliderSizeXHalf, position.y);
            Vector2 posB = new Vector2(position.x - colliderSizeXHalf, position.y);

            GridPathNode nodeA = grid.GetValue(posA, cellWorldPositionType);
            if (nodeA.isWalkable)
                return nodeA;

            GridPathNode nodeB = grid.GetValue(posB, cellWorldPositionType);
            if (nodeB.isWalkable)
                return nodeB;

            if (roundNodeType == RoundNodeType.None)
            {
                throw new System.Exception("Start node need be walkable.");
            }
            else if (roundNodeType == RoundNodeType.SearchDown)
            {
                return NearestDownWalkableNode(centerGridPos, grid);
            }
            else // roundNodeType == RoundNodeType.RoundInAllDirections
            {
                return NearestInAllDirections(centerNode, grid);
            }
        }


        /// <summary>
        /// Get the nearest walkable node in down direction.
        /// </summary>
        private GridPathNode NearestDownWalkableNode(int2 atualPos, MyGrid<GridPathNode> grid)
        {
            atualPos = new int2(atualPos.x, atualPos.y - 1);
            while (atualPos.x < grid.Width  && atualPos.x >= 0 && atualPos.y >= 0)
            {
                if (atualPos.y < grid.Height)
                {
                    GridPathNode downNode = grid.GetValue(atualPos);
                    if (downNode.isWalkable)
                        return downNode;
                }

                atualPos = new int2(atualPos.x, atualPos.y - 1);
            }

            throw new Exception("Can't find a nearest down node that is walkable.");
        }


        /// <summary>
        /// Get the nearest walkable node in all directions. (Just look around one node.)
        /// </summary>
        private GridPathNode NearestInAllDirections(GridPathNode atualNode, MyGrid<GridPathNode> grid)
        {
            NativeArray<FindNearestPathNode> pathNodeArray = new NativeArray<FindNearestPathNode>(grid.Width * grid.Height, Allocator.TempJob);
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    FindNearestPathNode findNearestPathNode = new FindNearestPathNode()
                    {
                        x = x,
                        y = y
                    };
                    GridPathNode gridPathNode = grid.GetValue(x, y);
                    findNearestPathNode.isWalkable = gridPathNode.isWalkable;

                    pathNodeArray[x + y * grid.Width] = findNearestPathNode;
                }
            }

            NativeArray<int2> resultNodePos = new NativeArray<int2>(1, Allocator.TempJob);

            FindNearestWalkablejob nearestWalkablejob = new FindNearestWalkablejob()
            {
                pathNodeArray = pathNodeArray,
                gridSize = new int2(grid.Width, grid.Height),
                startPos = atualNode.gridPosition.ToInt2(),
                resultNodePos = resultNodePos
            };

            JobHandle jobHandle = nearestWalkablejob.Schedule();
            jobHandle.Complete();

            int2 resultGridPos = nearestWalkablejob.resultNodePos[0];
            resultNodePos.Dispose();
            pathNodeArray.Dispose();

            return grid.GetValue(resultGridPos);
        }

        #endregion
    }

    /// <summary>
    /// Used in FindPath function
    /// </summary>
    class FindPathQueueNode : FastPriorityQueueNode
    {
        public PlatformerNode vertex;

        public FindPathQueueNode(PlatformerNode vertex)
        {
            this.vertex = vertex;
        }
    }
}
