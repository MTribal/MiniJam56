using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_Utils.Lean_Tween.Visual
{
    // A single lean tween group that can have concurrently
    // running lean tween items.
    [Serializable]
    public class LeanTweenGroup
    {
        // Name of this group.
        public string groupName = "Tweens";

        // Whether the group repeats
        public bool doesLoop = false;

        // The delay before tweens in this group start.
        public float delay = 0;

        public float loopDelay;

        // How many times the repeat is made (-1 means infinite)
        public int loopCount = -1;

        public int loopIter = 0;

        // Foldout used for GUI display.
        public bool foldout = true;

        public GameObject gameObject;

        // way to override all the gameObjects in the group
        public GameObject overrideGameObject;

        // List of concurrent tweens.
        public List<LeanTweenItem> itemList = new List<LeanTweenItem>();

        // Instantiates LeanTweenGroup.
        public LeanTweenGroup()
        {
        }
        public LeanTweenGroup(float delay)
        {
            this.delay = delay;
        }

        // Instantiates LeanTweenGroup by making a copy of group.
        // <param name="group">Group.</param>
        public LeanTweenGroup(LeanTweenGroup group)
        {
            groupName = group.groupName;
            delay = group.delay;
            foldout = group.foldout;
            itemList.Clear();
            foreach (LeanTweenItem item in group.itemList)
            {
                itemList.Add(new LeanTweenItem(item));
            }
        }

        // Gets the time in which the first tween will start
        // including all delays.
        // <value>The start time.</value>
        public float StartTime
        {
            get
            {
                float min = float.MaxValue;
                foreach (LeanTweenItem item in itemList)
                {
                    min = Mathf.Min(item.delay + delay, min);
                }
                if (itemList.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return min;
                }
            }
        }

        public void SetGenerateCode()
        {
            GenerateCode = true;
        }

        public bool GenerateCode { get; private set; } = false;
    }
}