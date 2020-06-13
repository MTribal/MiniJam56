using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

namespace My_Utils
{
    public class RendererGroup : MonoBehaviour
    {
        public RendererType rendererType;
        private RendererType _rendererTypeTemp;

        protected List<GenericRenderer> allRenderers;

        public bool useRemoteObjects;

        public List<GameObject> remoteObjects = new List<GameObject>();

        private void Start()
        {
            AtualizeRenderers();
            _rendererTypeTemp = rendererType;
        }

        protected virtual void Update()
        {
            // GetComponents
            if (rendererType != _rendererTypeTemp)
            {
                AtualizeRenderers();
                _rendererTypeTemp = rendererType;
            }
        }

        /// <summary>
        /// Get all renderers of the component again.
        /// </summary>
        public virtual void AtualizeRenderers()
        {
            List<GenericRenderer> newRenderers = transform.GetAllRenderers(rendererType);

            if (useRemoteObjects)
            {
                foreach (GameObject remoteObject in remoteObjects)
                {
                    RendererType objectRendererType = RendererGroupUtils.GetRendererType(remoteObject);
                    switch (objectRendererType)
                    {
                        case RendererType.SpriteRenderer:
                            newRenderers.Add(new GenericRenderer(remoteObject.GetComponent<SpriteRenderer>()));
                            break;

                        case RendererType.ColorGroup:
                            newRenderers.Add(new GenericRenderer(null, remoteObject.GetComponent<ColorGroup>()));
                            break;

                        case RendererType.AlphaGroup:
                            newRenderers.Add(new GenericRenderer(null, null, remoteObject.GetComponent<AlphaGroup>()));
                            break;

                        case RendererType.Tilemap:
                            newRenderers.Add(new GenericRenderer(null, null, null, remoteObject.GetComponent<Tilemap>()));
                            break;

                        case RendererType.Image:
                            newRenderers.Add(new GenericRenderer(null, null, null, null, remoteObject.GetComponent<Image>()));
                            break;

                        case RendererType.RawImage:
                            newRenderers.Add(new GenericRenderer(null, null, null, null, null, remoteObject.GetComponent<RawImage>()));
                            break;

                        case RendererType.Text:
                            newRenderers.Add(new GenericRenderer(null, null, null, null, null, null, remoteObject.GetComponent<Text>()));
                            break;

                        case RendererType.TmProText:
                            newRenderers.Add(new GenericRenderer(null, null, null, null, null, null, null, remoteObject.GetComponent<TextMeshProUGUI>()));
                            break;

                        case RendererType.CanvasGroup:
                            newRenderers.Add(new GenericRenderer(null, null, null, null, null, null, null, null, remoteObject.GetComponent<CanvasGroup>()));
                            break;
                    }
                }
            }

            allRenderers = newRenderers;
        }
    }
}