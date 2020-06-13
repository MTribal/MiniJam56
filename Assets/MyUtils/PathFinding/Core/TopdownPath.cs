using System.Text;
using UnityEngine;

namespace My_Utils.PathFinding
{
    public class TopdownPath
    {
        public Vector2[] VectorPath { get; private set; }

        private float pathDistance;

        public TopdownPath(Vector2[] vectorPath)
        {
            VectorPath = vectorPath;
            pathDistance = 0;
        }

        /// <summary>
        /// Returns the total distance of the path. (Cached)
        /// </summary>
        public float PathLenght
        {
            get
            {
                if (pathDistance == 0)
                {
                    float distance = 0;
                    for (int i = 0; i < VectorPath.Length - 1; i++)
                    {
                        distance += Mathf.Abs((VectorPath[i] - VectorPath[i + 1]).magnitude);
                    }
                    pathDistance = distance;
                }

                return pathDistance;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < VectorPath.Length; i++)
            {
                sb.Append(VectorPath[i]);
                if (i < VectorPath.Length - 1)
                {
                    sb.Append(" -> ");
                }
            }

            return sb.ToString();
        }
    }
}
