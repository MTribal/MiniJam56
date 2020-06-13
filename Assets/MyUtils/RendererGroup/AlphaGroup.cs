using System.Collections.Generic;
using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// An easy way of changing all childs (and recursive childs) alpha at once.
    /// </summary>
    [ExecuteAlways]
    public class AlphaGroup : RendererGroup
    {
        [Space]
        [Tooltip("The atual alpha of the group.")]
        [Range(0, 1)]
        public float alpha = 1f;

        public bool resetMaxAlphas;

        [SerializeField]
        private List<float> _maxAlphas = new List<float>();

        protected override void Update()
        {
            base.Update();

            if (allRenderers == null)
            {
                Debug.Log("All renderers is null");
                return;
            }

            if (_maxAlphas.Count < allRenderers.Count && !Application.isPlaying)
            {
                _AtualizeMaxAlphas();
            }

            for (int i = 0; i < allRenderers.Count; i++)
            {
                allRenderers[i].Color = new Color(allRenderers[i].Color.r, allRenderers[i].Color.g, allRenderers[i].Color.b, CalculateAtualAlpha(_maxAlphas[i]));
            }
        }

        public override void AtualizeRenderers()
        {
            base.AtualizeRenderers();
        }

        public void _AtualizeMaxAlphas()
        {
            if (allRenderers == null) return;

            _maxAlphas.Clear();
            for (int i = 0; i < allRenderers.Count; i++)
            {
                _maxAlphas.Add(CalculateMaxAlpha(allRenderers[i].Color.a));
            }
        }

        private float CalculateMaxAlpha(float atualAlpha)
        {
            return atualAlpha * 1 / alpha;
        }

        private float CalculateAtualAlpha(float maxAlpha)
        {
            return maxAlpha * alpha / 1;
        }
    }
}

