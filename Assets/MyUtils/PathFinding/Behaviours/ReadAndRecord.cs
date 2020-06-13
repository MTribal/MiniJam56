using UnityEngine;

namespace My_Utils.PathFinding
{
    [RequireComponent(typeof(Seeker))]
    /// <summary>
    /// Class that will be called when learnMode is activated
    /// </summary>
    public abstract class ReadAndRecord : MonoBehaviour
    {
        [Tooltip("The platfomerAI script that this class is related.")]
        public PlatformerAI platformerAI;

        protected Seeker seeker;
        
        public RecordType recordType;

        public abstract void SendGraphData(PlatformerAI enemyPlatformerAI, PlatformerEdge<Vector2> recordedEdge, RecordType recordType);
    }
}
