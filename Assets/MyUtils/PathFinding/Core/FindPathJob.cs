using My_Utils.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace My_Utils.PathFinding
{
    [BurstCompile]
    public struct FindPathJob : IJob
    {
        private const int MOVE_DIAGONAL_COST = 14;
        private const int MOVE_STRAIGHT_COST = 10;
        public NativeArray<PathNode> pathNodeArray;

        public int2 gridSize;

        public int2 startPos;
        public int2 targetPos;
        public FindPathType findPathType;
        public NativeList<Vector2> pathResult;


        private int ArrayPos(int x, int y)
        {
            return x + y * gridSize.x;
        }

        private int ArrayPos(int2 gridPos)
        {
            return ArrayPos(gridPos.x, gridPos.y);
        }

        private PathNode GetNode(int2 gridPos)
        {
            return pathNodeArray[ArrayPos(gridPos)];
        }

        private bool IsValid(int2 position)
        {
            return position.x >= 0 &&
                position.y >= 0 &&
                position.x < gridSize.x &&
                position.y < gridSize.y;
        }

        public void Execute()
        {
            // Invert StartPos and EndPos, so your path won't be inverted in the end
            int2 startPosAux = startPos;
            startPos = targetPos;
            targetPos = startPosAux;

            NativeArray<int2> neighboursOffsets;
            if (findPathType == FindPathType.Normal) // Allow diagonals
            {
                neighboursOffsets = new NativeArray<int2>(8, Allocator.Temp);
                neighboursOffsets[0] = new int2(0, 1); // Up
                neighboursOffsets[1] = new int2(0, -1); // Down
                neighboursOffsets[6] = new int2(-1, 0); // Left
                neighboursOffsets[3] = new int2(1, 0); // Right
                neighboursOffsets[2] = new int2(1, 1); // Right Up
                neighboursOffsets[4] = new int2(1, -1); // Right Down
                neighboursOffsets[5] = new int2(-1, 1); // Left Up
                neighboursOffsets[7] = new int2(-1, -1); // Left Down
            }
            else // NonDiagonals
            {
                neighboursOffsets = new NativeArray<int2>(4, Allocator.Temp);
                neighboursOffsets[0] = new int2(0, 1); // Up
                neighboursOffsets[1] = new int2(0, -1); // Down
                neighboursOffsets[2] = new int2(1, 0); // Right
                neighboursOffsets[3] = new int2(-1, 0); // Left
            }

            int endNodeIndex = ArrayPos(targetPos.x, targetPos.y);

            PathNode startNode = pathNodeArray[ArrayPos(startPos.x, startPos.y)];
            startNode.gCost = 0;
            startNode.AtualizeFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openListIndexes = new NativeList<int>(Allocator.Temp);
            NativeHashSet<int> closedListIndexes = new NativeHashSet<int>(pathNodeArray.Length, Allocator.Temp);

            openListIndexes.Add(startNode.index);

            while (openListIndexes.Length > 0)
            {
                // Get lowest fCost
                int currentNodeIndex = GetLowestFCostIndex(openListIndexes);

                if (currentNodeIndex == endNodeIndex)
                {
                    // Found endPos.
                    break;
                }

                // Remove currentIndex from open list and add to close list
                for (int i = 0; i < openListIndexes.Length; i++)
                {
                    if (openListIndexes[i] == currentNodeIndex)
                    {
                        openListIndexes.RemoveAtSwapBack(i);
                        break;
                    }
                }
                closedListIndexes.TryAdd(currentNodeIndex);

                // Add the neighbours to openList
                PathNode currentNode = pathNodeArray[currentNodeIndex];
                for (int i = 0; i < neighboursOffsets.Length; i++)
                {
                    int2 neighbourOffset = neighboursOffsets[i];
                    int2 neighbourPos = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsValid(neighbourPos))
                        continue;

                    int neighbourIndex = ArrayPos(neighbourPos.x, neighbourPos.y);
                    if (closedListIndexes.Contains(neighbourIndex))
                        continue;

                    PathNode neighbourNode = pathNodeArray[neighbourIndex];
                    if (!neighbourNode.isWalkable)
                        continue;

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPos);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.AtualizeFCost();
                        neighbourNode.cameFromIndex = currentNodeIndex;
                        pathNodeArray[neighbourIndex] = neighbourNode;
                        openListIndexes.Add(neighbourIndex);
                    }
                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromIndex == -1)
            {
                // Couldn't find a path
                //Debug.Log("Couldn't find a path.");
            }
            else
            {
                // Path found
                CalculatePathThatCameFrom(endNode);
            }

            neighboursOffsets.Dispose();
            openListIndexes.Dispose();
            closedListIndexes.Dispose();
        }

        /// <summary>
        /// Calculate the path finded and apply it to pathResult
        /// </summary>
        private void CalculatePathThatCameFrom(PathNode endNode)
        {
            pathResult.Add(endNode.worldPos);

            PathNode currentNode = endNode;
            while (currentNode.cameFromIndex != -1)
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromIndex];
                pathResult.Add(cameFromNode.worldPos);
                currentNode = cameFromNode;
            }
        }

        private int GetLowestFCostIndex(NativeList<int> openListIndexes)
        {
            PathNode lowestFCostNode = pathNodeArray[openListIndexes[0]];
            for (int i = 0; i < openListIndexes.Length; i++)
            {
                PathNode atualPathNode = pathNodeArray[openListIndexes[i]];
                if (atualPathNode.fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = atualPathNode;
                }
            }
            return lowestFCostNode.index;
        }

        public static int CalculateDistanceCost(int2 A, int2 B)
        {
            int xDistance = math.abs(A.x - B.x);
            int yDistance = math.abs(A.y - B.y);
            int straightPos = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + straightPos * MOVE_STRAIGHT_COST;
        }

    }
    public struct PathNode
    {
        public int index;

        public int x;
        public int y;
        public Vector2 worldPos;

        public int gCost;
        public int hCost;
        public int fCost;

        public int cameFromIndex;
        public bool isWalkable;

        public void AtualizeFCost()
        {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            this.isWalkable = isWalkable;
        }
    }
}
