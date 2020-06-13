using Unity.Mathematics;
using UnityEngine;

namespace My_Utils.PathFinding
{
    public class Seeker : MonoBehaviour
    {
        [Tooltip("Mark to draw the path with gizmos.")]
        public bool pathIsVisible;
        public bool IsWorking { get; private set; }

        private PlatformerPath platformerPath;
        private TopdownPath topdownPath;

        private PathFindingManager pathFindingManager;

        private void Awake()
        {
            pathFindingManager = FindObjectOfType<PathFindingManager>();
        }


        /// <summary>
        /// Start to search for a path (if not alredy searching another path) and callback when the path it's finished.
        /// </summary>
        /// <param name="startPos">The start position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <param name="boxColliderX">The size of your collider in X axis.</param>
        /// <param name="findPathType">The type of path that you want to find. With or without diagonals.</param>
        /// <param name="onPathComplete">The function that you execute and receive the path when the path is found.</param>
        public void FindTopdownPath(Vector2 startPos, Vector2 targetPos, float boxColliderX, FindPathType findPathType, VectorPathType vectorPathType, OnTopdownPathComplete onPathComplete)
        {
            if (!IsWorking)
            {
                FindPathData findPathData = new FindPathData()
                {
                    startPos = startPos,
                    targetPos = targetPos,
                    findPathType = findPathType,
                    boxColliderX = boxColliderX,
                    vectorPathType = vectorPathType
                };

                IsWorking = pathFindingManager.AddPathToFind(findPathData, (TopdownPath topdownPath) =>
                {
                    this.topdownPath = topdownPath;
                    onPathComplete.Invoke(topdownPath);
                    IsWorking = false;
                });
            }
        }


        /// <summary>
        /// Start to search for a path (if not alredy searching another path) and callback when the path it's finished.
        /// </summary>
        /// <param name="startPos">The start position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <param name="boxColliderX">The size of your collider in X axis.</param>
        /// <param name="findPathType">The type of path that you want to find. With or without diagonals.</param>
        /// <param name="onPathComplete">The function that you execute and receive the path when the path is found.</param>
        public void FindTopdownPath(string key, SaveKeyType saveKeyType, Vector2 startPos, Vector2 targetPos, float boxColliderX, FindPathType findPathType, VectorPathType vectorPathType, OnTopdownPathComplete onPathComplete)
        {
            if (!IsWorking)
            {
                FindPathData findPathData = new FindPathData()
                {
                    useKey = true,
                    gridDataKey = key,
                    saveKeyType = saveKeyType,
                    startPos = startPos,
                    targetPos = targetPos,
                    findPathType = findPathType,
                    boxColliderX = boxColliderX,
                    vectorPathType = vectorPathType
                };

                IsWorking = pathFindingManager.AddPathToFind(findPathData, (TopdownPath topdownPath) =>
                {
                    this.topdownPath = topdownPath;
                    onPathComplete.Invoke(topdownPath);
                    IsWorking = false;
                });
            }
        }


        /// <summary>
        /// Find the shortest path to a point based in a FindPathData. This executes instatly, but is slower. Use this function over 'AddPathToFind' when you need a path instantly.
        /// </summary>
        /// <param name="gridDataKey">The key to the saved grid that will be used to find the path.</param>
        /// <param name="saveKeyType">How you want the key data to be saved. Used an deleted, or saved to later uses (so it doesn't needs to deserialize again).</param>
        /// <param name="startPos">The start position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <param name="boxColliderX">The size of your collider in X axis.</param>
        /// <param name="findPathType">The type of path that you want to find. With or without diagonals.</param>
        public TopdownPath FindTopdownPathNow(Vector2 startPos, Vector2 targetPos, float boxColliderX, FindPathType findPathType, VectorPathType vectorPathType)
        {
            FindPathData findPathData = new FindPathData()
            {
                startPos = startPos,
                targetPos = targetPos,
                findPathType = findPathType,
                boxColliderX = boxColliderX,
                vectorPathType = vectorPathType
            };

            TopdownPath findedPath = pathFindingManager.FindTopdownPathNow(findPathData);
            topdownPath = findedPath;
            return findedPath;
        }


        /// <summary>
        /// Find the shortest path to a point based in a FindPathData. This executes instatly, but is slower. Use this function over 'AddPathToFind' when you need a path instantly.
        /// </summary>
        /// <param name="gridDataKey">The key to the saved grid that will be used to find the path.</param>
        /// <param name="saveKeyType">How you want the key data to be saved. Used an deleted, or saved to later uses (so it doesn't needs to deserialize again).</param>
        /// <param name="startPos">The start position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <param name="boxColliderX">The size of your collider in X axis.</param>
        /// <param name="findPathType">The type of path that you want to find. With or without diagonals.</param>
        public TopdownPath FindTopdownPathNow(string key, SaveKeyType saveKeyType, Vector2 startPos, Vector2 targetPos, float boxColliderX, FindPathType findPathType, VectorPathType vectorPathType)
        {
            FindPathData findPathData = new FindPathData()
            {
                useKey = true,
                gridDataKey = key,
                saveKeyType = saveKeyType,
                startPos = startPos,
                targetPos = targetPos,
                findPathType = findPathType,
                boxColliderX = boxColliderX,
                vectorPathType = vectorPathType
            };

            TopdownPath findedPath = pathFindingManager.FindTopdownPathNow(findPathData);
            topdownPath = findedPath;
            return findedPath;
        }


