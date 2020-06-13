using UnityEngine;

namespace My_Utils.PathFinding
{
    [RequireComponent(typeof(Seeker), typeof(Rigidbody2D))]
    public abstract class FindPathAI : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected Seeker seeker;

        protected bool isEnabled = true;

        [SerializeField] private SetTargetMode setTargetMode;
        [ConditionalShow("setTargetMode", SetTargetMode.Inspector)]
        [SerializeField] private TargetType targetType = TargetType.Object;

        [ConditionalShow("setTargetMode", SetTargetMode.Inspector, "targetType", TargetType.Tag)]
        [SerializeField] private string targetTag = "";
        [ConditionalShow("setTargetMode", SetTargetMode.Inspector, "targetType", TargetType.Tag)]
        [Tooltip("How the target will be defined. First -> The first with that tag, or the nearest with that tag.")]
        [SerializeField] private DefineTargetType _defineTargetType = default;

        [ConditionalShow("setTargetMode", SetTargetMode.Inspector, "targetType", TargetType.Object)]
        [SerializeField] protected Transform target;

        [Space]
        [Tooltip("The hitbox of this object.")]
        public Vector2 boxColliderSize = new Vector2(1f, 1f);

        [Tooltip("When the path will be atualized. Always -> Each pathRate sec; WhenTargetMove -> Each pathRate sec when target moved.")]
        [SerializeField] private AtualizePathMode atualizePathMode = default;
        private Vector3 lastTargetPos; // Store the last target pos, so whe can know when it changes

        [Tooltip("Time between each path atualization.")]
        [SerializeField] private float atualizePathRate = 0.5f;
        private float atualizePathTimer;

        protected int currentPathIndex;


        /// <summary>
        /// Forces a path atualization
        /// </summary>
        public void AtualizePath()
        {
            GetPathToTarget();
        }

        /// <summary>
        /// Set a new target for the path finding
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        /// <summary>
        /// Set a new target for the path finding. A object with this tag.
        /// </summary>
        public void SetTarget(string targetTag)
        {
            SetTarget(targetTag, _defineTargetType);
        }

        /// <summary>
        /// Set a new target for the path finding. A object with this tag.
        /// <param name="targetTag">The target tag.</param>
        /// <param name="defineTargetType">How the target will be defined.</param>
        /// </summary>
        public void SetTarget(string targetTag, DefineTargetType defineTargetType)
        {
            if (defineTargetType == DefineTargetType.First)
            {
                target = GameObject.FindGameObjectWithTag(targetTag).transform;
            }
            else
            {
                GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetTag);
                target = GetNearestTarget(possibleTargets);
            }
        }

        private Transform GetNearestTarget(GameObject[] possibleTargets)
        {
            if (possibleTargets.Length == 0)
                return null;

            float minDistToTarget = 0;
            int nearestTargetIndex = 0;
            for (int i = 0; i < possibleTargets.Length; i++)
            {
                if (i == 0)
                {
                    minDistToTarget = PathLenghtTo(possibleTargets[i].transform.position);
                }
                else
                {
                    float tentativeNewDistToTarget = PathLenghtTo(possibleTargets[i].transform.position);
                    if (tentativeNewDistToTarget < minDistToTarget)
                    {
                        minDistToTarget = tentativeNewDistToTarget;
                        nearestTargetIndex = i;
                    }
                }
            }

            return possibleTargets[nearestTargetIndex].transform;
        }


        /// <summary>
        /// Calculate the path lenght to a target. Used to find the nearest target.
        /// </summary>
        /// <returns></returns>
        protected abstract float PathLenghtTo(Vector2 targetPos);

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            seeker = GetComponent<Seeker>();
        }

        protected virtual void Start()
        {
            if (targetType == TargetType.Tag)
                SetTarget(targetTag);
        }


        /// <summary>
        /// Verify if needs to atualize the path
        /// </summary>
        protected virtual void VerifyToAtualizePath()
        {
            if (atualizePathTimer > 0)
            {
                atualizePathTimer -= Time.fixedDeltaTime;
            }
            else
            {
                if (atualizePathMode == AtualizePathMode.Always)
                {
                    atualizePathTimer = atualizePathRate;
                    GetPathToTarget();
                }
                else if (lastTargetPos != target.position) // AtualizePathMode.WhenTargetMoves
                {
                    lastTargetPos = target.position;
                    atualizePathTimer = atualizePathRate;
                    GetPathToTarget();
                }
            }
        }


        /// <summary>
        /// Function that call 'Seeker.FindPath()' and assign it to a variable. This functions is called by MoveAI base script.
        /// </summary>
        protected abstract void GetPathToTarget();

        protected virtual void FixedUpdate()
        {
            if (isEnabled && target != null)
            {
                VerifyToAtualizePath();
                Move();
            }
        }


        /// <summary>
        /// The function that makes the AI moves.
        /// </summary>
        protected abstract void Move();
    }
}
