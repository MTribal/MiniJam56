using UnityEngine;
using System;

namespace My_Utils
{
    /// <summary>
    /// An easy way of changing all childs (and recursive childs) color component at once.
    /// </summary>
    [ExecuteAlways]
    [Serializable]
    public class ColorGroup : RendererGroup
    {
        [Space]
        [Tooltip("The atual color of the group.")]
        public Color color;

        protected override void Update()
        {
            base.Update();

            allRenderers.ForEach(renderer => renderer.Color = color);
        }
    }
}

