using UnityEngine;
using System;
using My_Utils.Lean_Tween.Visual;

namespace My_Utils.Lean_Tween
{
    public static class LeanTweenExt
    {
        //LeanTween.addListener
        //LeanTween.alpha
        public static LTDescr LeanAlpha(this GameObject gameObject, float to, float time, bool useRecursion) { return LeanTween.Alpha(gameObject, to, time, useRecursion); }
        //LeanTween.alphaCanvas
        public static LTDescr LeanAlphaVertex(this GameObject gameObject, float to, float time, bool useRecursion) { return LeanTween.AlphaVertex(gameObject, to, time, useRecursion); }
        //LeanTween.alpha (RectTransform)
        public static LTDescr LeanAlpha(this RectTransform rectTransform, float to, float time, bool useRecursion) { return LeanTween.Alpha(rectTransform, to, time, useRecursion); }
        //LeanTween.alphaCanvas
        public static LTDescr LeanAlpha(this CanvasGroup canvas, float to, float time) { return LeanTween.AlphaCanvas(canvas, to, time); }
        //LeanTween.alphaText
        public static LTDescr LeanAlphaText(this RectTransform rectTransform, float to, float time, bool useRecursion) { return LeanTween.TextAlpha(rectTransform, to, time, useRecursion); }
        //LeanTween.cancel
        public static void LeanCancel(this GameObject gameObject) { LeanTween.Cancel(gameObject); }
        public static void LeanCancel(this GameObject gameObject, bool callOnComplete) { LeanTween.cancel(gameObject, callOnComplete); }
        public static void LeanCancel(this GameObject gameObject, int uniqueId, bool callOnComplete = false) { LeanTween.cancel(gameObject, uniqueId, callOnComplete); }
        //LeanTween.cancel
        public static void LeanCancel(this RectTransform rectTransform) { LeanTween.cancel(rectTransform); }
        //LeanTween.cancelAll
        //LeanTween.color
        public static LTDescr LeanColor(this GameObject gameObject, Color to, float time, bool useRecursion) { return LeanTween.Color(gameObject, to, time, useRecursion); }
        //LeanTween.colorText
        public static LTDescr LeanColorText(this RectTransform rectTransform, Color to, float time, bool useRecursion) { return LeanTween.TextColor(rectTransform, to, time, useRecursion); }
        //LeanTween.delayedCall
        public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, System.Action callback) { return LeanTween.DelayedCall(gameObject, delayTime, callback); }
        public static LTDescr LeanDelayedCall(this GameObject gameObject, float delayTime, System.Action<object> callback) { return LeanTween.DelayedCall(gameObject, delayTime, callback); }

        //LeanTween.isPaused
        public static bool LeanIsPaused(this GameObject gameObject) { return LeanTween.isPaused(gameObject); }
        public static bool LeanIsPaused(this RectTransform rectTransform) { return LeanTween.isPaused(rectTransform); }

