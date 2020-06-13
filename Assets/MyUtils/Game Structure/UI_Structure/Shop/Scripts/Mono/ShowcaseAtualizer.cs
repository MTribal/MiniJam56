using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// Atualize all showcase sections in the shop by three ways: 1 - In the start of scene; 2 - When shop active sections is changed; 3 - By trigger.
    /// </summary>
    [ExecuteInEditMode]
    public class ShowcaseAtualizer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Mark to atualize all sections of the store.")]
        private bool atualize = false;

        [Space]

        [SerializeField]
        [Tooltip("All sections in the store.")]
        private ItensGridManager[] itensManagers = new ItensGridManager[0];

        private void Start()
        {
            AtualizeAll();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (atualize)
            {
                atualize = false;
                AtualizeAll();
            }
#endif
        }

        internal void AtualizeAll()
        {
            for (int i = 0; i < itensManagers.Length; i++)
            {
                itensManagers[i].AtualizeItens();
            }
        }
    }
}
