using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace My_Utils.PathFinding
{
    [BurstCompile]
    public struct TopdownFindPathJob : IJob
    {
        private const int MOVE_DIAGONAL_COST = 14;
        private const int MOVE_STRAIGHT_COST = 10;

        public float nodeSize;
        public float2 boxColliderSize;
        public int2 gridSize;
        public NativeArray<bool> obstacleNodes;
        public int2 startPos;
        public int2 targetPos;
        public FindPathType findPathType;
        public NativeList<int2> pathResult;

        public void Execute()
        {
            // Invert StartPos and EndPos, so your path won't be inverted in the end
            int2 startPosAux = startPos;
            startPos = targetPos;
            targetPos = startPosAux;

            // Instantiate grid
            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode
                    {
                        x = x,
                        y = y,
                        index = MyUtils.PositionToIndex(x, y, gridSize.x),

                        gCost = int.MaxValue,
                        hCost = CalculateDistanceCost(new int2(x, y), targetPos)
                    };
                    pathNode.AtualizeFCost();

                    pathNode.cameFromIndex = -1;

                    pathNode.isWalkable = obstacleNodes[pathNode.index];

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

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

            int endNodeIndex = MyUtils.PositionToIndex(targetPos.x, targetPos.y, gridSize.x);

            PathNode startNode = pathNodeArray[MyUtils.PositionToIndex(startPos.x, startPos.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.AtualizeFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openListIndexes = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedListIndexes = new NativeList<int>(Allocator.Temp);

            openListIndexes.Add(startNode.index);

            while (openListIndexes.Length > 0)
            {
                // Get lowest fCost
                int currentNodeIndex = GetLowestFCostIndex(openListIndexes, pathNodeArray);

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
                closedListIndexes.Add(currentNodeIndex);

                // Add the neighbours to openList
                PathNode currentNode = pathNodeArray[currentNodeIndex];
                for (int i = 0; i < neighboursOffsets.Length; i++)
                {
                    int2 neighbourOffset = neighboursOffsets[i];
                    int2 neighbourPos = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionValid(neighbourPos, gridSize))
                    {
                        continue;
                    }

                    int neighbourIndex = MyUtils.PositionToIndex(neighbourPos.x, neighbourPos.y, gridSize.x);
                    if (closedListIndexes.Contains(neighbourIndex))
                    {
                        // Already searched this node
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourIndex];
                    if (!neighbourNode.isWalkable)
                    {
                        continue;
                    }
                    if (boxColliderSize.x > nodeSize)
                    {
                        bool shouldContinue1 = false;
                        for (int j = 0; j < neighboursOffsets.Length; j++)
                        {
                            int2 pos = new int2(neighbourPos.x + neighboursOffsets[j].x, neighbourPos.y + neighboursOffsets[j].y);
                            if (IsPositionValid(pos, gridSize))
                            {
                                int ind = MyUtils.PositionToIndex(pos.x, pos.y, gridSize.x);
                                PathNode subNeighbour = pathNodeArray[ind];
                                if (!subNeighbour.isWalkable)
                                {
                                    shouldContinue1 = true;
                                    break;
                                }
                            }
                        }
                        if (shouldContinue1)
                            continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPos);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.AtualizeFCost();
                        neighbourNode.cameFromIndex = currentNodeIndex;
                        pathNodeArray[neighbourIndex] = neighbourNode;

                        if (!openListIndexes.Contains(neighbourIndex))
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
                CalculatePathThatCameFrom(endNode, pathNodeArray);
            }

            neighboursOffsets.Dispose();
            openListIndexes.Dispose();
            closedListIndexes.Dispose();
            pathNodeArray.Dispose();
        }

        /// <summary>
        /// Calculate the path finded and apply it to pathResult
        /// </summary>
        private void CalculatePathThatCameFrom(PathNode endNode, NativeArray<PathNode> pathNodeArray)
        {
            pathResult.Add(new int2(endNode.x, endNode.y));

            PathNode currentNode = endNode;
            while (currentNode.cameFromIndex != -1)
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromIndex];
                pathResult.Add(new int2(cameFromNode.x, cameFromNode.y));
                currentNode = cameFromNode;
            }
        }

        private bool IsPositionValid(int2 position, int2 gridSize)
        {
            return position.x >= 0 &&
                position.y >= 0 &&
                position.x < gridSize.x &&
                position.y < gridSize.y;
        }

        private int GetLowestFCostIndex(NativeList<int> openListIndexes, NativeArray<PathNode> pathNodeArray)
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

        private int CalculateDistanceCost(int2 A, int2 B)
        {
            int xDistance = math.abs(A.x - B.x);
            int yDistance = math.abs(A.y - B.y);
            int straightPos = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + straightPos * MOVE_STRAIGHT_COST;
        }

        private struct PathNode
        {
            public int index;

            public int x;
            public int y;

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
}
