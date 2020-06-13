using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace My_Utils.Lean_Tween.Visual
{
    public class LTVisualShared : MonoBehaviour
    {

        public static string[] LT_2_12_MethodNames = new string[]{
            "MOVE_X","MOVE_Y","MOVE_Z","MOVE_LOCAL_X","MOVE_LOCAL_Y","MOVE_LOCAL_Z", "MOVE_ADD", "MOVE_CURVED","MOVE_CURVED_LOCAL","MOVE_SPLINE","MOVE_SPLINE_LOCAL", "CANVAS_MOVE", "CANVAS_MOVE_X", "CANVAS_MOVE_Y", "CANVAS_MOVE_Z", "CANVAS_MOVE_ADD", "SCALE_X","SCALE_Y","SCALE_Z", "SCALE_ADD", "ROTATE_X","ROTATE_Y","ROTATE_Z","ROTATE_AROUND","ALPHA","ALPHA_VERTEX","CALLBACK","MOVE","MOVE_LOCAL", "SCALE","VALUE","GUI_MOVE","GUI_MOVE_MARGIN","GUI_SCALE","GUI_ALPHA","GUI_ROTATE","DELAYED_SOUND"
        };

        // 0 - Transform 
        // 1 - RectTransform
        public static object[][] methodLabelsGrouping = new object[][]{
            new object[]{"Move","0"},
            new object[]{"MoveX","0"},
            new object[]{"MoveY","0"},
            new object[]{"MoveZ","0"},
            new object[]{"MoveLocal","0"},
            new object[]{"MoveLocalX","0"},
            new object[]{"MoveLocalY","0"},
            new object[]{"MoveLocalZ","0"},
            new object[]{"MoveAdd","0"},
            new object[]{"MoveBezier","0"},
            new object[]{"MoveLocalBezier","0"},
            new object[]{"MoveSpline","0"},
            new object[]{"MoveSplineLocal","0"},
            new object[]{"Move (UI)","1"},
            new object[]{"MoveX (UI)","1"},
            new object[]{"MoveY (UI)","1"},
            new object[]{"MoveZ (UI)","1"},
            new object[]{"MoveAdd (UI)","1"},
            new object[]{"MoveBezier (UI)","1"},
            new object[]{"MoveBezierLocal (UI)","1"},

            new object[]{"Rotate","0"},
            new object[]{"RotateLocal","0"},
            new object[]{"RotateX","0"},
            new object[]{"RotateY","0"},
            new object[]{"RotateZ","0"},
            new object[]{"RotateAdd","0"},
            new object[]{"CanvasRotate","1"},
            new object[]{"CanvasRotateLocal","1"},
            new object[]{"CanvasRotateAdd","1"},

            new object[]{"Scale","0"},
            new object[]{"ScaleX","0"},
            new object[]{"ScaleY","0"},
            new object[]{"ScaleZ","0"},
            new object[]{"ScaleAdd","0"},
            new object[]{"Scale (UI)","1"},
            new object[]{ "ScaleAdd (UI)", "1"},
            new object[]{ "Size (UI)", "1"},
            new object[]{ "SizeAdd (UI)", "1"},

            new object[]{"Color","0"},
            new object[]{"ColorGroup","0"},
            new object[]{"Alpha","0"},
            new object[]{"AlphaGroup","0"},
            new object[]{"AlphaVertex","0"},

            new object[]{ "PlaySprite (UI)", "1"},
            new object[]{"Alpha (Image)","1"},
            new object[]{"Alpha (Text)","1"},
            new object[]{"Alpha (CanvasGroup)","1"},
            new object[]{"AlphaGroup","1"},
            new object[]{"Color (Image)","1"},
            new object[]{"Color (Text)","1"},
            new object[]{"ColorGroup","1"},

            new object[]{"DelayedSound","0"},

            new object[]{"Event","0"},
            new object[]{"Event","1"},
        };

        public static string[] methodLabels;
        public static string[] methodStrMapping;
        public static int[] methodIntMapping;

        public static string[] methodStrMappingGrouping = new string[]{
            "Move",
            "MoveX",
            "MoveY",
            "MoveZ",
            "MoveLocal",
            "MoveLocalX",
            "MoveLocalY",
            "MoveLocalZ",
            "MoveAdd",
            "MoveBezier",
            "MoveBezierLocal",
            "MoveSpline",
            "MoveSplineLocal",
            "CanvasMove",
            "CanvasMoveX",
            "CanvasMoveY",
            "CanvasMoveZ",
            "CanvasMoveAdd",
            "CanvasMoveBezier",
            "CanvasMoveBezierLocal",

            "Rotate",
            "RotateLocal",
            "RotateX",
            "RotateY",
            "RotateZ",
            "RotateAdd",
            "CanvasRotate",
            "CanvasRotateLocal",
            "CanvasRotateAdd",

            "Scale",
            "ScaleX",
            "ScaleY",
            "ScaleZ",
            "ScaleAdd",
            "CanvasScale",
            "CanvasScaleAdd",
            "CanvasSize",
            "CanvasSizeAdd",

            "Color",
            "ColorGroup",
            "Alpha",
            "AlphaGroup",
            "AlphaVertex",

            "CanvasPlaySprite",
            "CanvasAlpha",
            "CanvasTextAlpha",
            "CanvasGroupAlpha",
            "AlphaGroup",       // This is for canvas
            "CanvasColor",
            "CanvasTextColor",
            "ColorGroup",      // This is for canvas

            "DelayedSound",

            "Event",
			
            // ????
			"Value3",
            "GuiMove",
            "GuiMoveMargin",
            "GuiScale",
            "GuiAlpha",
            "GuiRotate",
            "CallBackColor",
            "CallBack"
            // ????
        };
        public static int[] methodIntMappingGrouping = new int[]{
            (int)TweenAction.Move,
            (int)TweenAction.MoveX,
            (int)TweenAction.MoveY,
            (int)TweenAction.MoveZ,
            (int)TweenAction.MoveLocal,
            (int)TweenAction.MoveLocalX,
            (int)TweenAction.MoveLocalY,
            (int)TweenAction.MoveLocalZ,
            (int)TweenAction.MoveAdd,
            (int)TweenAction.MoveBezier,
            (int)TweenAction.MoveBezierLocal,
            (int)TweenAction.MoveSpline,
            (int)TweenAction.MoveSplineLocal,
            (int)TweenAction.CanvasMove,
            (int)TweenAction.CanvasMoveX,
            (int)TweenAction.CanvasMoveY,
            (int)TweenAction.CanvasMoveZ,
            (int)TweenAction.CanvasMoveAdd,
            (int)TweenAction.CanvasMoveBezier,
            (int)TweenAction.CanvasMoveBezierLocal,

            (int)TweenAction.Rotate,
            (int)TweenAction.RotateLocal,
            (int)TweenAction.RotateX,
            (int)TweenAction.RotateY,
            (int)TweenAction.RotateZ,
            (int)TweenAction.RotateAdd,
            (int)TweenAction.CanvasRotate,
            (int)TweenAction.CanvasRotateLocal,
            (int)TweenAction.CanvasRotateAdd,

            (int)TweenAction.Scale,
            (int)TweenAction.ScaleX,
            (int)TweenAction.ScaleY,
            (int)TweenAction.ScaleZ,
            (int)TweenAction.ScaleAdd,
            (int)TweenAction.CanvasScale,
            (int)TweenAction.CanvasScaleAdd,
            (int)TweenAction.CanvasSize,
            (int)TweenAction.CanvasSizeAdd,

            (int)TweenAction.Color,
            (int)TweenAction.ColorGroup,
            (int)TweenAction.Alpha,
            (int)TweenAction.AlphaGroup,
            (int)TweenAction.AlphaVertex,
            (int)TweenAction.CanvasPlaySprite,
            (int)TweenAction.CanvasAlpha,
            (int)TweenAction.CanvasTextAlpha,
            (int)TweenAction.CanvasGroupAlpha,
            (int)TweenAction.AlphaGroup,
            (int)TweenAction.CanvasColor,
            (int)TweenAction.CanvasTextColor,
            (int)TweenAction.ColorGroup,

            (int)TweenAction.DelayedSound,
            (int)TweenAction.Event,

            (int)TweenAction.Value3,
            (int)TweenAction.GuiMove,
            (int)TweenAction.GuiMoveMargin,
            (int)TweenAction.GuiScale,
            (int)TweenAction.GuiAlpha,
            (int)TweenAction.GuiRotate,
            (int)TweenAction.CallBackColor,
            (int)TweenAction.CallBack,
        };

        public static int[] easeIntMapping = new int[]
        {
            (int)LeanTweenType.NotUsed, (int)LeanTweenType.Linear, (int)LeanTweenType.EaseOutQuad, (int)LeanTweenType.EaseInQuad, (int)LeanTweenType.EaseInOutQuad, (int)LeanTweenType.EaseInCubic, (int)LeanTweenType.EaseOutCubic, (int)LeanTweenType.EaseInOutCubic, (int)LeanTweenType.EaseInQuart, (int)LeanTweenType.EaseOutQuart, (int)LeanTweenType.EaseInOutQuart,
            (int)LeanTweenType.EaseInQuint, (int)LeanTweenType.EaseOutQuint, (int)LeanTweenType.EaseInOutQuint, (int)LeanTweenType.EaseInSine, (int)LeanTweenType.EaseOutSine, (int)LeanTweenType.EaseInOutSine, (int)LeanTweenType.EaseInExpo, (int)LeanTweenType.EaseOutExpo, (int)LeanTweenType.EaseInOutExpo, (int)LeanTweenType.EaseInCirc, (int)LeanTweenType.EaseOutCirc,
            (int)LeanTweenType.EaseInOutCirc, (int)LeanTweenType.EaseInBounce, (int)LeanTweenType.EaseOutBounce, (int)LeanTweenType.EaseInOutBounce, (int)LeanTweenType.EaseInBack, (int)LeanTweenType.EaseOutBack, (int)LeanTweenType.EaseInOutBack, (int)LeanTweenType.EaseInElastic, (int)LeanTweenType.EaseOutElastic, (int)LeanTweenType.EaseInOutElastic,
            (int)LeanTweenType.EaseSpring, (int)LeanTweenType.EaseShake, (int)LeanTweenType.Punch, (int)LeanTweenType.Once, (int)LeanTweenType.AnimationCurve
        };

        public static string[] easeStrMapping = new string[]{
            "NotUsed", "Linear", "EaseOutQuad", "EaseInQuad", "EaseInOutQuad", "EaseInCubic", "EaseOutCubic", "EaseInOutCubic", "EaseInQuart", "EaseOutQuart", "EaseInOutQuart",
            "EaseInQuint", "EaseOutQuint", "EaseInOutQuint", "EaseInSine", "EaseOutSine", "EaseInOutSine", "EaseInExpo", "EaseOutExpo", "EaseInOutExpo", "EaseInCirc", "EaseOutCirc",
            "EaseInOutCirc", "EaseInBounce", "EaseOutBounce", "EaseInOutBounce", "EaseInBack", "EaseOutBack", "EaseInOutBack", "EaseInElastic", "EaseOutElastic", "EaseInOutElastic",
            "EaseSpring", "EaseShake", "Punch", "Once", "AnimationCurve"
        };

        public static void UpdateTweens(LeanTweenVisual tween)
        {
            //LeanTweenVisual tween = target as LeanTweenVisual;

            // Debug.Log("tween.versionNum:"+tween.versionNum);
            if (tween.versionNum == 0)
            { // upgrade script from 2.12
                foreach (LeanTweenGroup group in tween.groupList)
                {
                    foreach (LeanTweenItem item in group.itemList)
                    {
                        // search through old lookup table for enums to convert over item.action to new string based saved format
                        Debug.Log("to action:" + (int)item.action);
                        if ((int)item.action < LT_2_12_MethodNames.Length - 1)
                        {
                            item.actionStr = LT_2_12_MethodNames[(int)item.action];
                        }
                        else
                        {
                            item.actionStr = LT_2_12_MethodNames[LT_2_12_MethodNames.Length - 1];
                        }

                        Debug.Log("item.action toStr:" + item.actionStr);
                    }
                }
                tween.versionNum = 220;
            }
            else
            {
                tween.versionNum = 220;
            }
            string editType = "0";
#if !UNITY_4_3 && !UNITY_4_5
            if (tween.gameObject.GetComponent<RectTransform>())
            {
                editType = "1";
                //Debug.Log("tween :"+tween+" is RectTransform");
            }
            else
            {
                //Debug.Log("tween :"+tween+" is Transform");
            }
#endif

            // Debug.Log("OnEnable methodLabels:"+methodLabels);
            //if(methodLabels==null){
            int labelsCount = 0;
            for (int i = 0; i < methodLabelsGrouping.Length; i++)
            {
                for (int k = 0; k < methodLabelsGrouping[i].Length; k++)
                {
                    if (methodLabelsGrouping[i][k].Equals(editType))
                        labelsCount++;
                }
            }

            methodLabels = new string[labelsCount];
            methodStrMapping = new string[labelsCount];
            methodIntMapping = new int[labelsCount];
            int inputIter = 0;
            for (int i = 0; i < methodLabelsGrouping.Length; i++)
            {
                for (int k = 0; k < methodLabelsGrouping[i].Length; k++)
                {
                    if (methodLabelsGrouping[i][k].Equals(editType))
                    {
                        methodLabels[inputIter] = (string)methodLabelsGrouping[i][0];
                        methodStrMapping[inputIter] = methodStrMappingGrouping[i];
                        methodIntMapping[inputIter] = methodIntMappingGrouping[i];
                        inputIter++;
                    }
                }
            }
            //}

            foreach (LeanTweenGroup group in tween.groupList)
            {
                foreach (LeanTweenItem item in group.itemList)
                {
                    int actionIndex = LTVisualShared.actionIndex(item);
                    item.action = (TweenAction)methodIntMapping[actionIndex];
                    item.actionStr = methodStrMapping[actionIndex];
                }
            }
        }

        public static int actionIndex(LeanTweenItem item)
        {
            int actionIndex = -1;
            for (int j = 0; j < methodStrMapping.Length; j++)
            {
                if (item.actionStr == methodStrMapping[j])
                {
                    actionIndex = j;
                    // Debug.Log("found match actionIndex:"+actionIndex + " methodStrMapping[actionIndex]:"+methodStrMapping[actionIndex]);
                    break;
                }
            }
            if (actionIndex < 0) // nothing found set to intial 
                actionIndex = 0;
            return actionIndex;
        }

        public static void SetActionIndex(LeanTweenItem item, int index)
        {
            item.action = (TweenAction)methodIntMapping[index];
            item.actionStr = methodStrMapping[index];
        }

        public static int EaseIndex(LeanTweenItem item)
        {
            if (item.easeStr.Length <= 0)
            { // First Time setup
                item.easeStr = easeStrMapping[(int)item.ease];
                if (item.ease == LeanTweenType.NotUsed)
                    item.easeStr = "linear";
            }
            int easeIndex = -1; // easeIndex( item )
            for (int j = 0; j < easeStrMapping.Length; j++)
            {
                if (item.easeStr == easeStrMapping[j])
                {
                    easeIndex = j;
                    // Debug.Log("found match easeIndex:"+easeIndex + " easeStrMapping[easeIndex]:"+easeStrMapping[easeIndex]);
                    break;
                }
            }
            if (easeIndex < 0) // nothing found set to intial 
                easeIndex = 0;
            return easeIndex;
        }

        public static void SetEaseIndex(LeanTweenItem item, int newIndex)
        {
            item.ease = (LeanTweenType)LTVisualShared.easeIntMapping[newIndex];
            item.easeStr = LTVisualShared.easeStrMapping[newIndex];
        }

        // Helper Methods for the Undo Class
#if UNITY_EDITOR
        public static void change(ref bool setItem, bool compareTo, UnityEngine.Object obj, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(obj, message);
                setItem = compareTo;
            }
        }

        public static void change(ref int setItem, int compareTo, UnityEngine.Object obj, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(obj, message);
                setItem = compareTo;
            }
        }

        public static void change(ref Vector3 setItem, Vector3 compareTo, UnityEngine.Object obj, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(obj, message);
                setItem = compareTo;
            }
        }

        public static void change(ref float setItem, float compareTo, UnityEngine.Object obj, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(obj, message);
                setItem = compareTo;
            }
        }

        public static void change(ref Color setItem, Color compareTo, UnityEngine.Object obj, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(obj, message);
                setItem = compareTo;
            }
        }

        public static void change(ref AudioClip setItem, AudioClip compareTo, UnityEngine.Object obj, string message)
        {
            if (compareTo != setItem)
            {
                Undo.RecordObject(obj, message);
                setItem = compareTo;
            }
        }
#endif

    }// end LTVisual Shared

}
