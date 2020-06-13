using UnityEngine;
using System.Collections.Generic;
using System;

namespace My_Utils.Lean_Tween.Visual
{
    public class LeanTweenVisual : MonoBehaviour
    {
        #region Fields
        #region Public
        public float simulationTimer;

        public bool resetRepeatAfterSimulation; // if is needed reset repeatCount to -1 after simulation

        /// <summary>
        /// If this object has simulated at least once.
        /// </summary>
        public bool hasSimulated;

        // Values before simulation

        public bool isRect;
        public Vector3 startPos;
        public Vector3 startRotation;
        public Vector3 startScale;
        public Vector2 startSize; // UI

        public bool hasRenderer;
        public GenericRenderer genericRenderer;
        public Color startColor;
        public Sprite startSprite;


        // Structure containing the tween groups and items.
        // Holds information to run sequential and concurrent tweens.
        public List<LeanTweenGroup> groupList = new List<LeanTweenGroup>();

        public float allDelay;

        // Indicates whether the entire tween should continue repeating.
        public bool doesAllLoop;

        // Delay between repeats.
        public float loopAllDelay;

        // How many times the group of tweens repeat
        public int loopAllCount;

        public int repeatIter;

        // If it is repeating, time in game time to call next repeat.
        public float nextCall;

        // Indicates if the whole tween should restart when object is
        // enabled.
        public bool restartOnEnable;
        public bool playOnStart = true;

        public bool generateCode;

        public bool simulate; // Mark to play the tween in edit mode
        public bool stopSimulation;
        public bool isSimulating;
        public bool resetValues;
        public float timeToFinishSimulation = 1f;
        public int defaultLoopCount;

        public bool isTesting;

        public List<LeanTweenItem> itensWithInfiniteLoops = new List<LeanTweenItem>();
        public List<LeanTweenGroup> groupsWithInfiniteLoops = new List<LeanTweenGroup>();

        public int versionNum;

        #endregion

        #endregion

        private void Awake()
        {
            LTVisualShared.UpdateTweens(this);
        }

        // Initialize variables.  This is put in start
        // to be compatible with object recycler.
        private void Start()
        {
            // If Time.realtimeSinceStartup < 1f means is exiting play mode, so don't start 
            if (playOnStart && Time.realtimeSinceStartup > 1f)
            {
                Invoke("Init", allDelay);
            }
        }

        public void StartTweens()
        {
            BuildAllTweens(false, true);

        }

        public void StartTweens(GameObject target = null)
        {
            BuildAllTweens(false, true, target);
        }

        public void Cancel()
        {
            LeanTween.CancelAll();
        }

        public void Play()
        {
            Invoke("Init", allDelay);
        }

        public void Test()
        {
            isTesting = true;
            Simulate();
            Invoke("FinishTest", GetAllGroupsTotalDuration());
        }

        private void FinishTest()
        {
            isTesting = false;
        }

        public void Simulate()
        {
            LTVisualShared.UpdateTweens(this);
            Invoke("Init", allDelay);
        }

        private void Init()
        {
            BuildAllTweens(false, true);
        }

        /// <summary>
        /// Use to get all groups total duration with counting all repeats and all delays.
        /// </summary>
        /// <returns></returns>
        public float GetAllGroupsTotalDuration()
        {
            float totalDuration = GetAllGroupsDuration();
            float loopCount = 1;
            float loopDelay = 0;
            if (doesAllLoop)
            {
                loopDelay = loopAllDelay;
                loopCount = loopAllCount < 1 ? defaultLoopCount : loopAllCount;
            }
            return (totalDuration * loopCount) + (loopDelay * (loopCount - 1)) + allDelay;
        }

        /// <summary>
        /// Use to get all groups end time without counting all repeats
        /// </summary>
        public float GetAllGroupsDuration()
        {
            float groupsTotalDuration = 0;
            for (int i = 0; i < groupList.Count; i++)
            {
                if (i == 0)
                {
                    groupsTotalDuration = GetGroupTotalTime(groupList[i]);
                }
                else
                {
                    float atualGroupDuration = GetGroupTotalTime(groupList[i]);
                    if (atualGroupDuration > groupsTotalDuration)
                    {
                        groupsTotalDuration = atualGroupDuration;
                    }
                }
            }
            return groupsTotalDuration;
        }

