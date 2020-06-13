using System.Collections.Generic;
using UnityEngine;

namespace My_Utils
{
    [ExecuteInEditMode]
    public class IsVisbleGroup : MonoBehaviour
    {
        public bool isVisible;

        /// <summary>
        /// Just to controll when is visible.
        /// </summary>
        private bool isVisibleTwo;

        private List<GenericRenderer> genericRenderers;

        private void Awake()
        {
            ResetRenderers();
        }

        private void Update()
        {
            if (isVisible && !isVisibleTwo)
            {
                ResetRenderers();
                genericRenderers.ForEach(renderer => renderer.Enabled = true);
                isVisibleTwo = true;
            }
            else if (!isVisible && isVisibleTwo)
            {
                ResetRenderers();
                genericRenderers.ForEach(renderer => renderer.Enabled = false);
                isVisibleTwo = false;
            }
        }

        private void ResetRenderers()
        {
            genericRenderers = transform.GetAllRenderers(-1);
        }
    }
}
