using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

namespace My_Utils
{
    public static class RendererGroupUtils
    {
        public static bool IsValid(GameObject gameObject)
        {
            if (gameObject != null)
            {
                return gameObject.GetComponent<SpriteRenderer>() || gameObject.GetComponent<ColorGroup>() || gameObject.GetComponent<AlphaGroup>() ||
                    gameObject.GetComponent<Tilemap>() || gameObject.GetComponent<Image>() || gameObject.GetComponent<RawImage>() || gameObject.GetComponent<Text>() ||
                    gameObject.GetComponent<TextMeshProUGUI>() || gameObject.GetComponent<CanvasGroup>();
            }
            else
            {
                return false;
            }
        }

        public static RendererType GetRendererType(GameObject gameObject)
        {
            if (gameObject != null)
            {
                if (gameObject.GetComponent<SpriteRenderer>())
                {
                    return RendererType.SpriteRenderer;
                }
                else if (gameObject.GetComponent<ColorGroup>())
                {
                    return RendererType.ColorGroup;
                }
                else if (gameObject.GetComponent<AlphaGroup>())
                {
                    return RendererType.AlphaGroup;
                }
                else if (gameObject.GetComponent<Tilemap>())
                {
                    return RendererType.Tilemap;
                }
                else if (gameObject.GetComponent<Image>())
                {
                    return RendererType.Image;
                }
                else if (gameObject.GetComponent<RawImage>())
                {
                    return RendererType.RawImage;
                }
                else if (gameObject.GetComponent<Text>())
                {
                    return RendererType.Text;
                }
                else if (gameObject.GetComponent<TextMeshProUGUI>())
                {
                    return RendererType.TmProText;
                }
                else if (gameObject.GetComponent<CanvasGroup>())
                {
                    return RendererType.CanvasGroup;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