        /// <summary>
        /// Use to get a group end time with counting group repeats.
        /// </summary>
        public float GetGroupTotalTime(LeanTweenGroup group)
        {
            int loops = 1;
            if (group.doesLoop)
            {
                loops = group.loopCount < 1 ? defaultLoopCount : group.loopCount;
            }

            return (GetGroupEndTime(group) * loops) + (group.loopDelay * (loops - 1)) + group.delay;
        }

        /// <summary>
        /// Use to get a group end time withot counting group repeats
        /// </summary>
        public float GetGroupEndTime(LeanTweenGroup group)
        {
            float maxTime = 0;
            for (int i = 0; i < group.itemList.Count; i++)
            {
                if (i == 0)
                {
                    maxTime = GetItemTotalTime(group.itemList[i]);
                }
                else
                {
                    float atualTime = GetItemTotalTime(group.itemList[i]);
                    if (atualTime > maxTime)
                    {
                        maxTime = atualTime;
                    }
                }
            }

            return maxTime;
        }

        /// <summary>
        /// Use to get a item end time with counting item repeats
        /// </summary>
        public float GetItemTotalTime(LeanTweenItem item)
        {
            int loopCount = 1;
            float loopDelay = 0;
            if (item.doesLoop)
            {
                loopDelay = item.loopDelay;
                if (item.loopCount < 1)
                {
                    loopCount = defaultLoopCount;
                }
                else
                {
                    loopCount = item.loopCount;
                }
            }

            return (item.duration * loopCount) + (loopDelay * (loopCount - 1)) + item.delay;
        }

        // Called on enable and if you want the tween
        // to restart on enable / on active.
        private void OnEnable()
        {
            if (restartOnEnable)
            {
                BuildAllTweens(false, true);
            }
        }

        // Remove unnecessary tweens from LeanTween.
        private void OnDisable()
        {
            if (restartOnEnable)
            {
                LeanTween.Cancel(gameObject);
            }
        }

        // If object is destroyed, get rid of tweens.
        private void OnDestroy()
        {
            LeanTween.Cancel(gameObject);
        }

        #region Public Methods
        public void CopyTo(LeanTweenVisual tween)
        {
            tween.nextCall = nextCall;
            tween.doesAllLoop = doesAllLoop;
            tween.loopAllDelay = loopAllDelay;
            tween.restartOnEnable = restartOnEnable;

            tween.groupList = new List<LeanTweenGroup>();
            foreach (LeanTweenGroup group in groupList)
            {
                tween.groupList.Add(new LeanTweenGroup(group));
            }
        }
        #endregion

        #region Private Methods

        private System.Text.StringBuilder codeBuild;
        private string tabs;
        private LTDescr tween;
        private readonly float allTweensDelaySaved;

        public void BuildAllTweensAgain()
        {
            LeanTween.DelayedCall(gameObject, loopAllDelay, BuildAllTweensAgainNow);
        }

        public void BuildAllTweensAgainNow()
        {
            BuildAllTweens(false, false);
        }

        public LTDescr Append(string method, float to, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(gameObject, " + to + "f, " + duration + "f)");
            return null;
        }

        public LTDescr Append(string method, float to, float duration, bool useRecursive)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(gameObject, " + to + "f, " + duration + "f" + useRecursive + ")");
            return null;
        }

        public LTDescr AppendRect(string method, float to, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(GetComponent<RectTransform>(), " + to + "f, " + duration + "f)");
            return null;
        }

