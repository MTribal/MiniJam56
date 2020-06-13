using UnityEngine;

namespace My_Utils.PathFinding
{
    public struct FindPathData
    {
        public Vector2 startPos;
        public Vector2 targetPos;
        public FindPathType findPathType;
        public float boxColliderX;
        public VectorPathType vectorPathType;

        /// <summary>
        /// If a gridDataKey will be used to find the path.
        /// </summary>
        public bool useKey;
        public string gridDataKey;
        public SaveKeyType saveKeyType;

        /// <summary>
        /// Carry the param for find a path.
        /// </summary>
        /// <param name="startPos">The start position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <param name="findPathType">The type of path will want to search for. With or without diagonals.</param>
        public FindPathData(Vector2 startPos, Vector2 targetPos, float boxColliderX, FindPathType findPathType, VectorPathType vectorPathType, string gridDataKey, SaveKeyType saveKeyType, bool useKey = false)
        {
            this.startPos = startPos;
            this.targetPos = targetPos;
            this.findPathType = findPathType;
            this.boxColliderX = boxColliderX;
            this.gridDataKey = gridDataKey;
            this.saveKeyType = saveKeyType;
            this.vectorPathType = vectorPathType;
            this.useKey = useKey;
        }
    }
}