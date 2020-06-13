using UnityEngine;

namespace My_Utils.PathFinding
{
    public class PlatformerPath
    {
        public PlatformerEdge<Vector2>[] EdgePath { get; private set; }

        private float _cachedPathLenght = -1;

        public PlatformerPath(PlatformerEdge<Vector2>[] edgePath)
        {
            EdgePath = edgePath;
        }

        public int EdgesCount
        {
            get
            {
                return EdgePath.Length;
            }
        }


        /// <summary>
        /// Return the total lenght of the path. (Cached)
        /// </summary>
        public float PathLenght
        {
            get
            {
                if (_cachedPathLenght == -1)
                {
                    _cachedPathLenght = 0;
                    for (int i = 0; i < EdgePath.Length; i++)
                    {
                        _cachedPathLenght += EdgePath[i].wheight;
                        if  (i > 0)
                        {
                            _cachedPathLenght += (EdgePath[i].fromPosition - EdgePath[i - 1].toPosition).magnitude;
                        }
                    }
                }

                return _cachedPathLenght;
            }
        }
    }
}
