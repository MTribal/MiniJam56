using Unity.Mathematics;

namespace My_Utils.PathFinding
{
    [System.Serializable]
    public struct GridPathNode
    {
        /// <summary>
        /// The grid position in a int[]. int[0] == x && int[1] == y.
        /// </summary>
        public int[] gridPosition;
        public bool isWalkable;

        /// <summary>
        /// Represents wich Platform this is. Used in platform path finding.
        /// </summary>
        public int PlatformId { get; }

        /// <summary>
        /// Represents wich part of the Platform this is. A or B, true of false... Used in platform path finding.
        /// </summary>
        public bool Section { get; } 

        public GridPathNode(int2 position, bool isWalkable, int platformId, bool section = false)
        {
            gridPosition = new int[] { position.x, position.y };
            this.isWalkable = isWalkable;
            PlatformId = platformId;
            Section = section;
        }
    }
}