        /// <summary>
        /// Return the shortest platformer path.
        /// </summary>
        /// <param name="graphKey">The key of the saved graph that you want to use to find the path.</param>
        /// <param name="startPos">The start position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <param name="checkGroundSize">The check ground size in the X axis.</param>
        /// <param name="roundNodeType">The type of round optimization applied case not found a valid node.</param>
        /// <param name="allocateGraphType">The type of graph allocation. Persistent will save grapjh to use later if solicited the same key.</param>
        /// <returns>A platformer path.</returns>
        public PlatformerPath FindPlatformPath(string graphKey, Vector2 startPos, Vector2 targetPos, float checkGroundSize,
                RoundNodeType roundNodeType = RoundNodeType.None, SaveKeyType allocateGraphType = SaveKeyType.Persistent)
        {
            platformerPath = pathFindingManager.FindPlatfornerPath(graphKey, startPos, targetPos, checkGroundSize, allocateGraphType, roundNodeType);
            return platformerPath;
        }


        private void OnDrawGizmos()
        {
            if (pathIsVisible)
            {
                if (platformerPath != null)
                {
                    Gizmos.color = Color.green;
                    foreach (PlatformerEdge<Vector2> edge in platformerPath.EdgePath)
                    {
                        MyUtils.DrawArrow(edge.fromPosition, edge.toPosition, 1, 27, DrawType.Gizmos);
                    }
                }
                if (topdownPath != null)
                {
                    Gizmos.color = Color.green;
                    for (int i = 0; i < topdownPath.VectorPath.Length - 1; i++)
                    {
                        MyUtils.DrawArrow(topdownPath.VectorPath[i], topdownPath.VectorPath[i + 1], 1, 27, DrawType.Gizmos);
                    }
                }
            }
        }

        #region Utilities
        public bool IsValid(Vector2 worldPosition, CellWorldPositionType cellWorldPositionType, PathFindingType pathFindingType)
        {
            return pathFindingManager.IsValid(worldPosition, cellWorldPositionType, pathFindingType);
        }


        /// <summary>
        /// Return the equivalent world position in the path finding grid.
        /// </summary>
        public int2 GetGridPosition(Vector2 worldPosition, PathFindingType pathFindingType)
        {
            return pathFindingManager.GetGridPosition(worldPosition, pathFindingType);
        }


        /// <summary>
        /// Return the equivalent wolrd position from a path finding's grid position.
        /// </summary>
        /// <param name="x">Grid position X.</param>
        /// <param name="y">Grid position Y.</param>
        /// <param name="cellWorldPositionType">Type of wolrd position wanted.</param>
        public Vector2 GetWorldPosition(int x, int y, CellWorldPositionType cellWorldPositionType, PathFindingType pathFindingType)
        {
            return pathFindingManager.GetWorldPosition(x, y, cellWorldPositionType, pathFindingType);
        }


        /// <summary>
        /// Return the equivalent wolrd position from a path finding's grid position.
        /// </summary>
        /// <param name="x">Grid position X.</param>
        /// <param name="y">Grid position Y.</param>
        /// <param name="cellWorldPositionType">Type of wolrd position wanted.</param>
        public Vector2 GetWorldPosition(int2 gridPosition, CellWorldPositionType cellWorldPositionType, PathFindingType pathFindingType)
        {
            return pathFindingManager.GetWorldPosition(gridPosition.x, gridPosition.y, cellWorldPositionType, pathFindingType);
        }


        /// <summary>
        /// Return the grid node in a world position. (Not necesseraily a valid position)
        /// </summary>
        /// <returns></returns>
        public GridPathNode GetNode(int x, int y, PathFindingType pathFindingType)
        {
            return pathFindingManager.GetNode(new int2(x, y), pathFindingType);
        }


        /// <summary>
        /// Return the grid node in a world position. (Not necesseraily a valid position)
        /// </summary>
        /// <param name="grisPosition">The grid position.</param>
        /// <returns></returns>
        public GridPathNode GetNode(int2 grisPosition, PathFindingType pathFindingType)
        {
            return pathFindingManager.GetNode(grisPosition, pathFindingType);
        }


        /// <summary>
        /// Return the grid node in a world position. (Not necesseraily a valid position)
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns></returns>
        public GridPathNode GetNode(Vector2 worldPosition, CellWorldPositionType cellWorldPositionType, PathFindingType pathFindingType)
        {
            return pathFindingManager.GetNode(worldPosition, cellWorldPositionType, pathFindingType);
        }


        /// <summary>
        /// Return the base graph registered in the global PathFindingMap
        /// </summary>
        /// <returns></returns>
        public PlatformerGraph<Vector2> GetBaseGraph()
        {
            return pathFindingManager.PlatformerBaseGraph;
        }
        #endregion
    }
}