        //LeanTween.isTweening
        public static bool LeanIsTweening(this GameObject gameObject) { return LeanTween.isTweening(gameObject); }
        //LeanTween.isTweening
        //LeanTween.move
        public static LTDescr LeanMove(this GameObject gameObject, Vector3 to, float time) { return LeanTween.move(gameObject, to, time); }
        public static LTDescr LeanMove(this Transform transform, Vector3 to, float time) { return LeanTween.move(transform.gameObject, to, time); }
        public static LTDescr LeanMove(this RectTransform rectTransform, Vector3 to, float time) { return LeanTween.Move(rectTransform, to, time); }
        //LeanTween.move
        public static LTDescr LeanMove(this GameObject gameObject, Vector2 to, float time) { return LeanTween.Move(gameObject, to, time); }
        public static LTDescr LeanMove(this Transform transform, Vector2 to, float time) { return LeanTween.Move(transform.gameObject, to, time); }
        //LeanTween.move
        public static LTDescr LeanMove(this GameObject gameObject, LeanTweenPath path, float time) { return LeanTween.Move(gameObject, path, time, true); }
        public static LTDescr LeanMove(this Transform transform, LeanTweenPath path, float time) { return LeanTween.Move(transform.gameObject, path, time, true); }
        //LeanTween.moveLocal
        public static LTDescr LeanMoveLocal(this GameObject gameObject, Vector3 to, float time) { return LeanTween.moveLocal(gameObject, to, time); }
        public static LTDescr LeanMoveLocal(this GameObject gameObject, LeanTweenPath path, float time, bool alignToPath) { return LeanTween.MoveLocal(gameObject, path, time, true, alignToPath); }
        public static LTDescr LeanMoveLocal(this Transform transform, Vector3 to, float time) { return LeanTween.moveLocal(transform.gameObject, to, time); }
        public static LTDescr LeanMoveLocal(this Transform transform, LeanTweenPath path, float time, bool alignToPath) { return LeanTween.MoveLocal(transform.gameObject, path, time, true, alignToPath); }
        //LeanTween.moveLocal
        public static LTDescr LeanMoveLocalX(this GameObject gameObject, float to, float time) { return LeanTween.MoveLocalX(gameObject, to, time); }
        public static LTDescr LeanMoveLocalY(this GameObject gameObject, float to, float time) { return LeanTween.MoveLocalY(gameObject, to, time); }
        public static LTDescr LeanMoveLocalZ(this GameObject gameObject, float to, float time) { return LeanTween.MoveLocalZ(gameObject, to, time); }
        public static LTDescr LeanMoveLocalX(this Transform transform, float to, float time) { return LeanTween.MoveLocalX(transform.gameObject, to, time); }
        public static LTDescr LeanMoveLocalY(this Transform transform, float to, float time) { return LeanTween.MoveLocalY(transform.gameObject, to, time); }
        public static LTDescr LeanMoveLocalZ(this Transform transform, float to, float time) { return LeanTween.MoveLocalZ(transform.gameObject, to, time); }
        //LeanTween.moveSpline
        public static LTDescr LeanMoveSpline(this GameObject gameObject, LeanTweenPath path, float time) { return LeanTween.MoveSpline(gameObject, path, time, true); }
        public static LTDescr LeanMoveSpline(this Transform transform, LeanTweenPath path, float time) { return LeanTween.MoveSpline(transform.gameObject, path, time, true); }
        //LeanTween.moveSplineLocal
        public static LTDescr LeanMoveSplineLocal(this GameObject gameObject, LeanTweenPath path, float time, bool alignToPath) { return LeanTween.MoveSplineLocal(gameObject, path, time, true, alignToPath); }
        public static LTDescr LeanMoveSplineLocal(this Transform transform, LeanTweenPath path, float time, bool alignToPath) { return LeanTween.MoveSplineLocal(transform.gameObject, path, time, true, alignToPath); }
        //LeanTween.moveX
        public static LTDescr LeanMoveX(this GameObject gameObject, float to, float time) { return LeanTween.MoveX(gameObject, to, time); }
        public static LTDescr LeanMoveX(this Transform transform, float to, float time) { return LeanTween.MoveX(transform.gameObject, to, time); }
        //LeanTween.moveX (RectTransform)
        public static LTDescr LeanMoveX(this RectTransform rectTransform, float to, float time) { return LeanTween.MoveX(rectTransform, to, time); }
        //LeanTween.moveY
        public static LTDescr LeanMoveY(this GameObject gameObject, float to, float time) { return LeanTween.moveY(gameObject, to, time); }
        public static LTDescr LeanMoveY(this Transform transform, float to, float time) { return LeanTween.moveY(transform.gameObject, to, time); }
        //LeanTween.moveY (RectTransform)
        public static LTDescr LeanMoveY(this RectTransform rectTransform, float to, float time) { return LeanTween.MoveY(rectTransform, to, time); }
        //LeanTween.moveZ
        public static LTDescr LeanMoveZ(this GameObject gameObject, float to, float time) { return LeanTween.moveZ(gameObject, to, time); }
        public static LTDescr LeanMoveZ(this Transform transform, float to, float time) { return LeanTween.moveZ(transform.gameObject, to, time); }
        //LeanTween.moveZ (RectTransform)
        public static LTDescr LeanMoveZ(this RectTransform rectTransform, float to, float time) { return LeanTween.MoveZ(rectTransform, to, time); }
        //LeanTween.pause
        public static void LeanPause(this GameObject gameObject) { LeanTween.pause(gameObject); }
        //LeanTween.play
        public static LTDescr LeanPlay(this RectTransform rectTransform, UnityEngine.Sprite[] sprites) { return LeanTween.Play(rectTransform, sprites); }
        //LeanTween.removeListener
        //LeanTween.resume
        public static void LeanResume(this GameObject gameObject) { LeanTween.resume(gameObject); }
        //LeanTween.resumeAll
        //LeanTween.rotate 

        public static LTDescr LeanRotate(this GameObject gameObject, Vector3 to, float time) { return LeanTween.Rotate(gameObject, to, time); }
        public static LTDescr LeanRotate(this Transform transform, Vector3 to, float time) { return LeanTween.Rotate(transform.gameObject, to, time); }
        //LeanTween.rotateAround (RectTransform)
        public static LTDescr LeanRotate(this RectTransform rectTransform, Vector3 to, float time) { return LeanTween.Rotate(rectTransform, to, time); }
        //LeanTween.rotateAroundLocal
        public static LTDescr LeanRotateLocal(this GameObject gameObject, Vector3 to, float time) { return LeanTween.RotateLocal(gameObject, to, time); }
        public static LTDescr LeanRotateLocal(this Transform transform, Vector3 to, float time) { return LeanTween.RotateLocal(transform.gameObject, to, time); }
        //LeanTween.RotateLocal (RectTransform)
        public static LTDescr LeanRotateLocal(this RectTransform rectTransform, Vector3 to, float time) { return LeanTween.RotateLocal(rectTransform, to, time); }
        //LeanTween.rotateLocal

