using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

namespace My_Utils.PathFinding
{
    [BurstCompile]
    public struct FindNearestWalkablejob : IJob
    {
        public NativeArray<FindNearestPathNode> pathNodeArray;
        public int2 gridSize;
        public int2 startPos;
        public NativeArray<int2> resultNodePos;

        private int ArrayIndex(int x, int y)
        {
            return x + y * gridSize.x;
        }

        private bool IsValid(int2 position)
        {
            return position.x >= 0 && position.x < gridSize.x &&
                    position.y >= 0 && position.y < gridSize.y;
        }

        public void Execute()
        {
            NativeArray<int2> neighboursOffsets = new NativeArray<int2>(8, Allocator.Temp);
            neighboursOffsets[0] = new int2(0, 1); // Up
            neighboursOffsets[1] = new int2(0, -1); // Down
            neighboursOffsets[6] = new int2(-1, 0); // Left
            neighboursOffsets[3] = new int2(1, 0); // Right
            neighboursOffsets[2] = new int2(1, 1); // Right Up
            neighboursOffsets[4] = new int2(1, -1); // Right Down
            neighboursOffsets[5] = new int2(-1, 1); // Left Up
            neighboursOffsets[7] = new int2(-1, -1); // Left Down

            for (int i = 0; i < neighboursOffsets.Length; i++)
            {
                int2 atualNeighbourPos = new int2(startPos.x + neighboursOffsets[i].x, startPos.y + neighboursOffsets[i].y);

                if (!IsValid(atualNeighbourPos))
                    continue;

                int atualIndex = ArrayIndex(atualNeighbourPos.x, atualNeighbourPos.y);
                FindNearestPathNode atualNode = pathNodeArray[atualIndex];

                if (atualNode.isWalkable)
                {
                    int2 targetGridPos = new int2(atualNode.x, atualNode.y);
                    resultNodePos[0] = targetGridPos;
                    break;
                }
            }
        }
    }

    public struct FindNearestPathNode
    {
        public int x;
        public int y;

        public bool isWalkable;
    }
}
