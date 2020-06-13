using UnityEngine;

namespace My_Utils.PathFinding
{
    [RequireComponent(typeof(Seeker))]
    public class JustTesting : MonoBehaviour
    {
        public Transform from, to;

        public PathFindingType testWhat;

        [ConditionalShow("testWhat", PathFindingType.Platformer)]
        [DataKey(DataKeyType.Platformer)]
        public string platformerKey;
        [ConditionalShow("testWhat", PathFindingType.Platformer)]
        public float checkGroundX;

        [ConditionalShow("testWhat", PathFindingType.Topdown)]
        [DataKey(DataKeyType.Topdown)]
        public string topdownKey;

        private Seeker seeker;
        public int testSize;
        public bool logs;

        private void Start()
        {
            seeker = GetComponent<Seeker>();
        }

        private void Update()
        {
            float timeBefore = Time.realtimeSinceStartup;
            if (testWhat == PathFindingType.Platformer)
            {
                for (int i = 0; i < testSize; i++)
                {
                    seeker.FindPlatformPath(platformerKey, from.position, to.position, checkGroundX, RoundNodeType.SearchDown, SaveKeyType.Persistent);
                }
            }
            else
            {
                for (int i = 0; i < testSize; i++)
                {
                    seeker.FindTopdownPath(from.position, to.position, checkGroundX, FindPathType.Normal, VectorPathType.OptimizeSpace, (TopdownPath topdownPath) => { });
                }
            }
            float wastedTime = Time.realtimeSinceStartup - timeBefore;

            if (logs)
                Debug.Log($"Executed in {wastedTime * 1000f} ms");
        }
    }
}
