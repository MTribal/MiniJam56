using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

namespace My_Utils
{
    /// <summary>
    /// A easy way to treat many types of renderers at once.
    /// </summary>
    public class GenericRenderer
    {
        private readonly SpriteRenderer _spriteRenderer;
        private readonly ColorGroup _colorGroup;
        private readonly AlphaGroup _alphaGroup;
        private readonly Tilemap _tilemap;
        private readonly Image _image;
        private readonly RawImage _rawImage;
        private readonly Text _text;
        private readonly TextMeshProUGUI _tmProText;
        private readonly CanvasGroup _canvasGroup;

        private Color _color;
        private bool _enabled;
        private Sprite _sprite;

        public GenericRenderer(SpriteRenderer spriteRenderer = null, ColorGroup colorGroup = null, AlphaGroup alphaGroup = null, Tilemap tilemap = null, Image image = null,
            RawImage rawImage = null, Text text = null, TextMeshProUGUI tmProText = null, CanvasGroup canvasGroup = null)
        {
            #region Exceptions
            bool[] list = new bool[] { spriteRenderer, colorGroup, alphaGroup, tilemap, image, rawImage, text, tmProText, canvasGroup };

            int qttOfNull = 0;
            for (int i = 0; i < list.Length; i++) // false == null, true == not null
            {
                if (!list[i])
                {
                    qttOfNull++;
                }
            }

            if (qttOfNull < list.Length - 1) // More than 1 ins't null
            {
                //Debug.LogError("<GenericRenderer> More than one renderer ins't null, conflict.");
            }
            else if (qttOfNull > list.Length - 1) // All is null
            {
                //Debug.LogError("<GenericRenderer> All renderers is null.");
            }
            #endregion

            _spriteRenderer = spriteRenderer;
            _colorGroup = colorGroup;
            _alphaGroup = alphaGroup;
            _tilemap = tilemap;
            _image = image;
            _rawImage = rawImage;
            _text = text;
            _tmProText = tmProText;
            _canvasGroup = canvasGroup;

            StartValues();
        }

        private void StartValues()
        {
            if (_spriteRenderer != null)
            {
                _color = _spriteRenderer.color;
                _enabled = _spriteRenderer.enabled;
                _sprite = _spriteRenderer.sprite;
            }
            else if (_colorGroup != null)
            {
                _color = _colorGroup.color;
                _enabled = _colorGroup.enabled;
            }
            else if (_alphaGroup != null)
            {
                _color = new Color(0, 0, 0, _alphaGroup.alpha);
                _enabled = _alphaGroup.enabled;
            }
            else if (_tilemap != null)
            {
                _color = _tilemap.color;
                _enabled = _tilemap.enabled;
            }
            else if (_image != null)
            {
                _color = _image.color;
                _enabled = _image.enabled;
                _sprite = _image.sprite;
            }
            else if (_rawImage != null)
            {
                _color = _rawImage.color;
                _enabled = _rawImage.enabled;
            }
            else if (_text != null)
            {
                _color = _text.color;
                _enabled = _text.enabled;
            }
            else if (_tmProText != null)
            {
                _color = _tmProText.color;
                _enabled = _tmProText.enabled;
            }
            else if (_canvasGroup != null)
            {
                _color = new Color(0, 0, 0, _canvasGroup.alpha);
                _enabled = _canvasGroup.enabled;
            }
        }

        public bool IsAllNull()
        {
            return (_spriteRenderer == null && _colorGroup == null && _alphaGroup == null && _tilemap == null && _image == null && _rawImage == null && _text == null &&
                        _tmProText == null && _canvasGroup == null);
        }

        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;

                if (_spriteRenderer != null)
                {
                    _spriteRenderer.color = value;
                }
                if (_colorGroup != null)
                {
                    _colorGroup.color = value;
                }
                if (_alphaGroup != null)
                {
                    _alphaGroup.alpha = value.a;
                }
                if (_tilemap != null)
                {
                    _tilemap.color = value;
                }
                if (_image != null)
                {
                    _image.color = value;
                }
                if (_rawImage != null)
                {
                    _rawImage.color = value;
                }
                if (_text != null)
                {
                    _text.color = value;
                }
                if (_tmProText != null)
                {
                    _tmProText.color = value;
                }
                if (_canvasGroup != null)
                {
                    _canvasGroup.alpha = value.a;
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;

                if (_spriteRenderer != null)
                {
                    _spriteRenderer.enabled = value;
                }
                if (_colorGroup != null)
                {
                    _colorGroup.enabled = value;
                }
                if (_alphaGroup != null)
                {
                    _alphaGroup.enabled = value;
                }
                if (_tilemap != null)
                {
                    _tilemap.enabled = value;
                }
                if (_image != null)
                {
                    _image.enabled = value;
                }
                if (_rawImage != null)
                {
                    _rawImage.enabled = value;
                }
                if (_text != null)
                {
                    _text.enabled = value;
                }
                if (_tmProText != null)
                {
                    _tmProText.enabled = value;
                }
                if (_canvasGroup != null)
                {
                    _canvasGroup.enabled = value;
                }
            }
        }

        public Sprite Sprite
        {
            get
            {
                return _sprite;
            }
            set
            {
                _sprite = value;

                if (_spriteRenderer != null)
                {
                    _spriteRenderer.sprite = value;
                }
                if (_image != null)
                {
                    _image.sprite = value;
                }
            }
        }
    }
}
