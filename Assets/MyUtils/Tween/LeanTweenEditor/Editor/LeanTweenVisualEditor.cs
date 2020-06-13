using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace My_Utils.Lean_Tween.Visual
{
    [CustomEditor(typeof(LeanTweenVisual)), CanEditMultipleObjects]
    public class LeanTweenVisualEditor : Editor
    {
        public static LeanTweenVisual copyObj = null;
        private Vector2 scrollCodeViewPos;

        private Vector3 notAlignedPosition;
        private Vector3 notAlignedRotation;
        private bool wasAligned;

        private LeanTweenVisual[] tweens;
        private int qttOfAtualSimulations;

        public void OnEnable()
        {
            LTVisualShared.UpdateTweens(target as LeanTweenVisual);

            UnityEngine.Object[] objects = targets;
            tweens = new LeanTweenVisual[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                tweens[i] = (LeanTweenVisual)objects[i];
            }
        }

        private void SimulationTimer(LeanTweenVisual[] leanTweenVisuals)
        {
            foreach (LeanTweenVisual tweenVisual in leanTweenVisuals)
            {
                if (tweenVisual.isSimulating)
                {
                    if (Time.realtimeSinceStartup > tweenVisual.simulationTimer)
                    {
                        StopSimulation(tweenVisual);
                        qttOfAtualSimulations--;
                    }
                }
            }

            if (qttOfAtualSimulations == 0)
                EditorApplication.update -= () => SimulationTimer(tweens);
        }

        private void StopSimulation(LeanTweenVisual tweenVisual)
        {
            // Simulation has finished.
            tweenVisual.isSimulating = false;
            ResetValues(tweenVisual);
            tweenVisual.stopSimulation = false;
            LeanTween.canUseInEditMode = false;
        }

        private void GetStartValues(LeanTweenVisual tweenVisual)
        {
            Transform trans = tweenVisual.transform;
            RectTransform rect = tweenVisual.GetComponent<RectTransform>();

            if (rect != null)
            {
                tweenVisual.isRect = true;
                tweenVisual.startPos = rect.anchoredPosition3D;
                tweenVisual.startRotation = rect.localEulerAngles;
                tweenVisual.startScale = rect.localScale;
                tweenVisual.startSize = rect.sizeDelta;
            }
            else if (trans != null)
            {
                tweenVisual.isRect = false;
                tweenVisual.startPos = trans.position;
                tweenVisual.startRotation = trans.localEulerAngles;
                tweenVisual.startScale = trans.localScale;
            }

            #region GetRenderer

            tweenVisual.genericRenderer = trans.GetAllRenderers(-1)[0];

            if (!tweenVisual.genericRenderer.IsAllNull())
            {
                tweenVisual.hasRenderer = true;
                tweenVisual.startColor = tweenVisual.genericRenderer.Color;
                tweenVisual.startSprite = tweenVisual.genericRenderer.Sprite;
            }
            else
            {
                tweenVisual.hasRenderer = false;
            }
            #endregion

            tweenVisual.hasSimulated = true;
        }

        private void ResetValues(LeanTweenVisual tweenVisual)
        {
            if (tweenVisual.isRect)
            {
                RectTransform rect = tweenVisual.GetComponent<RectTransform>();

                rect.anchoredPosition3D = tweenVisual.startPos;
                rect.localEulerAngles = tweenVisual.startRotation;
                rect.localScale = tweenVisual.startScale;
                rect.sizeDelta = tweenVisual.startSize;
            }
            else
            {
                Transform trans = tweenVisual.transform;

                trans.position = tweenVisual.startPos;
                trans.localEulerAngles = tweenVisual.startRotation;
                trans.localScale = tweenVisual.startScale;
            }

            if (tweenVisual.hasRenderer)
            {
                tweenVisual.genericRenderer.Color = tweenVisual.startColor;
                tweenVisual.genericRenderer.Sprite = tweenVisual.startSprite;
            }

            #region Reset loop counts

            if (tweenVisual.resetRepeatAfterSimulation)
            {
                tweenVisual.resetRepeatAfterSimulation = false;
                tweenVisual.loopAllCount = -1;
            }

            foreach (LeanTweenGroup group in tweenVisual.groupsWithInfiniteLoops)
            {
                group.loopCount = -1;
            }
            tweenVisual.groupsWithInfiniteLoops = new List<LeanTweenGroup>();

            foreach (LeanTweenItem item in tweenVisual.itensWithInfiniteLoops)
            {
                item.loopCount = -1;
            }
            tweenVisual.itensWithInfiniteLoops = new List<LeanTweenItem>();

            #endregion
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            LeanTweenVisual mainTarget = target as LeanTweenVisual;
            EditorGUI.BeginChangeCheck();
            float overallDelay = 0;
            bool clicked, deleted;
            Vector3 vec;
            clicked = false;

            bool playOnStartBefore = mainTarget.playOnStart;
            EditorGUILayout.BeginHorizontal();
            bool playOnStartNow = EditorGUILayout.Toggle(new GUIContent("Play on Start", "Tweens won't start automatically, you can start them via code with .start()"), mainTarget.playOnStart);


            EditorGUILayout.EndHorizontal();

            if (playOnStartBefore != playOnStartNow)
            {
                foreach (LeanTweenVisual tweenVisual in tweens)
                {
                    Undo.RecordObject(tweenVisual, "Toggling play on start");
                    tweenVisual.playOnStart = playOnStartNow;
                }
            }

            EditorGUILayout.BeginHorizontal();
            bool generateCode = EditorGUILayout.Toggle(new GUIContent("Generate Code", "If C# code will be generated"), mainTarget.generateCode);
            tweens.ForEach(tween => tween.generateCode = generateCode);

            GUIContent restartOnEnableGui = new GUIContent("Restart on enable", "When you enable the gameobject these set of tweens will start again");
            bool newRestartOnEnableValue = EditorGUILayout.Toggle(restartOnEnableGui, mainTarget.restartOnEnable);
            foreach (LeanTweenVisual leanTween in tweens)
            {
                Change(ref leanTween.restartOnEnable, newRestartOnEnableValue, leanTween, "Restart on Enable");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            #region Simulation Stuff

            if (mainTarget.isSimulating)
            {
                GUI.enabled = false;
            }

            EditorGUILayout.BeginHorizontal();
            bool simulateTemp = EditorGUILayout.Toggle(new GUIContent("Simulate", "Mark to see the tweens in the edit mode."), mainTarget.simulate);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            // Reset Values
            EditorGUILayout.BeginHorizontal();
            if (!mainTarget.isSimulating)
                GUI.enabled = false;
            bool stopSimulation = EditorGUILayout.Toggle(new GUIContent("Stop simulation", "Click to stop the simulation"), mainTarget.stopSimulation);
            GUI.enabled = true;
            foreach (LeanTweenVisual visual in tweens)
            {
                if (visual.isSimulating)
                    visual.stopSimulation = stopSimulation;
            }

            if (mainTarget.isSimulating)
                GUI.enabled = false;
            bool resetValues = EditorGUILayout.Toggle(new GUIContent("Reset Values", "Click when the values of your GameObject don't reset after simulation."),
                                                    mainTarget.resetValues);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < tweens.Length; i++)
            {
                if (!tweens[i].isSimulating)
                {
                    if (resetValues && tweens[i].hasSimulated)
                    {
                        ResetValues(tweens[i]);
                    }
                }
                else
                {
                    if (stopSimulation)
                    {
                        tweens[i].Cancel();
                        StopSimulation(tweens[i]);
                    }
                }
            }

            for (int i = 0; i < tweens.Length; i++)
            {
                if (!tweens[i].isSimulating)
                {
                    tweens[i].simulate = simulateTemp;
                    if (simulateTemp)
                    {
                        tweens[i].isSimulating = true;

                        GetStartValues(tweens[i]);
                        tweens[i].simulationTimer = tweens[i].GetAllGroupsTotalDuration() + Time.realtimeSinceStartup + tweens[i].timeToFinishSimulation;
                        if (qttOfAtualSimulations == 0)
                            EditorApplication.update += () => SimulationTimer(tweens);

                        if (tweens[i].loopAllCount < 0)
                        {
                            tweens[i].resetRepeatAfterSimulation = true;
                            tweens[i].loopAllCount = tweens[i].defaultLoopCount;
                        }
                        LeanTween.canUseInEditMode = true;
                        tweens[i].Simulate();

                        tweens[i].simulate = false;
                    }
                }
            }

            EditorGUILayout.BeginHorizontal();
            Change(ref mainTarget.timeToFinishSimulation,
                EditorGUILayout.FloatField(new GUIContent("Time To Finish", "Time until restart this GameObject when simulate finishes."), mainTarget.timeToFinishSimulation),
                mainTarget,
                "Time to Finish");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUIContent defaultLoopGui = new GUIContent("Default Loop Count",
                                                    "Default loop count in a simulation when loopCount is infinite. (In Play Mode will work normally)");

            Change(ref mainTarget.defaultLoopCount,
                EditorGUILayout.IntField(defaultLoopGui, mainTarget.defaultLoopCount),
                mainTarget,
                "Default loop count");

            if (mainTarget.defaultLoopCount < 2)
                mainTarget.defaultLoopCount = 2;

            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.Separator();

            #region Repeat Stuff

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("All Delay", "Delay before start the whole set of tween groups."), GUILayout.Width(90));
            float newAllDelay = EditorGUILayout.FloatField("", mainTarget.allDelay);
            for (int i = 0; i < tweens.Length; i++)
            {
                Change(ref tweens[i].allDelay, newAllDelay, tweens[i], "All delay");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            bool newDoesAllLoop = EditorGUILayout.Toggle(new GUIContent("Loop All", "Repeat the whole set of tween groups once they finish"), mainTarget.doesAllLoop);
            for (int i = 0; i < tweens.Length; i++)
            {
                Change(ref tweens[i].doesAllLoop, newDoesAllLoop, tweens[i], "Loop All");
            }
            EditorGUILayout.EndHorizontal();

            if (mainTarget.doesAllLoop)
            {
                float newLoopAllDelay = EditorGUILayout.FloatField(new GUIContent("    Loop All Delay", "Delay between each all loop."), mainTarget.loopAllDelay);
                for (int i = 0; i < tweens.Length; i++)
                {
                    Change(ref tweens[i].loopAllDelay, newLoopAllDelay, tweens[i], "Loop All Delay");
                }

                int newLoopAllCount = EditorGUILayout.IntField(new GUIContent("    Loop All Count", "Quantity of loops all tweens will make."), mainTarget.loopAllCount);
                for (int i = 0; i < tweens.Length; i++)
                {
                    Change(ref tweens[i].loopAllCount, newLoopAllCount, tweens[i], "Loop All Count");
                }

                if (mainTarget.loopAllCount == 0 || mainTarget.loopAllCount == 1)
                {
                    tweens.ForEach(x => x.loopAllCount = 2);
                }
            }
            #endregion


            float addedGroupDelay = 0f;
            int groupIndex = 0;
            foreach (LeanTweenGroup group in mainTarget.groupList)
            {
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUI.color = LTEditor.Shared.colorGroupName;

                group.foldout = EditorGUILayout.Foldout(group.foldout, "", LTEditor.Shared.styleGroupFoldout);

                string groupStatus = "Group: " + group.groupName + " " + (group.StartTime) + "s - " + (mainTarget.GetGroupEndTime(group) + group.StartTime) + "s";
                if (group.doesLoop)
                {
                    if (group.loopCount < 1)
                    {
                        groupStatus += "    (* %%)";
                    }
                    else
                    {
                        groupStatus += "    (* " + group.loopCount + ")";
                    }
                }

                clicked = GUILayout.Button(groupStatus, LTEditor.Shared.styleGroupButton);
                GUI.color = LTEditor.Shared.colorDelete;
                deleted = GUILayout.Button("Delete", LTEditor.Shared.styleDeleteGroupButton);
                EditorGUILayout.EndHorizontal();
                GUI.color = Color.white;

                if (clicked)
                {
                    group.foldout = !group.foldout;
                }
                if (deleted)
                {
                    Undo.RecordObject(mainTarget, "Removing group item");
                    mainTarget.groupList.Remove(group);
                    break;
                }

                float addedTweenDelay = 0f;
                if (group.foldout)
                {
                    group.groupName = EditorGUILayout.TextField("Group Name", group.groupName);
                    EditorGUILayout.BeginHorizontal();
                    group.doesLoop = EditorGUILayout.Toggle("Group Loop", group.doesLoop);
                    group.delay = EditorGUILayout.FloatField("Group Delay", group.delay);
                    EditorGUILayout.EndHorizontal();

                    if (group.doesLoop)
                    {
                        group.loopCount = EditorGUILayout.IntField("Group Loop Count", group.loopCount);
                        if (group.loopCount == 0 || group.loopCount == 1)
                        {
                            group.loopCount = 2;
                        }

                        group.loopDelay = EditorGUILayout.FloatField(new GUIContent("Group Loop Delay", "Delay between each group loop"), group.loopDelay);
                    }

                    group.gameObject = EditorGUILayout.ObjectField("Group GameObject", group.gameObject, typeof(GameObject), true) as GameObject;
                    if (group.gameObject == null)
                    { // Should default to the current object
                        group.gameObject = mainTarget.gameObject;
                    }

                    int i = 0;
                    foreach (LeanTweenItem item in group.itemList)
                    {
                        if (item.actionStr == null)
                        {
                            item.actionStr = "MOVE";
                        }
                        //if (item.actionStr != null)
                        // {

                        #region TweenStatus and DeleteButton
                        EditorGUILayout.Separator();
                        GUI.color = LTEditor.Shared.colorTweenName;
                        EditorGUILayout.BeginHorizontal();

                        string actionName = LTVisualShared.methodLabels[LTVisualShared.actionIndex(item)];
                        item.foldout = EditorGUILayout.Foldout(item.foldout, "", LTEditor.Shared.styleItemFoldout);

                        string itemStatus = actionName + ": " + (item.delay) + "s - " + (item.delay + item.duration) + "s";
                        if (item.doesLoop)
                        {
                            if (item.loopCount < 0)
                            {
                                itemStatus += "    (* %%)";
                            }
                            else
                            {
                                itemStatus += "    (* " + item.loopCount + ")";
                            }
                        }

                        bool tweenClicked = GUILayout.Button(itemStatus, LTEditor.Shared.styleItemButton);
                        if (tweenClicked)
                        {
                            item.foldout = !item.foldout;
                        }

                        GUI.color = LTEditor.Shared.colorDelete;
                        deleted = GUILayout.Button("Delete", LTEditor.Shared.styleDeleteGroupButton);
                        EditorGUILayout.EndHorizontal();
                        GUI.color = Color.white;
                        #endregion

                        if (clicked)
                        {
                            item.foldout = !item.foldout;
                        }
                        if (deleted)
                        {
                            Undo.RecordObject(mainTarget, "Removing tween item");
                            group.itemList.Remove(item);

                            break;
                        }

                        if (item.foldout)
                        {
                            #region Action Field
                            EditorGUILayout.BeginHorizontal();

                            EditorGUILayout.LabelField("    Action", GUILayout.Width(70));
                            int atualIndex = LTVisualShared.actionIndex(item);
                            int newActionIndex = EditorGUILayout.Popup("", atualIndex, LTVisualShared.methodLabels, GUILayout.Width(160));
                            bool changeAction = false;
                            if (newActionIndex != atualIndex)
                            {
                                Undo.RecordObject(mainTarget, "Change action index");
                                LTVisualShared.SetActionIndex(item, newActionIndex);
                                changeAction = true;
                                item.alignToPath = false;
                            }
                            TweenAction a = (TweenAction)Enum.Parse(typeof(TweenAction), item.actionStr, true);
                            item.action = a;
                            item.actionLast = (int)item.action;

                            EditorGUILayout.EndHorizontal();
                            #endregion

                            item.gameObject = EditorGUILayout.ObjectField("    GameObject", item.gameObject, typeof(GameObject), true/*, GUILayout.Width(250)*/) as GameObject;
                            if (item.gameObject == null)
                            { // Should default to the current object
                                item.gameObject = mainTarget.gameObject;
                            }

                            // Path
                            bool isBezier = a == TweenAction.MoveBezier || a == TweenAction.MoveBezierLocal || a == TweenAction.CanvasMoveBezier ||
                                a == TweenAction.CanvasMoveBezierLocal;
                            bool isSpline = a == TweenAction.MoveSplineLocal || a == TweenAction.MoveSpline;
                            bool isCurve = false;
                            if (isBezier || isSpline)
                            {
                                isCurve = true;

                                if (isBezier)
                                {
                                    item.bezierPath = EditorGUILayout.ObjectField("    LeanTweenPath:", item.bezierPath, typeof(LeanTweenPath), true) as LeanTweenPath;
                                }
                                else
                                {
                                    item.splinePath = EditorGUILayout.ObjectField("    LeanTweenPath:", item.splinePath, typeof(LeanTweenPath), true) as LeanTweenPath;
                                }

                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField("   Orient to Path", GUILayout.Width(95));
                                Change(ref item.orientToPath, EditorGUILayout.Toggle(item.orientToPath), mainTarget, "Orient to Path");

                                EditorGUILayout.LabelField(new GUIContent("    Align to Path", "Click to align the object with the start of the path."),
                                                            GUILayout.Width(90));
                                bool align = EditorGUILayout.Toggle(item.alignToPath);
                                bool changed = align != wasAligned ? true : false;
                                wasAligned = align;

                                Undo.RecordObject(mainTarget, "Aligned to path");
                                item.alignToPath = align;

                                if (a != TweenAction.MoveBezierLocal && a != TweenAction.MoveSplineLocal && a != TweenAction.CanvasMoveBezierLocal)
                                {
                                    if (item.alignToPath)
                                    {
                                        if (isBezier)
                                        {
                                            LTBezierPath lTBezier = new LTBezierPath(item.bezierPath.vec3);

                                            Vector3 pt0 = lTBezier.Point(0.002f);
                                            mainTarget.transform.position = pt0;

                                            Vector3 added = lTBezier.Point(0.003f);
                                            Vector3 v3Dir = added - mainTarget.transform.position;
                                            float angle = Mathf.Atan2(v3Dir.y, v3Dir.x) * Mathf.Rad2Deg;
                                            mainTarget.transform.eulerAngles = new Vector3(0, 0, angle);
                                        }
                                        else
                                        {
                                            mainTarget.transform.position = item.splinePath.splineVector()[0];
                                        }
                                    }
                                    else
                                    {
                                        if (changed && !changeAction)
                                        {
                                            mainTarget.transform.position = notAlignedPosition;
                                            mainTarget.transform.eulerAngles = notAlignedRotation;
                                        }
                                        else
                                        {
                                            notAlignedPosition = mainTarget.transform.position;
                                            notAlignedRotation = mainTarget.transform.eulerAngles;
                                        }
                                    }
                                }

                                if (isBezier)
                                {
                                    EditorGUILayout.LabelField("    2D Path", GUILayout.Width(65));
                                    Change(ref item.isPath2d, EditorGUILayout.Toggle(item.isPath2d), mainTarget, "2D Path");
                                }

                                EditorGUILayout.EndHorizontal();
                            }

                            if (isCurve == false)
                            {
                                bool isVector = a == TweenAction.Move || a == TweenAction.MoveLocal || a == TweenAction.MoveAdd || a == TweenAction.CanvasMove ||
                                                 a == TweenAction.CanvasMoveAdd || a == TweenAction.Scale || a == TweenAction.ScaleAdd || a == TweenAction.CanvasScale ||
                                                 a == TweenAction.CanvasScaleAdd || a == TweenAction.CanvasSize || a == TweenAction.DelayedSound || a == TweenAction.CanvasRotate ||
                                                 a == TweenAction.CanvasSizeAdd || a == TweenAction.RotateAdd || a == TweenAction.CanvasRotateAdd || a == TweenAction.CanvasRotateLocal ||
                                                 a == TweenAction.Rotate || a == TweenAction.RotateLocal;

                                bool isColor = a == TweenAction.Color || a == TweenAction.CanvasColor || a == TweenAction.CanvasTextColor || a == TweenAction.ColorGroup;

                                bool isAlpha = a == TweenAction.Alpha || a == TweenAction.AlphaVertex || a == TweenAction.CanvasAlpha || a == TweenAction.CanvasTextAlpha ||
                                                a == TweenAction.CanvasGroupAlpha || a == TweenAction.AlphaGroup;

                                bool isPlay = a == TweenAction.CanvasPlaySprite;
                                bool usesFrom = !isColor && !isPlay;

                                // From Values
                                EditorGUILayout.BeginHorizontal();
                                if (usesFrom)
                                { // Not a Color tween
                                    EditorGUILayout.LabelField(new GUIContent("    From", "Specify where the tween starts from, otherwise it will start from it's current value"), GUILayout.Width(50));
                                    LeanTweenBetween between = EditorGUILayout.Toggle("", item.between == LeanTweenBetween.FromTo, GUILayout.Width(30)) ? LeanTweenBetween.FromTo : LeanTweenBetween.To;
                                    if (between != item.between)
                                    {
                                        Undo.RecordObject(mainTarget, "Changing to from/to");
                                        item.between = between;
                                    }
                                }
                                if (item.between == LeanTweenBetween.FromTo)
                                {
                                    if (isVector)
                                    {
                                        //										item.from = EditorGUILayout.Vector3Field("", item.from);
                                        Change(ref item.from, EditorGUILayout.Vector3Field("", item.from), mainTarget, "Changing from");
                                    }
                                    else if (isColor)
                                    {

                                    }
                                    else
                                    {
                                        vec = Vector3.zero;
                                        vec.x = EditorGUILayout.FloatField("From", item.from.x);

                                        if (vec.x != item.from.x)
                                        {
                                            Undo.RecordObject(mainTarget, "Setting new from value");
                                            item.from = vec;
                                        }
                                    }
                                }
                                EditorGUILayout.EndHorizontal();

                                // To Values
                                EditorGUILayout.BeginHorizontal();
                                if (isVector)
                                {
                                    EditorGUILayout.LabelField("    To", GUILayout.Width(85));
                                    Change(ref item.to, EditorGUILayout.Vector3Field("", item.to), mainTarget, "Changing vector3 to");
                                }
                                else if (isColor)
                                {
                                    EditorGUILayout.LabelField("    To", GUILayout.Width(85));
                                    Change(ref item.colorTo, EditorGUILayout.ColorField("", item.colorTo), mainTarget, "Change color to");
                                }
                                else if (isPlay)
                                {
                                    item.doesLoop = true;

                                    GUILayout.Space(24);
                                    item.spritesMaximized = EditorGUILayout.Foldout(item.spritesMaximized, "Sprites");
                                    if (item.spritesMaximized)
                                    {
                                        EditorGUILayout.LabelField("Add", GUILayout.Width(35));
                                        UnityEngine.Sprite sprite = EditorGUILayout.ObjectField("", null, typeof(UnityEngine.Sprite), true, GUILayout.Width(150)) as UnityEngine.Sprite;
                                        if (sprite != null)
                                        {
                                            Undo.RecordObject(mainTarget, "Adding a sprite");
                                            item.sprites = Add(item.sprites, sprite);
                                        }
                                        EditorGUILayout.Separator();
                                        EditorGUILayout.Separator();
                                        EditorGUILayout.Separator();
                                    }
                                }
                                else
                                {
                                    vec = Vector3.zero;
                                    EditorGUILayout.LabelField("    To", GUILayout.Width(85));

                                    float setToX;
                                    if (isAlpha)
                                    {
                                        setToX = EditorGUILayout.Slider("", item.to.x, 0, 1f);
                                    }
                                    else
                                    {
                                        setToX = EditorGUILayout.FloatField("", item.to.x);
                                    }


                                    Undo.RecordObject(mainTarget, "Setting x to");
                                    vec.x = setToX;
                                    item.to = vec;
                                }
                                EditorGUILayout.EndHorizontal();

                                // Sprite List
                                if (isPlay && item.spritesMaximized)
                                {
                                    for (int j = 0; j < item.sprites.Length; j++)
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                        EditorGUILayout.LabelField("        sprite" + j, GUILayout.Width(85));
                                        item.sprites[j] = EditorGUILayout.ObjectField("", item.sprites[j], typeof(UnityEngine.Sprite), true) as UnityEngine.Sprite;
                                        GUI.color = LTEditor.Shared.colorDelete;
                                        deleted = GUILayout.Button("Delete", LTEditor.Shared.styleDeleteButton);
                                        GUI.color = Color.white;
                                        EditorGUILayout.EndHorizontal();

                                        if (deleted)
                                        {
                                            Undo.RecordObject(mainTarget, "Removing sprite");
                                            item.sprites = Remove(item.sprites, j);
                                            break;
                                        }
                                    }
                                }
                            }
                            EditorGUILayout.Space();

                            // Easing
                            if (a == TweenAction.DelayedSound)
                            {
                                Change(ref item.audioClip, EditorGUILayout.ObjectField("    AudioClip:", item.audioClip, typeof(AudioClip), true) as AudioClip, mainTarget, "set audio clip");
                            }
                            else
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField("    Easing", GUILayout.Width(70));

                                int easeIndex = LTVisualShared.EaseIndex(item);
                                int easeIndexNew = EditorGUILayout.Popup("", easeIndex, LTVisualShared.easeStrMapping, GUILayout.Width(128));
                                if (easeIndex != easeIndexNew)
                                {
                                    Undo.RecordObject(mainTarget, "changing easing type");
                                    LTVisualShared.SetEaseIndex(item, easeIndexNew);
                                }

                                EditorGUILayout.Separator();
                                EditorGUILayout.EndHorizontal();

                                if (item.ease == LeanTweenType.AnimationCurve)
                                {
                                    Undo.RecordObject(mainTarget, "changing easing type anim curve");
                                    item.animationCurve = EditorGUILayout.CurveField("    Ease Curve", item.animationCurve);
                                }
                                EditorGUILayout.Space();
                            }
                            if (item.ease >= LeanTweenType.Once && item.ease < LeanTweenType.AnimationCurve)
                            {
                                EditorGUILayout.LabelField(new GUIContent("   ERROR: You Specified a non-easing type", "Select a type with the value 'Ease' in front of it (or linear)"), EditorStyles.boldLabel);
                            }

                            // Speed

                            EditorGUILayout.BeginHorizontal();
                            Change(ref item.useSpeed, EditorGUILayout.Toggle("    Use Speed", item.useSpeed), mainTarget, "toggled use speed");
                            EditorGUILayout.EndHorizontal();

                            // Timing
                            if (i > 0)
                            {
                                Change(ref item.alignWithPrevious, EditorGUILayout.Toggle(new GUIContent("    Align with Previous", "When you change the timing of a previous tween, this tween's timing will be adjusted to follow afterwards."), item.alignWithPrevious),
                                    mainTarget, "toggle align with previous");
                            }
                            EditorGUILayout.BeginHorizontal();
                            if (i > 0 && item.alignWithPrevious)
                            {
                                Change(ref item.delay, addedTweenDelay, mainTarget, "change delay");
                                EditorGUILayout.LabelField("    Delay:   " + item.delay, GUILayout.Width(50));
                            }
                            else
                            {
                                EditorGUILayout.LabelField("    Delay", GUILayout.Width(50));
                                Change(ref item.delay, EditorGUILayout.FloatField("", item.delay, GUILayout.Width(50)), mainTarget, "change delay");
                            }

                            if (a == TweenAction.DelayedSound)
                            {
                                EditorGUILayout.LabelField("Volume", GUILayout.Width(50));
                                Change(ref item.duration, EditorGUILayout.FloatField("", item.duration, GUILayout.Width(50)), mainTarget, "change volume");
                            }
                            else if (a == TweenAction.CanvasPlaySprite)
                            {
                                EditorGUILayout.LabelField("Frame Rate", GUILayout.Width(85));
                                Change(ref item.frameRate, EditorGUILayout.FloatField("", item.frameRate, GUILayout.Width(50)), mainTarget, "change volume");
                            }
                            else if (item.useSpeed)
                            {
                                EditorGUILayout.LabelField("Speed", GUILayout.Width(50));
                                Change(ref item.speed, EditorGUILayout.FloatField("", item.speed, GUILayout.Width(50)), mainTarget, "change speed");
                            }
                            else
                            {
                                EditorGUILayout.LabelField("Time", GUILayout.Width(50));
                                float newDuration = EditorGUILayout.FloatField("", item.duration, GUILayout.Width(50));
                                if (newDuration <= 0.0f)
                                    newDuration = 0.0001f;
                                Change(ref item.duration, newDuration, mainTarget, "change timing");
                            }
                            EditorGUILayout.Separator();
                            EditorGUILayout.EndHorizontal();


                            // Repeat
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("    Loops", GUILayout.Width(50));
                            Change(ref item.doesLoop, EditorGUILayout.Toggle("", item.doesLoop, GUILayout.Width(50)), mainTarget, "Toggled does loop");

                            if (item.doesLoop)
                            {
                                EditorGUILayout.LabelField(new GUIContent("Repeat", "-1 repeats infinitely"), GUILayout.Width(50));
                                Change(ref item.loopCount, EditorGUILayout.IntField(new GUIContent("", ""), item.loopCount, GUILayout.Width(50)), mainTarget, "changed loop count");
                                if (item.loopCount == 0 || item.loopCount == 1)
                                {
                                    item.loopCount = 2;
                                }
                                EditorGUILayout.LabelField(new GUIContent("    Wrap", "How the tween repeats\nClamp: restart from beginning\nPingpong: goes back and forth\nAdd: not reset when finished."), GUILayout.Width(50));
                                int index = (int)item.loopType - 1; // -1 cause Once is not in list Normal list -> {Once, Clamp, PingPong, Add};
                                index = EditorGUILayout.Popup("", index, new string[] { "Clamp", "Ping Pong", "Add" }, GUILayout.Width(70));
                                EditorGUILayout.EndHorizontal();

                                LoopType newLoopType = LoopType.Clamp;
                                switch (index)
                                {
                                    case 0:
                                        newLoopType = LoopType.Clamp;
                                        break;
                                    case 1:
                                        newLoopType = LoopType.PingPong;
                                        break;
                                    case 2:
                                        newLoopType = LoopType.Add;
                                        break;
                                }

                                if (newLoopType != item.loopType)
                                {
                                    Undo.RecordObject(mainTarget, "change loop type");
                                    item.loopType = newLoopType;
                                }

                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(new GUIContent("    LoopDelay", "Delay between each loop of this tween."), GUILayout.Width(90));
                                Change(ref item.loopDelay, EditorGUILayout.FloatField("", item.loopDelay, GUILayout.Width(50)), mainTarget, "Changed loop delay");
                                if (item.loopDelay < 0)
                                {
                                    item.loopDelay = 0;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            else
                            {
                                EditorGUILayout.EndHorizontal();
                            }

                            EditorGUILayout.Separator();

                            EditorGUILayout.BeginHorizontal();
                            //EditorGUILayout.LabelField("", GUILayout.Width(100));

                            item.callbackFoldout = EditorGUILayout.Foldout(item.callbackFoldout, "", LTEditor.Shared.styleCallbackFoldout);

                            GUI.color = Color.gray;
                            bool callbacksClicked = GUILayout.Button("Callbacks", LTEditor.Shared.styleCallbackButton);
                            GUI.color = Color.white;

                            EditorGUILayout.EndHorizontal();

                            if (callbacksClicked)
                            {
                                item.callbackFoldout = !item.callbackFoldout;
                            }

                            if (item.callbackFoldout)
                            {
                                #region Draw UnityEvents
                                SerializedProperty prop = serializedObject.GetIterator();
                                while (prop.NextVisible(true))
                                {
                                    string itemPath = $"groupList.Array.data[{groupIndex}].itemList.Array.data[{i}]";
                                    bool drawProperty = (prop.propertyPath == itemPath + ".onCompleteLoop") ||
                                                        (prop.propertyPath == itemPath + ".onCompleteItem");

                                    if (drawProperty)
                                    {
                                        EditorGUILayout.Separator();

                                        EditorGUILayout.BeginHorizontal();
                                        EditorGUILayout.LabelField("", GUILayout.Width(28));
                                        EditorGUILayout.PropertyField(prop);
                                        EditorGUILayout.EndHorizontal();
                                    }
                                }
                                #endregion
                            }

                            addedTweenDelay = item.duration + item.delay + (item.loopDelay * item.loopCount);

                            EditorGUILayout.Separator();
                            EditorGUILayout.Separator();

                            i++;
                        }
                    }
                    EditorGUILayout.Separator();
                    if (ShowLeftButton("+ Tween", LTEditor.Shared.colorAddTween, 15))
                    {
                        Undo.RecordObject(mainTarget, "adding another tween");
                        LeanTweenItem newItem = new LeanTweenItem(addedTweenDelay);
                        //newItem.alignWithPrevious = true;
                        group.itemList.Add(newItem);
                    }
                    addedGroupDelay += addedTweenDelay;

                    EditorGUILayout.Separator();
                }
                overallDelay += mainTarget.GetGroupEndTime(group);
                groupIndex++;
            }

            EditorGUILayout.Separator();
            if (ShowLeftButton("+ Group", LTEditor.Shared.colorAddGroup))
            {
                Undo.RecordObject(mainTarget, "adding another group");
                // Debug.Log("adding group with delay:"+addedGroupDelay);
                mainTarget.groupList.Add(new LeanTweenGroup(addedGroupDelay));
            }

            if (mainTarget.generateCode && !mainTarget.isSimulating)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Generated C# Code", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                if (Application.isPlaying == false)
                {
                    scrollCodeViewPos = EditorGUILayout.BeginScrollView(scrollCodeViewPos, GUILayout.Height(150));

                    EditorGUILayout.TextArea(mainTarget.BuildAllTweens(true, true), LTEditor.Shared.styleCodeTextArea);

                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.LabelField("    Not available during runtime");
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndChangeCheck();
            serializedObject.ApplyModifiedProperties();
        }

        private static void Change(ref bool setItem, bool compareTo, LeanTweenVisual tween, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(tween, message);
                setItem = compareTo;
            }
        }

        private static void Change(ref int setItem, int compareTo, LeanTweenVisual tween, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(tween, message);
                setItem = compareTo;
            }
        }

        private static void Change(ref Vector3 setItem, Vector3 compareTo, LeanTweenVisual tween, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(tween, message);
                setItem = compareTo;
            }
        }

        private static void Change(ref float setItem, float compareTo, LeanTweenVisual tween, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(tween, message);
                setItem = compareTo;
            }
        }

        private static void Change(ref Color setItem, Color compareTo, LeanTweenVisual tween, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(tween, message);
                setItem = compareTo;
            }
        }

        private static void Change(ref AudioClip setItem, AudioClip compareTo, LeanTweenVisual tween, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(tween, message);
                setItem = compareTo;
            }
        }

        // Sprite Methods
        private Sprite[] Add(Sprite[] arr, Sprite sprite)
        {
            Sprite[] newArr = new Sprite[arr.Length + 1];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i];
            }
            newArr[newArr.Length - 1] = sprite;
            return newArr;
        }

        private Sprite[] Remove(Sprite[] arr, int removePt)
        {
            Sprite[] newArr = new Sprite[arr.Length - 1];
            int k = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != removePt)
                {
                    newArr[k] = arr[i];
                    k++;
                }
            }
            return newArr;
        }

        // Editor Methods
        private bool ShowLeftButton(string label, Color color)
        {
            return ShowLeftButton(label, color, 0);
        }

        private bool ShowLeftButton(string label, Color color, float space)
        {
            bool clicked;
            GUI.color = color;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(space);
            clicked = GUILayout.Button(label, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;
            return clicked;
        }

        private Vector3 ShowAxis(string label, Vector3 value)
        {
            Vector3 axis = EditorGUILayout.Vector3Field("    Axis", value);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.Label("Presets:");
            if (GUILayout.Button("Up"))
                axis = Vector3.up;
            if (GUILayout.Button("Down"))
                axis = Vector3.down;
            if (GUILayout.Button("Left"))
                axis = Vector3.left;
            if (GUILayout.Button("Right"))
                axis = Vector3.right;
            if (GUILayout.Button("Back"))
                axis = Vector3.back;
            if (GUILayout.Button("Forw"))
                axis = Vector3.forward;
            EditorGUILayout.EndHorizontal();

            return axis;
        }
    }
}