        public LTDescr AppendRect(string method, float to, float duration, bool useRecursion)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(GetComponent<RectTransform>(), " + to + "f, " + duration + "f, " + useRecursion + ")");
            return null;
        }

        public LTDescr Append(string method, Vector3 to, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(gameObject, " + vecToStr(to) + ", " + duration + "f)");
            return null;
        }

        public LTDescr AppendRect(string method, Vector3 to, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(GetComponent<RectTransform>(), " + vecToStr(to) + ", " + duration + "f)");
            return null;
        }

        public LTDescr appendRect(string method, Vector2 to, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(GetComponent<RectTransform>(), " + vecToStr(to) + ", " + duration + "f)");
            return null;
        }

        public LTDescr AppendRect(string method, Color color, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(GetComponent<RectTransform>(), " + ColorToStr(color) + ", " + duration + "f)");
            return null;
        }

        public LTDescr AppendRect(string method, Color color, float duration, bool useRecursion)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(GetComponent<RectTransform>(), " + ColorToStr(color) + "f, " + duration + "f, " + useRecursion + ")");
            return null;
        }

        public LTDescr Append(string method, Color color, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(gameObject, " + ColorToStr(color) + ", " + duration + "f)");
            return null;
        }

        public LTDescr Append(string method, Color color, float duration, bool useRecursion)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(gameObject, " + ColorToStr(color) + "f, " + duration + "f, " + useRecursion + ")");
            return null;
        }

        public LTDescr Append(string method, string methodName, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(gameObject, " + methodName + ", " + duration + "f)");
            return null;
        }

        public LTDescr append(string method, Vector3[] to, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(gameObject, new Vector3[]{");

            if (to == null)
            {
                codeBuild.Append("null");
            }
            else
            {
                for (int i = 0; i < to.Length; i++)
                {
                    codeBuild.Append(vecToStr(to[i]));
                    if (i < to.Length - 1)
                        codeBuild.Append(", ");
                }
            }
            codeBuild.Append("} , " + duration + "f)");
            return null;
        }

        public LTDescr AppendRect(string method, Vector3[] to, float duration)
        {
            codeBuild.Append(tabs + "LeanTween." + method + "(GetComponent<RectTransform>(), new Vector3[]{");

            if (to == null)
            {
                codeBuild.Append("null");
            }
            else
            {
                for (int i = 0; i < to.Length; i++)
                {
                    codeBuild.Append(vecToStr(to[i]));
                    if (i < to.Length - 1)
                        codeBuild.Append(", ");
                }
            }
            codeBuild.Append("} , " + duration + "f)");
            return null;
        }

        public void Append(AnimationCurve curve)
        {
            codeBuild.Append("new AnimationCurve(");
            for (int i = 0; i < curve.length; i++)
            {
                codeBuild.Append("new Keyframe(" + curve[i].time + "f," + curve[i].value + "f)");
                if (i < curve.length - 1)
                    codeBuild.Append(", ");
            }
            codeBuild.Append(")");
        }

        private string vecToStr(Vector3 vec3)
        {
            return "new Vector3(" + vec3.x + "f," + vec3.y + "f," + vec3.z + "f)";
        }

        private string vecToStr(Vector2 vec2)
        {
            return "new Vector2(" + vec2.x + "f," + vec2.y + "f)";
        }

        private string ColorToStr(Color color)
        {
            return "new Color(" + color.r + "f," + color.g + "f," + color.b + "f," + color.a + "f)";
        }

        private void BuildTween(LeanTweenItem item, float delayAdd, bool generateCode, bool resetPath)
        {
            item.recursive = false; // Don't use recursive
            float delay = item.delay + delayAdd;
            bool code = generateCode;
            float d = item.duration;

            if (isSimulating)
            {
                code = false;
                if (item.loopCount < 1)
                {
                    item.loopCount = defaultLoopCount;
                    itensWithInfiniteLoops.Add(item);
                }
            }

            if (item.actionStr == null)
            {
                item.actionStr = "Move";
            }
            item.action = (TweenAction)Enum.Parse(typeof(TweenAction), item.actionStr, true);

            // Debug.Log("item.gameObject:"+item.gameObject+" name:"+item.gameObject.transform.name);
            if (item.gameObject == null)
            {
                item.gameObject = gameObject;
            }

            switch (item.action)
            {
                case TweenAction.Move:
                    tween = code ? Append("Move", item.to, d) : LeanTween.move(item.gameObject, item.to, d);
                    break;

                case TweenAction.MoveX:
                    tween = code ? Append("MoveX", item.to.x, d) : LeanTween.MoveX(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.MoveY:
                    tween = code ? Append("MoveY", item.to.x, d) : LeanTween.moveY(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.MoveZ:
                    tween = code ? Append("MoveZ", item.to.x, d) : LeanTween.moveZ(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.MoveLocal:
                    tween = code ? Append("MoveLocal", item.to, d) : LeanTween.moveLocal(item.gameObject, item.to, d);
                    break;

                case TweenAction.MoveLocalX:
                    tween = code ? Append("MoveLocalX", item.to.x, d) : LeanTween.MoveLocalX(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.MoveLocalY:
                    tween = code ? Append("MoveLocalY", item.to.x, d) : LeanTween.MoveLocalY(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.MoveLocalZ:
                    tween = code ? Append("MoveLocalZ", item.to.x, d) : LeanTween.MoveLocalZ(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.MoveAdd:
                    tween = code ? Append("MoveAdd", item.to, d) : LeanTween.MoveAdd(item.gameObject, item.to, d);
                    break;

                case TweenAction.MoveBezier:
                    tween = code ? append("Move", item.bezierPath ? item.bezierPath.vec3 : null, d) : LeanTween.Move(item.gameObject, item.bezierPath, d, resetPath);
                    if (item.orientToPath)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath(" + item.orientToPath.ToString().ToLower() + ")");
                        }
                        else
                        {
                            tween.SetOrientToPath(item.orientToPath);
                        }
                    }
                    if (item.isPath2d)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath2d(true)");
                        }
                        else
                        {
                            tween.setOrientToPath2d(item.isPath2d);
                        }
                    }
                    break;

                case TweenAction.MoveBezierLocal:
                    tween = code ? append("MoveLocal", item.bezierPath ? item.bezierPath.vec3 : null, d) : LeanTween.MoveLocal(item.gameObject, item.bezierPath, d, resetPath, item.alignToPath);
                    if (item.orientToPath)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath(" + item.orientToPath + ")");
                        }
                        else
                        {
                            tween.SetOrientToPath(item.orientToPath);
                        }
                    }
                    if (item.isPath2d)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath2d(true)");
                        }
                        else
                        {
                            tween.setOrientToPath2d(item.isPath2d);
                        }
                    }
                    break;

                case TweenAction.MoveSpline:
                    tween = code ? append("MoveSpline", item.splinePath ? item.splinePath.splineVector() : null, d) : LeanTween.MoveSpline(item.gameObject, item.splinePath, d, resetPath);
                    if (item.orientToPath)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath(" + item.orientToPath + ")");
                        }
                        else
                        {
                            tween.SetOrientToPath(item.orientToPath);
                        }
                    }
                    if (item.isPath2d)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath2d(true)");
                        }
                        else
                        {
                            tween.setOrientToPath2d(item.isPath2d);
                        }
                    }
                    break;

                case TweenAction.MoveSplineLocal:
                    tween = code ? append("MoveSplineLocal", item.splinePath ? item.splinePath.splineVector() : null, d) : LeanTween.MoveSplineLocal(item.gameObject, item.splinePath, d, resetPath, item.alignToPath);
                    if (item.orientToPath)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath(" + item.orientToPath + ")");
                        }
                        else
                        {
                            tween.SetOrientToPath(item.orientToPath);
                        }
                    }
                    if (item.isPath2d)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath2d(true)");
                        }
                        else
                        {
                            tween.setOrientToPath2d(item.isPath2d);
                        }
                    }
                    break;

                case TweenAction.CanvasMove:
                    tween = code ? AppendRect("Move", item.to, d) : LeanTween.Move(item.gameObject.GetComponent<RectTransform>(), item.to, d);
                    break;

                case TweenAction.CanvasMoveX:
                    tween = code ? AppendRect("MoveX", item.to.x, d) : LeanTween.MoveX(item.gameObject.GetComponent<RectTransform>(), item.to.x, d);
                    break;

                case TweenAction.CanvasMoveY:
                    tween = code ? AppendRect("MoveY", item.to.x, d) : LeanTween.MoveY(item.gameObject.GetComponent<RectTransform>(), item.to.x, d);
                    break;

                case TweenAction.CanvasMoveZ:
                    tween = code ? AppendRect("MoveZ", item.to.x, d) : LeanTween.MoveZ(item.gameObject.GetComponent<RectTransform>(), item.to.x, d);
                    break;

                case TweenAction.CanvasMoveAdd:
                    tween = code ? AppendRect("MoveAdd", item.to, d) : LeanTween.MoveAdd(item.gameObject.GetComponent<RectTransform>(), item.to, d);
                    break;

                case TweenAction.CanvasMoveBezier:
                    tween = code ? AppendRect("Move", item.bezierPath ? item.bezierPath.vec3 : null, d) : LeanTween.Move(item.gameObject.GetComponent<RectTransform>(), item.bezierPath, d, resetPath);
                    if (item.orientToPath)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath(" + item.orientToPath.ToString().ToLower() + ")");
                        }
                        else
                        {
                            tween.SetOrientToPath(item.orientToPath);
                        }
                    }
                    if (item.isPath2d)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath2d(true)");
                        }
                        else
                        {
                            tween.setOrientToPath2d(item.isPath2d);
                        }
                    }
                    break;

                case TweenAction.CanvasMoveBezierLocal:
                    tween = code ? AppendRect("MoveLocal", item.bezierPath ? item.bezierPath.vec3 : null, d) : LeanTween.MoveLocal(item.gameObject.GetComponent<RectTransform>(), item.bezierPath, d, resetPath, item.alignToPath);
                    if (item.orientToPath)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath(" + item.orientToPath + ")");
                        }
                        else
                        {
                            tween.SetOrientToPath(item.orientToPath);
                        }
                    }
                    if (item.isPath2d)
                    {
                        if (code)
                        {
                            codeBuild.Append(".SetOrientToPath2d(true)");
                        }
                        else
                        {
                            tween.setOrientToPath2d(item.isPath2d);
                        }
                    }
                    break;

                case TweenAction.Rotate:
                    tween = code ? Append("Rotate", item.to, d) : LeanTween.Rotate(item.gameObject, item.to, d);
                    break;

                case TweenAction.RotateLocal:
                    tween = code ? Append("RotateLocal", item.to, d) : LeanTween.RotateLocal(item.gameObject, item.to, d);
                    break;

                case TweenAction.RotateX:
                    tween = code ? Append("RotateX", item.to.x, d) : LeanTween.RotateX(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.RotateY:
                    tween = code ? Append("RotateY", item.to.x, d) : LeanTween.RotateY(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.RotateZ:
                    tween = code ? Append("RotateZ", item.to.x, d) : LeanTween.RotateZ(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.RotateAdd:
                    tween = code ? Append("RotateAdd", item.to, d) : LeanTween.RotateAdd(item.gameObject, item.to, d);
                    break;

                case TweenAction.CanvasRotate:
                    tween = code ? AppendRect("Rotate", item.to, d) : LeanTween.Rotate(item.gameObject.GetComponent<RectTransform>(), item.to, d);
                    break;

                case TweenAction.CanvasRotateLocal:
                    tween = code ? AppendRect("RotateLocal", item.to, d) : LeanTween.RotateLocal(item.gameObject.GetComponent<RectTransform>(), item.to, d);
                    break;

                case TweenAction.CanvasRotateAdd:
                    tween = code ? AppendRect("RotateAdd", item.to, d) : LeanTween.RotateAdd(item.gameObject.GetComponent<RectTransform>(), item.to, d);
                    break;

                case TweenAction.Scale:
                    tween = code ? Append("Scale", item.to, d) : LeanTween.Scale(item.gameObject, item.to, d);
                    break;

                case TweenAction.ScaleX:
                    tween = code ? Append("ScaleX", item.to.x, d) : LeanTween.ScaleX(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.ScaleY:
                    tween = code ? Append("ScaleY", item.to.x, d) : LeanTween.ScaleY(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.ScaleZ:
                    tween = code ? Append("ScaleZ", item.to.x, d) : LeanTween.ScaleZ(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.ScaleAdd:
                    tween = code ? Append("ScaleAdd", item.to, d) : LeanTween.ScaleAdd(item.gameObject, item.to, d);
                    break;

                case TweenAction.CanvasScale:
                    tween = code ? AppendRect("Scale", item.to, d) : LeanTween.Scale(item.gameObject.GetComponent<RectTransform>(), item.to, d);
                    break;

                case TweenAction.CanvasScaleAdd:
                    tween = code ? AppendRect("ScaleAdd", item.to, d) : LeanTween.ScaleAdd(item.gameObject.GetComponent<RectTransform>(), item.to, d);
                    break;

                case TweenAction.CanvasSize:
                    Vector2 to = new Vector2(item.to.x, item.to.y);
                    tween = code ? appendRect("Size", to, d) : LeanTween.Size(item.gameObject.GetComponent<RectTransform>(), to, d);
                    break;

                case TweenAction.CanvasSizeAdd:
                    to = new Vector2(item.to.x, item.to.y);
                    tween = code ? appendRect("SizeAdd", to, d) : LeanTween.SizeAdd(item.gameObject.GetComponent<RectTransform>(), to, d);
                    break;

                case TweenAction.Color:
                    tween = code ? Append("Color", item.colorTo, d, item.recursive) : LeanTween.Color(item.gameObject, item.colorTo, d, item.recursive);
                    break;

                case TweenAction.ColorGroup:
                    tween = code ? Append("ColorGroup", item.colorTo, d) : LeanTween.ColorGroup(item.gameObject, item.colorTo, d);
                    break;

                case TweenAction.Alpha:
                    tween = code ? Append("Alpha", item.to.x, d, item.recursive) : LeanTween.Alpha(item.gameObject, item.to.x, d, item.recursive);
                    break;

                case TweenAction.AlphaGroup:
                    tween = code ? Append("AlphaGroup", item.to.x, d) : LeanTween.AlphaGroup(item.gameObject, item.to.x, d);
                    break;

                case TweenAction.AlphaVertex:
                    tween = code ? Append("AlphaVertex", item.to.x, d, item.recursive) : LeanTween.AlphaVertex(item.gameObject, item.to.x, d, item.recursive);
                    break;

                case TweenAction.CanvasPlaySprite:
                    if (generateCode)
                    {
                        codeBuild.Append(tabs + "LeanTween.Play(GetComponent<RectTransform>(), new UnityEngine.Sprite[" + item.sprites.Length + "]).setFrameRate(" + item.frameRate + "f)");
                    }
                    else if (item.sprites != null)
                    {
                        if (item.sprites.Length > 0)
                        {
                            tween = LeanTween.Play(item.gameObject.GetComponent<RectTransform>(), item.sprites).setFrameRate(item.frameRate);
                        }
                        else
                        {
                            Debug.LogError("Sprite list lenght == 0.");
                            return;
                        }
                    }
                    else
                    {
                        Debug.LogError("Sprite list == null.");
                        return;
                    }
                    break;

                case TweenAction.CanvasAlpha:
                    tween = code ? AppendRect("Alpha", item.to.x, d, item.recursive) : LeanTween.Alpha(item.gameObject.GetComponent<RectTransform>(), item.to.x, d, item.recursive);
                    break;

                case TweenAction.CanvasTextAlpha:
                    tween = code ? AppendRect("TextAlpha", item.to.x, d, item.recursive) : LeanTween.TextAlpha(item.gameObject.GetComponent<RectTransform>(), item.to.x, d, item.recursive);
                    break;

                case TweenAction.CanvasGroupAlpha:
                    tween = code ? AppendRect("CanvasGroup", item.to.x, d) : LeanTween.CanvasGroupAlpha(item.gameObject.GetComponent<RectTransform>(), item.to.x, d);
                    break;

                case TweenAction.CanvasColor:
                    tween = code ? AppendRect("Color", item.colorTo, d, item.recursive) : LeanTween.Color(item.gameObject.GetComponent<RectTransform>(), item.colorTo, d, item.recursive);
                    break;

                case TweenAction.CanvasTextColor:
                    tween = code ? AppendRect("TextColor", item.colorTo, d, item.recursive) : LeanTween.TextColor(item.gameObject.GetComponent<RectTransform>(), item.colorTo, d, item.recursive);
                    break;

                case TweenAction.DelayedSound:
                    if (generateCode)
                    {
                        codeBuild.Append(tabs + "LeanTween.DelayedSound(gameObject, passAudioClipHere, " + vecToStr(item.from) + ", " + d + "f)");
                    }
                    else
                    {
                        tween = LeanTween.delayedSound(gameObject, item.audioClip, item.from, item.duration);
                    }
                    break;

                default:
                    tween = null;
                    Debug.Log("The tween '" + item.action.ToString() + "' has not been implemented. info item:" + item);
                    return;
            }

            if (item.onCompleteItem.GetPersistentEventCount() > 0)
            {
                LeanTween.DelayedCall(gameObject, GetItemTotalTime(item), item.CallOnCompleteItem);
            }
            if (item.onCompleteLoop.GetPersistentEventCount() > 0)
            {
                float timeToWait = item.duration + item.delay;
                for (int i = 0; i < item.loopCount; i++)
                {
                    LeanTween.DelayedCall(gameObject, timeToWait, item.CallOnCompleteLoop);

                    timeToWait += item.loopDelay + item.duration;
                }
            }

            // Append Extras
            if (generateCode)
            {
                if (delay > 0f)
                    codeBuild.Append(".SetDelay(" + delay + "f)");
            }
            else
            {
                tween = tween.SetDelay(delay);
            }
            if (item.ease == LeanTweenType.AnimationCurve)
            {
                if (generateCode)
                {
                    codeBuild.Append(".SetEase(");
                    Append(item.animationCurve);
                    codeBuild.Append(")");
                }
                else
                {
                    tween.SetEase(item.animationCurve);
                }
            }
            else
            {
                if (generateCode)
                {
                    if (item.ease != LeanTweenType.Linear)
                        codeBuild.Append(".SetEase(LeanTweenType." + item.ease + ")");
                }
                else
                {
                    tween.SetEase(item.ease);
                }
            }
            // Debug.Log("curve:"+item.animationCurve+" item.ease:"+item.ease);
            if (item.between == LeanTweenBetween.FromTo)
            {
                if (generateCode)
                    codeBuild.Append(".SetFrom(" + vecToStr(item.from) + ")");
                else
                    tween.SetFrom(item.from);
            }
            if (item.doesLoop)
            {
                if (generateCode)
                {
                    codeBuild.Append(".SetRepeat(" + item.loopCount + ")");
                    codeBuild.Append(".SetLoopDelay(" + item.loopDelay + "f)");
                }
                else
                {
                    tween.SetLoopDelay(item.loopDelay);
                    tween.SetRepeat(item.loopCount);
                }

                if (item.loopType == LoopType.PingPong)
                {
                    if (generateCode)
                        codeBuild.Append(".SetLoopPingPong()");
                    else
                        tween.SetLoopPingPong();
                }
                else if (item.loopType == LoopType.Add)
                {
                    if (generateCode)
                        codeBuild.Append(".SetLoopAdd()");
                    else
                        tween.SetLoopAdd();
                }
            }
            if (generateCode)
            {
                if (item.useSpeed)
                    codeBuild.Append(".SetSpeed(" + item.speed + "f)");
            }
            else
            {
                if (item.useSpeed)
                    tween.SetSpeed(item.speed);
            }
            if (generateCode)
                codeBuild.Append(";\n");
        }

        private bool resetPath;

        public void BuildGroup(object g)
        {
            LeanTweenGroup group = (LeanTweenGroup)g;

            if (isSimulating)
            {
                if (group.loopCount < 1)
                {
                    group.loopCount = defaultLoopCount;
                    groupsWithInfiniteLoops.Add(group);
                }
            }

            if (group.gameObject == null)
                group.gameObject = gameObject;

            if (group.overrideGameObject != null)
                group.gameObject = group.overrideGameObject;

            foreach (LeanTweenItem item in group.itemList)
            {
                if (group.overrideGameObject != null)
                    item.gameObject = group.overrideGameObject;

                if (isSimulating)
                {
                    BuildTween(item, group.delay, false, resetPath);
                }
                else
                {
                    BuildTween(item, group.delay, group.GenerateCode, resetPath);
                }
            }

            if ((!group.GenerateCode || isSimulating) && group.doesLoop && (group.loopCount < 0 || group.loopIter < group.loopCount - 1))
            {
                LeanTween.DelayedCall(group.gameObject, GetGroupEndTime(group), BuildGroup).SetOnCompleteParam(group).SetDelay(group.loopDelay);
                group.loopIter++;
            }
        }

        // Builds the tween structure with all the appropriate values.
        // Cancels all previous tweens to keep a clean tween list.
        // The overallDelay variable is used to set a delay
        // to the entire group.
        // <param name="overallDelay">Overall delay.</param>
        public string BuildAllTweens(bool generateCode, bool resetPath, GameObject overrideGameObject = null)
        {
            if (isSimulating)
                generateCode = false;

            if (generateCode)
            {
                codeBuild = new System.Text.StringBuilder(1024);
                tabs = "";
                if (doesAllLoop)
                    tabs += "\t";
            }
            else
            {
                LeanTween.Cancel(gameObject);
            }

            string preTabs = tabs;
            float lastTweenTime = GetAllGroupsDuration();
            int i = 0;
            foreach (LeanTweenGroup group in groupList)
            {
                bool wrapCode = (group.doesLoop && group.itemList.Count > 0) || group.delay > 0f;
                if (generateCode)
                {
                    if (i != 0)
                        codeBuild.Append("\n");
                    codeBuild.Append(tabs + "// " + group.groupName + " Group\n");
                }
                if (generateCode && wrapCode)
                {
                    codeBuild.Append(tabs + "LeanTween.DelayedCall(gameObject, " + GetGroupEndTime(group) + "f, ()=>{\n");
                    tabs += "\t";
                }

                if (generateCode)
                    group.SetGenerateCode();
                group.loopIter = 0;
                group.overrideGameObject = overrideGameObject;
                this.resetPath = resetPath;
                BuildGroup(group);

                tabs = preTabs;
                if (generateCode && wrapCode)
                {
                    if (group.delay > 0f)
                    {
                        codeBuild.Append(".SetDelay(" + group.delay + "f)");
                    }
                    codeBuild.Append(tabs + "}).SetOnCompleteOnStart(true)");
                    if (group.doesLoop)
                        codeBuild.Append(".SetRepeat(" + (group.loopCount < 0 ? -1 : group.loopCount - 1) + ")");
                    codeBuild.Append(";\n");
                }

                i++;
            }

            if (doesAllLoop)
            {
                if (generateCode)
                {
                    codeBuild.Insert(0, "LeanTween.DelayedCall(gameObject, " + lastTweenTime + "f, ()=>{\n");
                    codeBuild.Append("}).SetRepeat(" + repeatIter + ").SetOnCompleteOnStart(true);\n");
                }
                else if (repeatIter < loopAllCount - 1 || loopAllCount < 0)
                {
                    LeanTween.DelayedCall(gameObject, lastTweenTime, BuildAllTweensAgain);
                    repeatIter++;
                }
                else
                {
                    repeatIter = 0;
                    // reset optional
                }
            }
            if (generateCode)
            {
                return codeBuild.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}