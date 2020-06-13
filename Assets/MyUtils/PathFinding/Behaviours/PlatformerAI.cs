using UnityEngine;

namespace My_Utils.PathFinding
{
    [RequireComponent(typeof(Seeker))]
    public abstract class PlatformerAI : FindPathAI
    {
        [Header("Edges")]

        [Tooltip("The atual mode of the AI. Follow - Follow a path to a target; Record - Record inputs and save.")]
        public AIMode mode;

        [Tooltip("Record inputs class attached to this object. Is the class that will be called when learnMode is activated. That class will read and record the inputs.")]
        public ReadAndRecord readRecordInputs;

        [DataKey(DataKeyType.Platformer)]
        public string saveGraphKey;

        [HideInInspector]
        public int choicedIndex;

        protected PlatformerPath platformerPath = null;
        protected Vector2 atualPosTarget;
        protected bool controlledByEdge;
        protected int currentFrameIndex;
        protected PlatformerFrame[] currentFrames;

        protected override void Start()
        {
            base.Start();
            isEnabled = mode != AIMode.Record; // Deactivate ai if is in learn mode

            if (readRecordInputs == null)
            {
                Debug.LogError("ReadRecordInputs class is null");
            }

            readRecordInputs.enabled = mode == AIMode.Record;
        }

        /// <summary>
        /// Receive a recorded edge from a ReadAndRecord script.
        /// </summary>
        /// <param name="recordedEdge">Recorded edge.</param>
        public void ReceiveGraphData(PlatformerEdge<Vector2> recordedEdge, RecordType recordType)
        {
            PlatformerGraph<Vector2> savedGraph = SaveSystem.LoadData<GraphData>(saveGraphKey).Load();
            if (savedGraph.VerticesCount == 0)
                savedGraph = seeker.GetBaseGraph();

            savedGraph.AddEdge(recordedEdge, recordType);
            SaveSystem.SaveDataIn(new GraphData(savedGraph), saveGraphKey);
        }

        protected override float PathLenghtTo(Vector2 targetPos)
        {
            PlatformerPath platformerPath = seeker.FindPlatformPath(saveGraphKey, transform.position, targetPos, boxColliderSize.x, RoundNodeType.SearchDown, SaveKeyType.Persistent);
            float distToTarget = platformerPath != null ? platformerPath.PathLenght : 0f;
            return distToTarget;
        }
    }
}