        //LeanTween.rotateX
        public static LTDescr LeanRotateX(this GameObject gameObject, float to, float time) { return LeanTween.RotateX(gameObject, to, time); }
        public static LTDescr LeanRotateX(this Transform transform, float to, float time) { return LeanTween.RotateX(transform.gameObject, to, time); }
        //LeanTween.rotateY
        public static LTDescr LeanRotateY(this GameObject gameObject, float to, float time) { return LeanTween.RotateY(gameObject, to, time); }
        public static LTDescr LeanRotateY(this Transform transform, float to, float time) { return LeanTween.RotateY(transform.gameObject, to, time); }
        //LeanTween.rotateZ
        public static LTDescr LeanRotateZ(this GameObject gameObject, float to, float time) { return LeanTween.RotateZ(gameObject, to, time); }
        public static LTDescr LeanRotateZ(this Transform transform, float to, float time) { return LeanTween.RotateZ(transform.gameObject, to, time); }
        //LeanTween.scale
        public static LTDescr LeanScale(this GameObject gameObject, Vector3 to, float time) { return LeanTween.Scale(gameObject, to, time); }
        public static LTDescr LeanScale(this Transform transform, Vector3 to, float time) { return LeanTween.Scale(transform.gameObject, to, time); }
        //LeanTween.scale (GUI)
        //LeanTween.scale (RectTransform)
        public static LTDescr LeanScale(this RectTransform rectTransform, Vector3 to, float time) { return LeanTween.Scale(rectTransform, to, time); }
        //LeanTween.scaleX
        public static LTDescr LeanScaleX(this GameObject gameObject, float to, float time) { return LeanTween.ScaleX(gameObject, to, time); }
        public static LTDescr LeanScaleX(this Transform transform, float to, float time) { return LeanTween.ScaleX(transform.gameObject, to, time); }
        //LeanTween.scaleY
        public static LTDescr LeanScaleY(this GameObject gameObject, float to, float time) { return LeanTween.ScaleY(gameObject, to, time); }
        public static LTDescr LeanScaleY(this Transform transform, float to, float time) { return LeanTween.ScaleY(transform.gameObject, to, time); }
        //LeanTween.scaleZ
        public static LTDescr LeanScaleZ(this GameObject gameObject, float to, float time) { return LeanTween.ScaleZ(gameObject, to, time); }
        public static LTDescr LeanScaleZ(this Transform transform, float to, float time) { return LeanTween.ScaleZ(transform.gameObject, to, time); }
        //LeanTween.sequence
        //LeanTween.size (RectTransform)
        public static LTDescr LeanSize(this RectTransform rectTransform, Vector2 to, float time) { return LeanTween.Size(rectTransform, to, time); }
        //LeanTween.tweensRunning
        //LeanTween.value (Color)
        public static LTDescr LeanValue(this GameObject gameObject, Color from, Color to, float time) { return LeanTween.value(gameObject, from, to, time); }
        //LeanTween.value (Color)
        //LeanTween.value (float)
        public static LTDescr LeanValue(this GameObject gameObject, float from, float to, float time) { return LeanTween.value(gameObject, from, to, time); }
        public static LTDescr LeanValue(this GameObject gameObject, Vector2 from, Vector2 to, float time) { return LeanTween.value(gameObject, from, to, time); }
        public static LTDescr LeanValue(this GameObject gameObject, Vector3 from, Vector3 to, float time) { return LeanTween.value(gameObject, from, to, time); }
        //LeanTween.value (float)
        public static LTDescr LeanValue(this GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time) { return LeanTween.value(gameObject, callOnUpdate, from, to, time); }
        public static LTDescr LeanValue(this GameObject gameObject, Action<float, float> callOnUpdate, float from, float to, float time) { return LeanTween.value(gameObject, callOnUpdate, from, to, time); }
        public static LTDescr LeanValue(this GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time) { return LeanTween.value(gameObject, callOnUpdate, from, to, time); }
        public static LTDescr LeanValue(this GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time) { return LeanTween.value(gameObject, callOnUpdate, from, to, time); }
        public static LTDescr LeanValue(this GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time) { return LeanTween.value(gameObject, callOnUpdate, from, to, time); }
        public static LTDescr LeanValue(this GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time) { return LeanTween.value(gameObject, callOnUpdate, from, to, time); }

        public static void LeanSetPosX(this Transform transform, float val)
        {
            transform.position = new Vector3(val, transform.position.y, transform.position.z);
        }
        public static void LeanSetPosY(this Transform transform, float val)
        {
            transform.position = new Vector3(transform.position.x, val, transform.position.z);
        }
        public static void LeanSetPosZ(this Transform transform, float val)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, val);
        }

        public static void LeanSetLocalPosX(this Transform transform, float val)
        {
            transform.localPosition = new Vector3(val, transform.localPosition.y, transform.localPosition.z);
        }
        public static void LeanSetLocalPosY(this Transform transform, float val)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, val, transform.localPosition.z);
        }
        public static void LeanSetLocalPosZ(this Transform transform, float val)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, val);
        }

        public static Color LeanColor(this Transform transform)
        {
            return transform.GetComponent<Renderer>().material.color;
        }
    }
}

