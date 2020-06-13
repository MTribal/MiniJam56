using System;
using UnityEngine;
using UnityEngine.Events;

namespace My_Utils.Lean_Tween.Visual
{

    // Contains a single lean tween item.
    [Serializable]
    public class LeanTweenItem 
    {
        public string itemName = "Tween";

        public GameObject gameObject;

        // Holds the action type performed by the tween
        public TweenAction action = TweenAction.Move;
        public int actionLast = -1; // For keeping track of when the action has changed.

        // Holds the action type performed by the tween in string format
        public string actionStr;

        // If the action will be recursive (valid for alphas and colors)
        public bool recursive;

        // Indicates if the action if from a certain value to a certain value or
        // from its current value to a value.
        public LeanTweenBetween between = LeanTweenBetween.To;

        // The easing to use.
        public LeanTweenType ease = LeanTweenType.Linear;
        public string easeStr = "";

        public AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        // The start value if provided.
        // If it is propagated using a float value, it is just stored in the x value.
        public Vector3 from;

        // The end vector value.
        // If it is propagated using a float value, it is just stored in the x value.
        public Vector3 to;

        // The end color value.
        public Color colorTo = Color.red;

        // Axis to rotate around, useful only for rotate around tween.
        public Vector3 axis = Vector3.forward;

        // The duration of the tween.
        public float duration = 1f;

        // The delay of this tween based on the begining of the group.
        public float delay;

        // The delay between each loop of this tween (if has loop)
        public float loopDelay;

        // speed of tween (overrides time)
        public float speed = 1f;

        public bool useSpeed = false; // defaults to time

        // If set this will adjust the delay time of this item to line up with the previous tween
        public bool alignWithPrevious = false;

        // Foldout used for GUI display.
        public bool foldout = true;

        // Bezier Path used if the tween follows along one
        public LeanTweenPath bezierPath;

        // Spline Path used if the tween follows along one
        public LeanTweenPath splinePath;

        // AudioClip used in delayedSound
        public AudioClip audioClip;


        public UnityEvent onCompleteLoop, onCompleteItem; 
        public void CallOnCompleteLoop() => onCompleteLoop.Invoke();
        public void CallOnCompleteItem() => onCompleteItem.Invoke();
        public bool callbackFoldout;


        public bool orientToPath = true; // For use on path tweens
        public bool alignToPath; // Mark for align with the start of the path before tween starts

        // Set the path to behave in a 2d way
        public bool isPath2d = false;

        public bool doesLoop = false;
        public int loopCount = -1;
        public LoopType loopType;

        public Sprite[] sprites = new Sprite[] { };
        public bool spritesMaximized = true;
        public float frameRate = 6f;

        // Instantiates LeanTweenGroup.
        public LeanTweenItem()
        {
        }

        public LeanTweenItem(float delay)
        {
            this.delay = delay;
        }

        // Instantiates LeanTweenGroup by making a copy of group.
        // <param name="group">Group.</param>
        public LeanTweenItem(LeanTweenItem item)
        {
            itemName = item.itemName;
            action = item.action;
            recursive = item.recursive;
            between = item.between;
            ease = item.ease;
            from = item.from;
            to = item.to;
            axis = item.axis;
            duration = item.duration;
            delay = item.delay;
            loopDelay = item.loopDelay;
            foldout = item.foldout;
        }
    }
}
