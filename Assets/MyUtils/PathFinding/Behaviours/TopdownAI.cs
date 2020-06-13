using UnityEngine;

namespace My_Utils.PathFinding
{
    public class TopdownAI : FindPathAI
    {
        [Tooltip("The speed that gameObject will move.")]
        public float moveSpeed = 2f;

        [Tooltip("The minimum distance that the gameObject need to be from the current way point to change to the next way point.")]
        [SerializeField] private float _nextPointDist = 0.05f;

        [Tooltip("Type of the path that you want. Normal -> Normal A*; NonDiagonal -> Not allow diagonal movement.")]
        public FindPathType findPathType;

        private TopdownPath _path = new TopdownPath(new Vector2[0]);

        protected override float PathLenghtTo(Vector2 targetPos)
        {
            return seeker.FindTopdownPathNow(transform.position, targetPos, boxColliderSize.x, findPathType, VectorPathType.OptimizeSpace).PathLenght;
        }

        protected override void GetPathToTarget()
        {
            seeker.FindTopdownPath(transform.position, target.position, boxColliderSize.x, findPathType, VectorPathType.OptimizeSpace, (TopdownPath topdownPath) => 
            {
                _path = topdownPath;
                currentPathIndex = 0;
            });
        }

        protected override void Move()
        {
            if (_path.VectorPath.Length > 0 && currentPathIndex < _path.VectorPath.Length)
            { // Path exist
                Vector2 nextPointDirection = GetNextPathPointDirection();

                Vector2 atualDir = GetAtualDirection(Axis.XandY); // Atual direction that gameObject will move
                Vector2 correctDir = AtualizePathIndex(atualDir, nextPointDirection);

                rb.velocity = correctDir.normalized * moveSpeed * Time.fixedDeltaTime * 100;

                // Check if reached wanted way point
                if (_path.VectorPath[currentPathIndex].IsCloseTo(transform.position, _nextPointDist))
                {
                    currentPathIndex++;
                    if (currentPathIndex >= _path.VectorPath.Length)
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
            }
        }


        /// <summary>
        /// Atualize currentPathIndex if necessary and return the new direction. You still need increase the index if the transform reached a point.
        /// </summary>
        /// <param name="atualDirection">The atual direction that the transform is pointing to.</param>
        /// <param name="nextPointDirection">The atual direction of the path based on the currentPathIndex.</param>
        /// <returns></returns>
        private Vector2 AtualizePathIndex(Vector2 atualDirection, Vector2 nextPointDirection)
        {

            if (currentPathIndex + 1 < _path.VectorPath.Length
                && atualDirection != nextPointDirection
                && _path.VectorPath[currentPathIndex].IsCloseTo(transform.position, 1f))
            {
                currentPathIndex++;
                atualDirection = GetAtualDirection(Axis.XandY);
            }

            return atualDirection;
        }

        private Vector2 GetAtualDirection(Axis axis)
        {
            //Vector2 gridClampedPos = seeker.GetWorldPosition(seeker.GetGridPosition(transform.position), CellWorldPositionType.Center);

            Vector2 normalDirection = (_path.VectorPath[currentPathIndex] - (Vector2)transform.position).AsDirection();

            Vector2 adjustedDirection = normalDirection;
            if (axis == Axis.X)
            {
                adjustedDirection = new Vector2(normalDirection.x, 0);
            }
            else if (axis == Axis.Y)
            {
                adjustedDirection = new Vector2(0, normalDirection.y);
            }

            return adjustedDirection;
        }

        private Vector2 GetNextPathPointDirection()
        {
            if (currentPathIndex - 1 >= 0)
                return (_path.VectorPath[currentPathIndex] - _path.VectorPath[currentPathIndex - 1]).AsDirection();
            else
                return (_path.VectorPath[currentPathIndex + 1] - _path.VectorPath[currentPathIndex]).AsDirection();
        }
    }
}
