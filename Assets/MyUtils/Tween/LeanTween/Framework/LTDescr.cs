using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using My_Utils;

/**
* Internal Representation of a Tween<br>
* <br>
* This class represents all of the optional parameters you can pass to a method (it also represents the internal representation of the tween).<br><br>
* <strong id='optional'>Optional Parameters</strong> are passed at the end of every method:<br> 
* <br>
* &nbsp;&nbsp;<i>Example:</i><br>
* &nbsp;&nbsp;LeanTween.moveX( gameObject, 1f, 1f).setEase( <a href="LeanTweenType.html">LeanTweenType</a>.easeInQuad ).setDelay(1f);<br>
* <br>
* You can pass the optional parameters in any order, and chain on as many as you wish.<br>
* You can also <strong>pass parameters at a later time</strong> by saving a reference to what is returned.<br>
* <br>
* Retrieve a <strong>unique id</strong> for the tween by using the "id" property. You can pass this to LeanTween.pause, LeanTween.resume, LeanTween.cancel, LeanTween.isTweening methods<br>
* <br>
* &nbsp;&nbsp;<h4>Example:</h4>
* &nbsp;&nbsp;int id = LeanTween.moveX(gameObject, 1f, 3f).id;<br>
* <div style="color:gray">&nbsp;&nbsp;// pause a specific tween</div>
* &nbsp;&nbsp;LeanTween.pause(id);<br>
* <div style="color:gray">&nbsp;&nbsp;// resume later</div>
* &nbsp;&nbsp;LeanTween.resume(id);<br>
* <div style="color:gray">&nbsp;&nbsp;// check if it is tweening before kicking of a new tween</div>
* &nbsp;&nbsp;if( LeanTween.isTweening( id ) ){<br>
* &nbsp;&nbsp; &nbsp;&nbsp;	LeanTween.cancel( id );<br>
* &nbsp;&nbsp; &nbsp;&nbsp;	LeanTween.moveZ(gameObject, 10f, 3f);<br>
* &nbsp;&nbsp;}<br>
* @class LTDescr
* @constructor
*/
namespace My_Utils.Lean_Tween
{
    public class LTDescr
    {
        public bool toggle;
        public bool useEstimatedTime;
        public bool useFrames;
        public bool useManualTime;
        public bool usesNormalDt;
        public bool hasInitiliazed;
        public bool hasExtraOnCompletes;
        public bool hasPhysics;
        public bool onCompleteOnRepeat;
        public bool onCompleteOnStart;
        public bool useRecursion;
        public float ratioPassed;
        public float passed;
        public float delay;
        public float loopDelay;
        private float _loopDelay;
        public float time;
        public float speed;
        public float lastVal;
        private uint _id;
        public int loopCount;
        public uint counter = uint.MaxValue;
        public float direction;
        public float directionLast;
        public float overshoot;
        public float period;
        public float scale;
        public bool destroyOnComplete;
        public Transform trans;
        internal Vector3 fromInternal;
        public Vector3 From { get { return fromInternal; } set { fromInternal = value; } }
        internal Vector3 toInternal;
        public Vector3 To { get { return toInternal; } set { toInternal = value; } }
        internal Vector3 diff;
        internal Vector3 diffDiv2;
        public TweenAction type;
        private LeanTweenType easeType;
        public LoopType loopType;

        public bool hasUpdateCallback;

        public EaseTypeDelegate EaseMethod;
        public ActionMethodDelegate EaseInternal { get; set; }
        public ActionMethodDelegate InitInternal { get; set; }

        public delegate Vector3 EaseTypeDelegate();
        public delegate void ActionMethodDelegate();
        public SpriteRenderer spriteRen;

        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;
        public ColorGroup colorGroup;
        public AlphaGroup alphaGroup;
        public Text uiText;
        public TextMeshProUGUI tmText;
        public Image uiImage;
        public RawImage rawImage;
        public Sprite[] sprites;

        // Initial pos of the gameObject
        private Vector3 initialPos;
        private Vector3 initialPosLocal;

        // Convenience Getters
        public Transform ToTrans
        {
            get
            {
                return optional.toTrans;
            }
        }

        public LTDescrOptional _optional = new LTDescrOptional();

        public override string ToString()
        {
            return (trans != null ? "name:" + trans.gameObject.name : "gameObject:null") + " toggle:" + toggle + " passed:" + passed + " time:" + time + " delay:" + delay + " direction:" + direction + " from:" + From + " to:" + To + " diff:" + diff + " type:" + type + " ease:" + easeType + " useEstimatedTime:" + useEstimatedTime + " id:" + Id + " hasInitiliazed:" + hasInitiliazed;
        }

        public LTDescr()
        {

        }

        [System.Obsolete("Use 'LeanTween.cancel( id )' instead")]
        public LTDescr Cancel(GameObject gameObject)
        {
            // Debug.Log("canceling id:"+this._id+" this.uniqueId:"+this.uniqueId+" go:"+this.trans.gameObject);
            if (gameObject == this.trans.gameObject)
                LeanTween.removeTween((int)this._id, this.UniqueId);
            return this;
        }

        public int UniqueId
        {
            get
            {
                uint toId = _id | counter << 16;

                /*uint backId = toId & 0xFFFF;
                uint backCounter = toId >> 16;
                if(_id!=backId || backCounter!=counter){
                    Debug.LogError("BAD CONVERSION toId:"+_id);
                }*/

                return (int)toId;
            }
        }

        public int Id
        {
            get
            {
                return UniqueId;
            }
        }

        public LTDescrOptional optional
        {
            get
            {
                return _optional;
            }
            set
            {
                this._optional = value;
            }
        }

        public LTDescr SetInitialPos(Transform trans)
        {
            RectTransform rectTrans = trans.GetComponent<RectTransform>();
            if (rectTrans == null)
            {
                initialPos = trans.position;
                initialPosLocal = trans.localPosition;
            }
            else
            {
                initialPos = rectTrans.anchoredPosition3D;
                initialPosLocal = rectTrans.localPosition;
            }
            return this;
        }


        public void Reset()
        {
            toggle = useRecursion = usesNormalDt = true;
            trans = null;
            spriteRen = null;
            passed = delay = lastVal = _loopDelay = 0.0f;
            hasUpdateCallback = useEstimatedTime = useFrames = hasInitiliazed = onCompleteOnRepeat = destroyOnComplete = onCompleteOnStart = useManualTime = hasExtraOnCompletes = false;
            easeType = LeanTweenType.Linear;
            loopType = LoopType.Once;
            loopCount = 0;
            direction = directionLast = overshoot = scale = 1.0f;
            period = 0.3f;
            speed = -1f;
            EaseMethod = EaseLinear;
            From = To = Vector3.zero;
            _optional.Reset();
        }

        // Initialize and Internal Methods

        public LTDescr SetFollow()
        {
            this.type = TweenAction.Follow;
            return this;
        }

        public LTDescr SetMoveAdd()
        {
            type = TweenAction.MoveAdd;
            InitInternal = () => { To += trans.position; From = trans.position; };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                trans.position = newVect;
            };

            return this;
        }

        public LTDescr SetMoveX()
        {
            type = TweenAction.MoveX;
            InitInternal = () => { this.fromInternal.x = trans.position.x; };
            EaseInternal = () => { trans.position = new Vector3(EaseMethod().x, trans.position.y, trans.position.z); };
            return this;
        }

        public LTDescr SetMoveY()
        {
            this.type = TweenAction.MoveY;
            this.InitInternal = () => { this.fromInternal.x = trans.position.y; };
            this.EaseInternal = () => { trans.position = new Vector3(trans.position.x, EaseMethod().x, trans.position.z); };
            return this;
        }

        public LTDescr SetMoveZ()
        {
            this.type = TweenAction.MoveZ;
            this.InitInternal = () => { this.fromInternal.x = trans.position.z; }; ;
            this.EaseInternal = () => { trans.position = new Vector3(trans.position.x, trans.position.y, EaseMethod().x); };
            return this;
        }

        public LTDescr SetMoveLocalX()
        {
            this.type = TweenAction.MoveLocalX;
            this.InitInternal = () => { this.fromInternal.x = trans.localPosition.x; };
            this.EaseInternal = () => { trans.localPosition = new Vector3(EaseMethod().x, trans.localPosition.y, trans.localPosition.z); };
            return this;
        }

        public LTDescr SetMoveLocalY()
        {
            this.type = TweenAction.MoveLocalY;
            this.InitInternal = () => { this.fromInternal.x = trans.localPosition.y; };
            this.EaseInternal = () => { trans.localPosition = new Vector3(trans.localPosition.x, EaseMethod().x, trans.localPosition.z); };
            return this;
        }

        public LTDescr SetMoveLocalZ()
        {
            this.type = TweenAction.MoveLocalZ;
            this.InitInternal = () => { this.fromInternal.x = trans.localPosition.z; };
            this.EaseInternal = () => { trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, EaseMethod().x); };
            return this;
        }

        public LTDescr SetOffset(Vector3 offset)
        {
            this.toInternal = offset;
            return this;
        }

        public LTDescr SetMoveCurved(Transform pathTrans)
        {
            type = TweenAction.MoveBezier;
            InitInternal = () =>
            {
                fromInternal.x = 0;
                _optional.path.SetInitialPos(initialPos, initialPosLocal, pathTrans);
            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                if (_optional.path.orientToPath)
                {
                    if (_optional.path.orientToPath2d)
                    {
                        _optional.path.Place2d(trans, val);
                    }
                    else
                    {
                        _optional.path.Place(trans, val);
                    }
                }
                else
                {
                    trans.position = _optional.path.Point(val);
                }
            };
            return this;
        }

        public LTDescr SetMoveCurvedLocal(Transform pathTrans, bool alignToPath)
        {
            type = TweenAction.MoveBezierLocal;
            InitInternal = () =>
            {
                fromInternal.x = 0;
                _optional.path.SetInitialPos(initialPos, initialPosLocal, pathTrans);
            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                if (_optional.path.orientToPath)
                {
                    if (_optional.path.orientToPath2d)
                    {
                        _optional.path.PlaceLocal2d(trans, val, alignToPath);
                    }
                    else
                    {
                        _optional.path.PlaceLocal(trans, val);
                    }
                }
                else
                {
                    trans.localPosition = _optional.path.Point(val);
                }
            };
            return this;
        }

        public LTDescr SetMoveCurvedLocalCanvas(Transform pathTrans, bool alignToPath)
        {
            type = TweenAction.MoveBezierLocal;
            InitInternal = () =>
            {
                fromInternal.x = 0;
                _optional.path.SetInitialPos(initialPos, initialPosLocal, pathTrans);
            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                if (_optional.path.orientToPath)
                {
                    if (_optional.path.orientToPath2d)
                    {
                        _optional.path.PlaceLocal2dCanvas(rectTransform, val, alignToPath);
                    }
                    else
                    {
                        _optional.path.PlaceLocal(rectTransform, val);
                    }
                }
                else
                {
                    rectTransform.localPosition = _optional.path.Point(val).ToCanvasSpace(rectTransform.GetComponentInParents<Canvas>());
                }
            };
            return this;
        }



        public LTDescr SetMoveSpline(Transform pathTrans)
        {
            type = TweenAction.MoveSpline;
            InitInternal = () =>
            {
                fromInternal.x = 0;
                _optional.spline.SetInitialPos(initialPos, initialPosLocal, pathTrans);
            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                if (_optional.spline.orientToPath)
                {
                    if (_optional.spline.orientToPath2d)
                    {
                        _optional.spline.Place2d(trans, val);
                    }
                    else
                    {
                        _optional.spline.place(trans, val);
                    }
                }
                else
                {
                    trans.position = _optional.spline.point(val);
                }
            };
            return this;
        }

        public LTDescr SetMoveSplineLocal(Transform pathTrans, bool alignToPath)
        {
            type = TweenAction.MoveSplineLocal;
            InitInternal = () =>
            {
                fromInternal.x = 0;
                _optional.spline.SetInitialPos(initialPos, initialPosLocal, pathTrans);
            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                if (_optional.spline.orientToPath)
                {
                    if (_optional.spline.orientToPath2d)
                    {
                        _optional.spline.PlaceLocal2d(trans, val, alignToPath);
                    }
                    else
                    {
                        _optional.spline.placeLocal(trans, val);
                    }
                }
                else
                {
                    trans.localPosition = _optional.spline.point(val);
                }
            };
            return this;
        }

        public LTDescr SetScaleX()
        {
            this.type = TweenAction.ScaleX;
            this.InitInternal = () => { this.fromInternal.x = trans.localScale.x; };
            this.EaseInternal = () => { trans.localScale = new Vector3(EaseMethod().x, trans.localScale.y, trans.localScale.z); };
            return this;
        }

        public LTDescr SetScaleY()
        {
            this.type = TweenAction.ScaleY;
            this.InitInternal = () => { this.fromInternal.x = trans.localScale.y; };
            this.EaseInternal = () => { trans.localScale = new Vector3(trans.localScale.x, EaseMethod().x, trans.localScale.z); };
            return this;
        }

        public LTDescr SetScaleZ()
        {
            this.type = TweenAction.ScaleZ;
            this.InitInternal = () => { this.fromInternal.x = trans.localScale.z; };
            this.EaseInternal = () => { trans.localScale = new Vector3(trans.localScale.x, trans.localScale.y, EaseMethod().x); };
            return this;
        }

        public LTDescr SetRotateX()
        {
            this.type = TweenAction.RotateX;
            this.InitInternal = () => { this.fromInternal.x = trans.eulerAngles.x; this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x); };
            this.EaseInternal = () => { trans.eulerAngles = new Vector3(EaseMethod().x, trans.eulerAngles.y, trans.eulerAngles.z); };
            return this;
        }

        public LTDescr SetRotateY()
        {
            this.type = TweenAction.RotateY;
            this.InitInternal = () => { this.fromInternal.x = trans.eulerAngles.y; this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x); };
            this.EaseInternal = () => { trans.eulerAngles = new Vector3(trans.eulerAngles.x, EaseMethod().x, trans.eulerAngles.z); };
            return this;
        }

        public LTDescr SetRotateZ()
        {
            this.type = TweenAction.RotateZ;
            this.InitInternal = () =>
            {
                this.fromInternal.x = trans.eulerAngles.z;
                this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
            };
            this.EaseInternal = () => { trans.eulerAngles = new Vector3(trans.eulerAngles.x, trans.eulerAngles.y, EaseMethod().x); };
            return this;
        }

        public LTDescr SetRotate()
        {
            type = TweenAction.Rotate;
            InitInternal = () =>
            {
                From = trans.eulerAngles;
            };
            EaseInternal = () =>
            {
                trans.eulerAngles = EaseMethod();
            };
            return this;
        }

        public LTDescr SetRotateLocal()
        {
            type = TweenAction.RotateLocal;
            InitInternal = () =>
            {
                From = trans.localEulerAngles;
            };
            EaseInternal = () =>
            {
                trans.localEulerAngles = EaseMethod();
            };
            return this;
        }

        public LTDescr SetRotateAdd()
        {
            type = TweenAction.RotateAdd;
            InitInternal = () =>
            {
                From = trans.localEulerAngles;
                To += trans.localEulerAngles;
            };
            EaseInternal = () =>
            {
                trans.localEulerAngles = EaseMethod();
            };

            return this;
        }

        public LTDescr SetAlpha(bool useRecursion)
        {
            type = TweenAction.Alpha;
            InitInternal = () =>
            {
                this.useRecursion = useRecursion;
                SpriteRenderer ren = trans.GetComponent<SpriteRenderer>();
                if (ren != null)
                {
                    fromInternal.x = ren.color.a;
                }
                else
                {
                    if (trans.GetComponent<Renderer>() != null && trans.GetComponent<Renderer>().material.HasProperty("_Color"))
                    {
                        fromInternal.x = trans.GetComponent<Renderer>().material.color.a;
                    }
                    else if (trans.GetComponent<Renderer>() != null && trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
                    {
                        Color col = trans.GetComponent<Renderer>().material.GetColor("_TintColor");
                        fromInternal.x = col.a;
                    }
                    else if (trans.childCount > 0)
                    {
                        foreach (Transform child in trans)
                        {
                            if (child.gameObject.GetComponent<Renderer>() != null)
                            {
                                Color col = child.gameObject.GetComponent<Renderer>().material.color;
                                fromInternal.x = col.a;
                                break;
                            }
                        }
                    }
                }

                EaseInternal = () =>
                {
                    val = EaseMethod().x;

                    if (spriteRen != null)
                    {
                        spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, val);
                        AlphaRecursiveSprite(trans, val);
                    }
                    else
                    {
                        AlphaRecursive(trans, val, this.useRecursion);
                    }
                };

            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;

                if (spriteRen != null)
                {
                    spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, val);
                    AlphaRecursiveSprite(trans, val);
                }
                else
                {
                    AlphaRecursive(trans, val, this.useRecursion);
                }
            };
            return this;
        }

        public LTDescr SetAlphaGroup(Transform trans)
        {
            type = TweenAction.AlphaGroup;
            alphaGroup = trans.GetComponent<AlphaGroup>();
            if (alphaGroup == null)
            {
                Debug.Log("You are trying to tween a AlphaGroup, but this GameObject hasn't a AlphaGroup component.");
                LeanTween.Cancel(UniqueId);
            }
            InitInternal = () =>
            {
                fromInternal.x = alphaGroup.alpha;
            };
            EaseInternal = () =>
            {
                alphaGroup.alpha = EaseMethod().x;
            };
            return this;
        }


        public LTDescr SetTextAlpha(bool useRecursion)
        {
            type = TweenAction.CanvasTextAlpha;
            InitInternal = () =>
            {
                this.useRecursion = useRecursion;
                uiText = trans.GetComponent<UnityEngine.UI.Text>();
                fromInternal.x = uiText != null ? uiText.color.a : 1f;
            };
            EaseInternal = () =>
            {
                TextAlphaRecursive(trans, EaseMethod().x, this.useRecursion);
            };
            return this;
        }

        public LTDescr setAlphaVertex(bool useRecursion)
        {
            type = TweenAction.AlphaVertex;
            InitInternal = () => { fromInternal.x = trans.GetComponent<MeshFilter>().mesh.colors32[0].a; };
            EaseInternal = () =>
            {
                this.useRecursion = useRecursion;
                newVect = EaseMethod();
                val = newVect.x;
                Mesh mesh = trans.GetComponent<MeshFilter>().mesh;
                Vector3[] vertices = mesh.vertices;
                Color32[] colors = new Color32[vertices.Length];
                if (colors.Length == 0)
                { //MaxFW fix: add vertex colors if the mesh doesn't have any             
                Color32 transparentWhiteColor32 = new Color32(0xff, 0xff, 0xff, 0x00);
                    colors = new Color32[mesh.vertices.Length];
                    for (int k = 0; k < colors.Length; k++)
                        colors[k] = transparentWhiteColor32;
                    mesh.colors32 = colors;
                }// fix end
            Color32 c = mesh.colors32[0];
                c = new Color(c.r, c.g, c.b, val);
                for (int k = 0; k < vertices.Length; k++)
                    colors[k] = c;
                mesh.colors32 = colors;
            };
            return this;
        }

        public LTDescr SetColor(bool useRecursion)
        {
            type = TweenAction.Color;
            InitInternal = () =>
            {
                this.useRecursion = useRecursion;
                SpriteRenderer renColor = trans.GetComponent<SpriteRenderer>();
                if (renColor != null)
                {
                    SetFromColor(renColor.color);
                }
                else
                {
                    if (trans.GetComponent<Renderer>() != null && trans.GetComponent<Renderer>().material.HasProperty("_Color"))
                    {
                        Color col = trans.GetComponent<Renderer>().material.color;
                        SetFromColor(col);
                    }
                    else if (trans.GetComponent<Renderer>() != null && trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
                    {
                        Color col = trans.GetComponent<Renderer>().material.GetColor("_TintColor");
                        SetFromColor(col);
                    }
                    else if (trans.childCount > 0)
                    {
                        foreach (Transform child in trans)
                        {
                            if (child.gameObject.GetComponent<Renderer>() != null)
                            {
                                Color col = child.gameObject.GetComponent<Renderer>().material.color;
                                SetFromColor(col);
                                break;
                            }
                        }
                    }
                }
            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                Color toColor = TweenColor(this, val);


                if (this.spriteRen != null)
                {
                    spriteRen.color = toColor;
                    ColorRecursiveSprite(trans, toColor);
                }
                else
                {
                // Debug.Log("val:"+val+" tween:"+tween+" tween.diff:"+tween.diff);
                if (type == TweenAction.Color)
                        ColorRecursive(trans, toColor, this.useRecursion);

                }
                if (dt != 0f && _optional.onUpdateColor != null)
                {
                    _optional.onUpdateColor(toColor);
                }
                else if (dt != 0f && _optional.onUpdateColorObject != null)
                {
                    _optional.onUpdateColorObject(toColor, _optional.onUpdateParam);
                }
            };
            return this;
        }

        public LTDescr SetCallbackColor()
        {
            this.type = TweenAction.CallBackColor;
            this.InitInternal = () => { this.diff = new Vector3(1.0f, 0.0f, 0.0f); };
            this.EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                Color toColor = TweenColor(this, val);

                if (this.spriteRen != null)
                {
                    this.spriteRen.color = toColor;
                    ColorRecursiveSprite(trans, toColor);
                }
                else
                {
                // Debug.Log("val:"+val+" tween:"+tween+" tween.diff:"+tween.diff);
                if (this.type == TweenAction.Color)
                        ColorRecursive(trans, toColor, this.useRecursion);

                }
                if (dt != 0f && this._optional.onUpdateColor != null)
                {
                    this._optional.onUpdateColor(toColor);
                }
                else if (dt != 0f && this._optional.onUpdateColorObject != null)
                {
                    this._optional.onUpdateColorObject(toColor, this._optional.onUpdateParam);
                }
            };
            return this;
        }



        public LTDescr SetTextColor(bool useRecursion)
        {
            type = TweenAction.CanvasTextColor;
            InitInternal = () =>
            {
                this.useRecursion = useRecursion;
                uiText = trans.GetComponent<Text>();
                tmText = trans.GetComponent<TextMeshProUGUI>();
                if (uiText != null)
                {
                    SetFromColor(uiText.color);
                }
                else if (tmText != null)
                {
                    SetFromColor(tmText.color);
                }
                else
                {
                    Debug.LogWarning("You are trying to tween text, but this gameObject doesn't has a Text or TextMeshProUGUI component.");
                    SetFromColor(Color.white);
                }
            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                Color toColor = TweenColor(this, val);

                if (uiText != null)
                {
                    uiText.color = toColor;
                }
                else if (tmText != null)
                {
                    tmText.color = toColor;
                }


                if (dt != 0f && _optional.onUpdateColor != null)
                    _optional.onUpdateColor(toColor);

                if (this.useRecursion && trans.childCount > 0)
                    TextColorRecursive(trans, toColor);
            };
            return this;
        }

        public LTDescr SetCanvasAlpha(bool useRecursion)
        {
            type = TweenAction.CanvasAlpha;
            InitInternal = () =>
            {
                this.useRecursion = useRecursion;
                uiImage = trans.GetComponent<Image>();
                if (uiImage != null)
                {
                    fromInternal.x = uiImage.color.a;
                }
                else
                {
                    rawImage = trans.GetComponent<RawImage>();
                    if (rawImage != null)
                    {
                        fromInternal.x = rawImage.color.a;
                    }
                    else
                    {
                        fromInternal.x = 1f;
                    }
                }

            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                if (uiImage != null)
                {
                    Color c = uiImage.color; c.a = val; uiImage.color = c;
                }
                else if (rawImage != null)
                {
                    Color c = rawImage.color; c.a = val; rawImage.color = c;
                }
                if (useRecursion)
                {
                    AlphaRecursive(rectTransform, val, 0);
                    TextAlphaChildrenRecursive(rectTransform, val);
                }
            };
            return this;
        }

        public LTDescr SetCanvasGroupAlpha(Transform trans)
        {
            type = TweenAction.CanvasGroupAlpha;
            canvasGroup = trans.GetComponent<CanvasGroup>();

            InitInternal = () =>
            {
                if (canvasGroup != null)
                {
                    fromInternal.x = trans.GetComponent<CanvasGroup>().alpha;
                }

            };

            if (canvasGroup == null)
            {
                Debug.LogWarning("You are trying to tween canvas group, but this gameObject doesn't has a CanvasGroup component.");
                LeanTween.Cancel(UniqueId);
            }

            EaseInternal = () =>
            {
                canvasGroup.alpha = EaseMethod().x;
            };
            return this;
        }

        public LTDescr SetCanvasColor(bool useRecursion)
        {
            type = TweenAction.CanvasColor;
            InitInternal = () =>
            {
                this.useRecursion = useRecursion;
                uiImage = trans.GetComponent<Image>();
                if (uiImage == null)
                {
                    rawImage = trans.GetComponent<RawImage>();
                    SetFromColor(rawImage != null ? rawImage.color : Color.white);
                }
                else
                {
                    SetFromColor(uiImage.color);
                }

            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                Color toColor = TweenColor(this, val);
                if (uiImage != null)
                {
                    uiImage.color = toColor;
                }
                else if (rawImage != null)
                {
                    rawImage.color = toColor;
                }

                if (dt != 0f && _optional.onUpdateColor != null)
                    _optional.onUpdateColor(toColor);

                if (this.useRecursion)
                    ColorRecursive(rectTransform, toColor);
            };
            return this;
        }

        public LTDescr SetColorGroup(Transform trans)
        {
            type = TweenAction.ColorGroup;
            colorGroup = trans.GetComponent<ColorGroup>();
            if (colorGroup == null)
            {
                Debug.Log("You are trying to tween a ColorGroup, but this GameObject hasn't a ColorGroup component.");
                LeanTween.Cancel(UniqueId);
            }

            InitInternal = () =>
            {
                SetFromColor(colorGroup.color);

            };
            EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                Color toColor = TweenColor(this, val);
                colorGroup.color = toColor;

                if (dt != 0f && _optional.onUpdateColor != null)
                    _optional.onUpdateColor(toColor);
            };
            return this;
        }


        public LTDescr SetCanvasMove()
        {
            type = TweenAction.CanvasMove;
            InitInternal = () => { fromInternal = rectTransform.anchoredPosition3D; };
            EaseInternal = () => { rectTransform.anchoredPosition3D = EaseMethod(); };
            return this;
        }

        public LTDescr SetCanvasMoveX()
        {
            type = TweenAction.CanvasMoveX;
            InitInternal = () => { fromInternal.x = rectTransform.anchoredPosition3D.x; };
            EaseInternal = () => { Vector3 c = rectTransform.anchoredPosition3D; rectTransform.anchoredPosition3D = new Vector3(EaseMethod().x, c.y, c.z); };
            return this;
        }

        public LTDescr SetCanvasMoveY()
        {
            this.type = TweenAction.CanvasMoveY;
            this.InitInternal = () => { this.fromInternal.x = this.rectTransform.anchoredPosition3D.y; };
            this.EaseInternal = () => { Vector3 c = this.rectTransform.anchoredPosition3D; this.rectTransform.anchoredPosition3D = new Vector3(c.x, EaseMethod().x, c.z); };
            return this;
        }

        public LTDescr SetCanvasMoveZ()
        {
            this.type = TweenAction.CanvasMoveZ;
            this.InitInternal = () => { this.fromInternal.x = this.rectTransform.anchoredPosition3D.z; };
            this.EaseInternal = () => { Vector3 c = this.rectTransform.anchoredPosition3D; this.rectTransform.anchoredPosition3D = new Vector3(c.x, c.y, EaseMethod().x); };
            return this;
        }

        public LTDescr SetCanvasMoveAdd()
        {
            type = TweenAction.CanvasMoveAdd;
            InitInternal = () =>
            {
                To += rectTransform.anchoredPosition3D;
                fromInternal = rectTransform.anchoredPosition3D;
            };
            EaseInternal = () => { rectTransform.anchoredPosition3D = EaseMethod(); };

            return this;
        }

        public LTDescr SetCanvasRotate()
        {
            this.type = TweenAction.CanvasRotate;
            InitInternal = () =>
            {
                From = rectTransform.eulerAngles;
            };
            EaseInternal = () =>
            {
                rectTransform.eulerAngles = EaseMethod();
            };
            return this;
        }

        public LTDescr SetCanvasRotateLocal()
        {
            type = TweenAction.CanvasRotateLocal;
            InitInternal = () =>
            {
                From = rectTransform.localEulerAngles;
            };
            EaseInternal = () =>
            {
                rectTransform.localEulerAngles = EaseMethod();
            };
            return this;
        }

        public LTDescr SetCanvasRotateAdd()
        {
            type = TweenAction.CanvasRotateAdd;
            InitInternal = () =>
            {
                To += rectTransform.localEulerAngles;
                From = rectTransform.localEulerAngles;
            };
            EaseInternal = () =>
            {
                rectTransform.localEulerAngles = EaseMethod();
            };

            return this;
        }

        public LTDescr SetCanvasPlaySprite()
        {
            type = TweenAction.CanvasPlaySprite;
            InitInternal = () =>
            {
                uiImage = trans.GetComponent<UnityEngine.UI.Image>();
                fromInternal.x = 0f;
            };
            this.EaseInternal = () =>
            {
                newVect = EaseMethod();
                val = newVect.x;
                int frame = (int)Mathf.Round(val);
                uiImage.sprite = sprites[frame];
            };
            return this;
        }

        public LTDescr SetCanvasScale()
        {
            this.type = TweenAction.CanvasScale;
            this.InitInternal = () => { this.From = this.rectTransform.localScale; };
            this.EaseInternal = () => { this.rectTransform.localScale = EaseMethod(); };
            return this;
        }

        public LTDescr SetCanvasSizeDelta()
        {
            this.type = TweenAction.CanvasSize;
            this.InitInternal = () => { this.From = this.rectTransform.sizeDelta; };
            this.EaseInternal = () => { this.rectTransform.sizeDelta = EaseMethod(); };
            return this;
        }

        public LTDescr SetCanvasSizeDeltaAdd()
        {
            type = TweenAction.CanvasSizeAdd;
            InitInternal = () =>
            {
                To = (Vector2)To + rectTransform.sizeDelta;
                From = rectTransform.sizeDelta;
            };
            EaseInternal = () =>
            {
                rectTransform.sizeDelta = EaseMethod();
            };
            return this;
        }

        private void callback() { newVect = EaseMethod(); val = newVect.x; }

        public LTDescr setCallback()
        {
            this.type = TweenAction.CallBack;
            this.InitInternal = () => { };
            this.EaseInternal = this.callback;
            return this;
        }
        public LTDescr setValue3()
        {
            this.type = TweenAction.Value3;
            this.InitInternal = () => { };
            this.EaseInternal = this.callback;
            return this;
        }

        public LTDescr setMove()
        {
            this.type = TweenAction.Move;
            this.InitInternal = () => { this.From = trans.position; };
            this.EaseInternal = () =>
            {
                newVect = EaseMethod();
                trans.position = newVect;
            };
            return this;
        }

        public LTDescr setMoveLocal()
        {
            this.type = TweenAction.MoveLocal;
            this.InitInternal = () => { this.From = trans.localPosition; };
            this.EaseInternal = () =>
            {
                newVect = EaseMethod();
                trans.localPosition = newVect;
            };
            return this;
        }

        public LTDescr setMoveToTransform()
        {
            this.type = TweenAction.MoveToTransform;
            this.InitInternal = () => { this.From = trans.position; };
            this.EaseInternal = () =>
            {
                this.To = this._optional.toTrans.position;
                this.diff = this.To - this.From;
                this.diffDiv2 = this.diff * 0.5f;

                newVect = EaseMethod();
                this.trans.position = newVect;
            };
            return this;
        }


        public LTDescr SetScale()
        {
            this.type = TweenAction.Scale;
            this.InitInternal = () => { this.From = trans.localScale; };
            this.EaseInternal = () =>
            {
                newVect = EaseMethod();
                trans.localScale = newVect;
            };
            return this;
        }

        public LTDescr SetScaleAdd()
        {
            type = TweenAction.ScaleAdd;
            InitInternal = () =>
            {
                To += trans.localScale;
                From = trans.localScale;
            };
            EaseInternal = () =>
            {
                trans.localScale = EaseMethod();
            };

            return this;
        }

        public LTDescr SetCanvasScaleAdd()
        {
            type = TweenAction.CanvasScaleAdd;
            InitInternal = () =>
            {
                To += rectTransform.localScale;
                From = rectTransform.localScale;
            };
            EaseInternal = () =>
            {
                rectTransform.localScale = EaseMethod();
            };

            return this;
        }

        public LTDescr setGUIMove()
        {
            this.type = TweenAction.GuiMove;
            this.InitInternal = () => { this.From = new Vector3(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, 0); };
            this.EaseInternal = () => { Vector3 v = EaseMethod(); this._optional.ltRect.rect = new Rect(v.x, v.y, this._optional.ltRect.rect.width, this._optional.ltRect.rect.height); };
            return this;
        }

        public LTDescr setGUIMoveMargin()
        {
            this.type = TweenAction.GuiMoveMargin;
            this.InitInternal = () => { this.From = new Vector2(this._optional.ltRect.margin.x, this._optional.ltRect.margin.y); };
            this.EaseInternal = () => { Vector3 v = EaseMethod(); this._optional.ltRect.margin = new Vector2(v.x, v.y); };
            return this;
        }

        public LTDescr setGUIScale()
        {
            this.type = TweenAction.GuiScale;
            this.InitInternal = () => { this.From = new Vector3(this._optional.ltRect.rect.width, this._optional.ltRect.rect.height, 0); };
            this.EaseInternal = () => { Vector3 v = EaseMethod(); this._optional.ltRect.rect = new Rect(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, v.x, v.y); };
            return this;
        }

        public LTDescr setGUIAlpha()
        {
            this.type = TweenAction.GuiAlpha;
            this.InitInternal = () => { this.fromInternal.x = this._optional.ltRect.alpha; };
            this.EaseInternal = () => { this._optional.ltRect.alpha = EaseMethod().x; };
            return this;
        }

        public LTDescr SetGUIRotate()
        {
            this.type = TweenAction.GuiRotate;
            this.InitInternal = () =>
            {
                if (this._optional.ltRect.rotateEnabled == false)
                {
                    this._optional.ltRect.rotateEnabled = true;
                    this._optional.ltRect.resetForRotation();
                }

                this.fromInternal.x = this._optional.ltRect.rotation;
            };
            this.EaseInternal = () => { this._optional.ltRect.rotation = EaseMethod().x; };
            return this;
        }

        public LTDescr SetDelayedSound()
        {
            type = TweenAction.DelayedSound;
            InitInternal = () => { hasExtraOnCompletes = true; };
            EaseInternal = callback;
            return this;
        }

        public LTDescr SetTarget(Transform trans)
        {
            optional.toTrans = trans;
            return this;
        }

        private void Init()
        {
            hasInitiliazed = true;


            usesNormalDt = !(useEstimatedTime || useManualTime || useFrames); // only set this to true if it uses non of the other timing modes

            if (useFrames)
                optional.initFrameCount = Time.frameCount;

            if (time <= 0f) // avoid dividing by zero
                time = Mathf.Epsilon;

            InitInternal?.Invoke(); // Invoke if initInternal != null

            diff = To - From;
            diffDiv2 = diff * 0.5f;

            _optional.onStart?.Invoke();

            if (onCompleteOnStart)
                CallOnCompletes();

            if (speed >= 0)
            {
                InitSpeed();
            }
        }

        private void InitSpeed()
        {
            if (type == TweenAction.MoveBezier || type == TweenAction.MoveBezierLocal)
            {
                time = _optional.path.distance / speed;
            }
            else if (type == TweenAction.MoveSpline || type == TweenAction.MoveSplineLocal)
            {
                time = _optional.spline.distance / speed;
            }
            else
            {
                time = (To - From).magnitude / speed;
            }
        }

        public static float val;
        public static float dt;
        public static Vector3 newVect;

        /**
        * If you need a tween to happen immediately instead of waiting for the next Update call, you can force it with this method
        * 
        * @method updateNow
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 0f ).updateNow();
        */
        public LTDescr UpdateNow()
        {
            UpdateInternal();
            return this;
        }

        public bool UpdateInternal()
        {
            float directionLocal = direction;
            if (usesNormalDt)
            {
                dt = LeanTween.dtActual;
            }
            else if (useEstimatedTime)
            {
                dt = LeanTween.dtEstimated;
            }
            else if (useFrames)
            {
                dt = optional.initFrameCount == 0 ? 0 : 1;
                optional.initFrameCount = Time.frameCount;
            }
            else if (useManualTime)
            {
                dt = LeanTween.dtManual;
            }

            //		Debug.Log ("tween:" + this+ " dt:"+dt);
            if (delay <= 0f && directionLocal != 0f)
            {
                if (trans == null)
                    return true;

                // initialize if has not done so yet
                if (!hasInitiliazed)
                    Init();

                dt *= directionLocal;
                passed += dt;

                passed = Mathf.Clamp(passed, 0f, time); // need to clamp when finished so it will finish at the exact spot and not overshoot

                ratioPassed = (passed / time);

                EaseInternal();

                if (hasUpdateCallback)
                    _optional.callOnUpdate(val, ratioPassed);

                bool isTweenFinished = directionLocal > 0f ? passed >= time : passed <= 0f;
                if (isTweenFinished)
                { // increment, flip or acumulate tween
                    if (_loopDelay <= 0)
                    {
                        loopCount--;
                        _loopDelay = loopCount > 1 ? loopDelay : 0;

                        if (loopType == LoopType.PingPong)
                        {
                            direction = 0.0f - directionLocal;
                        }
                        else
                        {
                            passed = Mathf.Epsilon;

                            if (loopType == LoopType.Add)
                            {
                                Vector3 previousFrom = From;
                                Vector3 previousTo = To;

                                From = previousTo;
                                To += previousTo - previousFrom;
                            }
                        }

                        isTweenFinished = loopCount == 0 || loopType == LoopType.Once; // only return true if it is fully complete

                        if (isTweenFinished == false && onCompleteOnRepeat && hasExtraOnCompletes)
                        {
                            CallOnCompletes(); // this only gets called if onCompleteOnRepeat is set to true, otherwise LeanTween class takes care of calling it
                        }

                        return isTweenFinished;
                    }
                    else
                    {
                        _loopDelay -= Mathf.Abs(dt);
                    }
                }
            }
            else
            {
                delay -= dt;
            }

            return false;
        }

        public void CallOnCompletes()
        {
            if (type == TweenAction.GuiRotate)
                _optional.ltRect.rotateFinished = true;

            if (type == TweenAction.DelayedSound)
            {
                AudioSource.PlayClipAtPoint((AudioClip)_optional.onCompleteParam, To, From.x);
            }
            if (_optional.onComplete != null)
            {
                _optional.onComplete();
            }
            else
            {
                _optional.onCompleteObject?.Invoke(_optional.onCompleteParam);
            }
        }

        // Helper Methods

        public LTDescr SetFromColor(Color col)
        {
            this.From = new Vector3(0.0f, col.a, 0.0f);
            this.diff = new Vector3(1.0f, 0.0f, 0.0f);
            this._optional.axis = new Vector3(col.r, col.g, col.b);
            return this;
        }

        private static void AlphaRecursive(Transform transform, float val, bool useRecursion = true)
        {
            Renderer renderer = transform.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, val);
                    }
                    else if (mat.HasProperty("_TintColor"))
                    {
                        Color col = mat.GetColor("_TintColor");
                        mat.SetColor("_TintColor", new Color(col.r, col.g, col.b, val));
                    }
                }
            }
            if (useRecursion && transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    AlphaRecursive(child, val);
                }
            }
        }

        private static void ColorRecursive(Transform transform, Color toColor, bool useRecursion = true)
        {
            Renderer ren = transform.gameObject.GetComponent<Renderer>();
            if (ren != null)
            {
                foreach (Material mat in ren.materials)
                {
                    mat.color = toColor;
                }
            }
            if (useRecursion && transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    ColorRecursive(child, toColor);
                }
            }
        }


        private static void AlphaRecursive(RectTransform rectTransform, float val, int recursiveLevel = 0)
        {
            if (rectTransform.childCount > 0)
            {
                foreach (RectTransform child in rectTransform)
                {
                    UnityEngine.UI.MaskableGraphic uiImage = child.GetComponent<UnityEngine.UI.Image>();
                    if (uiImage != null)
                    {
                        Color c = uiImage.color; c.a = val; uiImage.color = c;
                    }
                    else
                    {
                        uiImage = child.GetComponent<UnityEngine.UI.RawImage>();
                        if (uiImage != null)
                        {
                            Color c = uiImage.color; c.a = val; uiImage.color = c;
                        }
                    }

                    AlphaRecursive(child, val, recursiveLevel + 1);
                }
            }
        }

        private static void AlphaRecursiveSprite(Transform transform, float val)
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    SpriteRenderer ren = child.GetComponent<SpriteRenderer>();
                    if (ren != null)
                        ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, val);
                    AlphaRecursiveSprite(child, val);
                }
            }
        }

        private static void ColorRecursiveSprite(Transform transform, Color toColor)
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    SpriteRenderer ren = transform.gameObject.GetComponent<SpriteRenderer>();
                    if (ren != null)
                        ren.color = toColor;
                    ColorRecursiveSprite(child, toColor);
                }
            }
        }

        private static void ColorRecursive(RectTransform rectTransform, Color toColor)
        {

            if (rectTransform.childCount > 0)
            {
                foreach (RectTransform child in rectTransform)
                {
                    UnityEngine.UI.MaskableGraphic uiImage = child.GetComponent<UnityEngine.UI.Image>();
                    if (uiImage != null)
                    {
                        uiImage.color = toColor;
                    }
                    else
                    {
                        uiImage = child.GetComponent<UnityEngine.UI.RawImage>();
                        if (uiImage != null)
                            uiImage.color = toColor;
                    }
                    ColorRecursive(child, toColor);
                }
            }
        }

        private static void TextAlphaChildrenRecursive(Transform trans, float val, bool useRecursion = true)
        {

            if (useRecursion && trans.childCount > 0)
            {
                foreach (Transform child in trans)
                {
                    UnityEngine.UI.Text uiText = child.GetComponent<UnityEngine.UI.Text>();
                    if (uiText != null)
                    {
                        Color c = uiText.color;
                        c.a = val;
                        uiText.color = c;
                    }
                    TextAlphaChildrenRecursive(child, val);
                }
            }
        }

        private static void TextAlphaRecursive(Transform trans, float val, bool useRecursion = false)
        {
            UnityEngine.UI.Text uiText = trans.GetComponent<UnityEngine.UI.Text>();
            if (uiText != null)
            {
                Color c = uiText.color;
                c.a = val;
                uiText.color = c;
            }
            if (useRecursion && trans.childCount > 0)
            {
                foreach (Transform child in trans)
                {
                    TextAlphaRecursive(child, val);
                }
            }
        }

        private static void TextColorRecursive(Transform trans, Color toColor)
        {
            if (trans.childCount > 0)
            {
                foreach (Transform child in trans)
                {
                    Text uiText = child.GetComponent<Text>();
                    TextMeshProUGUI tmText = child.GetComponent<TextMeshProUGUI>();
                    if (uiText != null)
                    {
                        uiText.color = toColor;
                    }
                    else if (tmText != null)
                    {
                        tmText.color = toColor;
                    }
                    TextColorRecursive(child, toColor);
                }
            }
        }

        private static Color TweenColor(LTDescr tween, float val)
        {
            Vector3 diff3 = tween._optional.point - tween._optional.axis;
            float diffAlpha = tween.To.y - tween.From.y;
            return new Color(tween._optional.axis.x + diff3.x * val, tween._optional.axis.y + diff3.y * val, tween._optional.axis.z + diff3.z * val, tween.From.y + diffAlpha * val);
        }

        /**
        * Pause a tween
        * 
        * @method pause
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        */
        public LTDescr pause()
        {
            if (this.direction != 0.0f)
            { // check if tween is already paused
                this.directionLast = this.direction;
                this.direction = 0.0f;
            }

            return this;
        }

        /**
        * Resume a paused tween
        * 
        * @method resume
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        */
        public LTDescr resume()
        {
            this.direction = this.directionLast;

            return this;
        }

        /**
        * Set Axis optional axis for tweens where it is relevant
        * 
        * @method setAxis
        * @param {Vector3} axis either the tween rotates around, or the direction it faces in the case of setOrientToPath
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.move( ltLogo, path, 1.0f ).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setAxis(Vector3.forward);
        */
        public LTDescr setAxis(Vector3 axis)
        {
            this._optional.axis = axis;
            return this;
        }

        /**
        * Delay the start of a tween
        * 
        * @method setDelay
        * @param {float} float time The time to complete the tween in
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setDelay( 1.5f );
        */
        public LTDescr SetDelay(float delay)
        {
            this.delay = delay;

            return this;
        }

        public LTDescr SetLoopDelay(float loopDelay)
        {
            this.loopDelay = loopDelay;
            _loopDelay = loopDelay;

            return this;
        }

        /**
        * Set the type of easing used for the tween. <br>
        * <ul><li><a href="LeanTweenType.html">List of all the ease types</a>.</li>
        * <li><a href="http://www.robertpenner.com/easing/easing_demo.html">This page helps visualize the different easing equations</a></li>
        * </ul>
        * 
        * @method setEase
        * @param {LeanTweenType} easeType:LeanTweenType the easing type to use
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setEase( LeanTweenType.easeInBounce );
        */
        public LTDescr SetEase(LeanTweenType easeType)
        {

            switch (easeType)
            {
                case LeanTweenType.Linear:
                    setEaseLinear(); break;
                case LeanTweenType.EaseOutQuad:
                    setEaseOutQuad(); break;
                case LeanTweenType.EaseInQuad:
                    setEaseInQuad(); break;
                case LeanTweenType.EaseInOutQuad:
                    setEaseInOutQuad(); break;
                case LeanTweenType.EaseInCubic:
                    setEaseInCubic(); break;
                case LeanTweenType.EaseOutCubic:
                    setEaseOutCubic(); break;
                case LeanTweenType.EaseInOutCubic:
                    setEaseInOutCubic(); break;
                case LeanTweenType.EaseInQuart:
                    setEaseInQuart(); break;
                case LeanTweenType.EaseOutQuart:
                    setEaseOutQuart(); break;
                case LeanTweenType.EaseInOutQuart:
                    setEaseInOutQuart(); break;
                case LeanTweenType.EaseInQuint:
                    setEaseInQuint(); break;
                case LeanTweenType.EaseOutQuint:
                    setEaseOutQuint(); break;
                case LeanTweenType.EaseInOutQuint:
                    setEaseInOutQuint(); break;
                case LeanTweenType.EaseInSine:
                    setEaseInSine(); break;
                case LeanTweenType.EaseOutSine:
                    setEaseOutSine(); break;
                case LeanTweenType.EaseInOutSine:
                    setEaseInOutSine(); break;
                case LeanTweenType.EaseInExpo:
                    setEaseInExpo(); break;
                case LeanTweenType.EaseOutExpo:
                    setEaseOutExpo(); break;
                case LeanTweenType.EaseInOutExpo:
                    setEaseInOutExpo(); break;
                case LeanTweenType.EaseInCirc:
                    setEaseInCirc(); break;
                case LeanTweenType.EaseOutCirc:
                    setEaseOutCirc(); break;
                case LeanTweenType.EaseInOutCirc:
                    setEaseInOutCirc(); break;
                case LeanTweenType.EaseInBounce:
                    setEaseInBounce(); break;
                case LeanTweenType.EaseOutBounce:
                    setEaseOutBounce(); break;
                case LeanTweenType.EaseInOutBounce:
                    setEaseInOutBounce(); break;
                case LeanTweenType.EaseInBack:
                    setEaseInBack(); break;
                case LeanTweenType.EaseOutBack:
                    setEaseOutBack(); break;
                case LeanTweenType.EaseInOutBack:
                    setEaseInOutBack(); break;
                case LeanTweenType.EaseInElastic:
                    setEaseInElastic(); break;
                case LeanTweenType.EaseOutElastic:
                    setEaseOutElastic(); break;
                case LeanTweenType.EaseInOutElastic:
                    setEaseInOutElastic(); break;
                case LeanTweenType.Punch:
                    setEasePunch(); break;
                case LeanTweenType.EaseShake:
                    setEaseShake(); break;
                case LeanTweenType.EaseSpring:
                    setEaseSpring(); break;
                default:
                    setEaseLinear(); break;
            }

            return this;
        }

        public LTDescr setEaseLinear() { this.easeType = LeanTweenType.Linear; this.EaseMethod = this.EaseLinear; return this; }

        public LTDescr setEaseSpring() { this.easeType = LeanTweenType.EaseSpring; this.EaseMethod = this.easeSpring; return this; }

        public LTDescr setEaseInQuad() { this.easeType = LeanTweenType.EaseInQuad; this.EaseMethod = this.easeInQuad; return this; }

        public LTDescr setEaseOutQuad() { this.easeType = LeanTweenType.EaseOutQuad; this.EaseMethod = this.easeOutQuad; return this; }

        public LTDescr setEaseInOutQuad() { this.easeType = LeanTweenType.EaseInOutQuad; this.EaseMethod = this.easeInOutQuad; return this; }

        public LTDescr setEaseInCubic() { this.easeType = LeanTweenType.EaseInCubic; this.EaseMethod = this.easeInCubic; return this; }

        public LTDescr setEaseOutCubic() { this.easeType = LeanTweenType.EaseOutCubic; this.EaseMethod = this.easeOutCubic; return this; }

        public LTDescr setEaseInOutCubic() { this.easeType = LeanTweenType.EaseInOutCubic; this.EaseMethod = this.easeInOutCubic; return this; }

        public LTDescr setEaseInQuart() { this.easeType = LeanTweenType.EaseInQuart; this.EaseMethod = this.easeInQuart; return this; }

        public LTDescr setEaseOutQuart() { this.easeType = LeanTweenType.EaseOutQuart; this.EaseMethod = this.easeOutQuart; return this; }

        public LTDescr setEaseInOutQuart() { this.easeType = LeanTweenType.EaseInOutQuart; this.EaseMethod = this.easeInOutQuart; return this; }

        public LTDescr setEaseInQuint() { this.easeType = LeanTweenType.EaseInQuint; this.EaseMethod = this.easeInQuint; return this; }

        public LTDescr setEaseOutQuint() { this.easeType = LeanTweenType.EaseOutQuint; this.EaseMethod = this.easeOutQuint; return this; }

        public LTDescr setEaseInOutQuint() { this.easeType = LeanTweenType.EaseInOutQuint; this.EaseMethod = this.easeInOutQuint; return this; }

        public LTDescr setEaseInSine() { this.easeType = LeanTweenType.EaseInSine; this.EaseMethod = this.easeInSine; return this; }

        public LTDescr setEaseOutSine() { this.easeType = LeanTweenType.EaseOutSine; this.EaseMethod = this.easeOutSine; return this; }

        public LTDescr setEaseInOutSine() { this.easeType = LeanTweenType.EaseInOutSine; this.EaseMethod = this.easeInOutSine; return this; }

        public LTDescr setEaseInExpo() { this.easeType = LeanTweenType.EaseInExpo; this.EaseMethod = this.easeInExpo; return this; }

        public LTDescr setEaseOutExpo() { this.easeType = LeanTweenType.EaseOutExpo; this.EaseMethod = this.easeOutExpo; return this; }

        public LTDescr setEaseInOutExpo() { this.easeType = LeanTweenType.EaseInOutExpo; this.EaseMethod = this.easeInOutExpo; return this; }

        public LTDescr setEaseInCirc() { this.easeType = LeanTweenType.EaseInCirc; this.EaseMethod = this.easeInCirc; return this; }

        public LTDescr setEaseOutCirc() { this.easeType = LeanTweenType.EaseOutCirc; this.EaseMethod = this.easeOutCirc; return this; }

        public LTDescr setEaseInOutCirc() { this.easeType = LeanTweenType.EaseInOutCirc; this.EaseMethod = this.easeInOutCirc; return this; }

        public LTDescr setEaseInBounce() { this.easeType = LeanTweenType.EaseInBounce; this.EaseMethod = this.easeInBounce; return this; }

        public LTDescr setEaseOutBounce() { this.easeType = LeanTweenType.EaseOutBounce; this.EaseMethod = this.easeOutBounce; return this; }

        public LTDescr setEaseInOutBounce() { this.easeType = LeanTweenType.EaseInOutBounce; this.EaseMethod = this.easeInOutBounce; return this; }

        public LTDescr setEaseInBack() { this.easeType = LeanTweenType.EaseInBack; this.EaseMethod = this.easeInBack; return this; }

        public LTDescr setEaseOutBack() { this.easeType = LeanTweenType.EaseOutBack; this.EaseMethod = this.easeOutBack; return this; }

        public LTDescr setEaseInOutBack() { this.easeType = LeanTweenType.EaseInOutBack; this.EaseMethod = this.easeInOutBack; return this; }

        public LTDescr setEaseInElastic() { this.easeType = LeanTweenType.EaseInElastic; this.EaseMethod = this.easeInElastic; return this; }

        public LTDescr setEaseOutElastic() { this.easeType = LeanTweenType.EaseOutElastic; this.EaseMethod = this.easeOutElastic; return this; }

        public LTDescr setEaseInOutElastic() { this.easeType = LeanTweenType.EaseInOutElastic; this.EaseMethod = this.easeInOutElastic; return this; }

        public LTDescr setEasePunch() { this._optional.animationCurve = LeanTween.punch; this.toInternal.x = this.From.x + this.To.x; this.EaseMethod = this.tweenOnCurve; return this; }

        public LTDescr setEaseShake() { this._optional.animationCurve = LeanTween.shake; this.toInternal.x = this.From.x + this.To.x; this.EaseMethod = this.tweenOnCurve; return this; }

        private Vector3 tweenOnCurve()
        {
            return new Vector3(this.From.x + (this.diff.x) * this._optional.animationCurve.Evaluate(ratioPassed),
                this.From.y + (this.diff.y) * this._optional.animationCurve.Evaluate(ratioPassed),
                this.From.z + (this.diff.z) * this._optional.animationCurve.Evaluate(ratioPassed));
        }

        // Vector3 Ease Methods

        private Vector3 easeInOutQuad()
        {
            val = this.ratioPassed * 2f;

            if (val < 1f)
            {
                val = val * val;
                return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
            }
            val = (1f - val) * (val - 3f) + 1f;
            return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
        }

        private Vector3 easeInQuad()
        {
            val = ratioPassed * ratioPassed;
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeOutQuad()
        {
            val = this.ratioPassed;
            val = -val * (val - 2f);
            return (this.diff * val + this.From);
        }

        private Vector3 EaseLinear()
        {
            val = this.ratioPassed;
            return new Vector3(this.From.x + this.diff.x * val, this.From.y + this.diff.y * val, this.From.z + this.diff.z * val);
        }

        private Vector3 easeSpring()
        {
            val = Mathf.Clamp01(this.ratioPassed);
            val = (Mathf.Sin(val * Mathf.PI * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + (1.2f * (1f - val)));
            return this.From + this.diff * val;
        }

        private Vector3 easeInCubic()
        {
            val = this.ratioPassed * this.ratioPassed * this.ratioPassed;
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeOutCubic()
        {
            val = this.ratioPassed - 1f;
            val = (val * val * val + 1);
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeInOutCubic()
        {
            val = this.ratioPassed * 2f;
            if (val < 1f)
            {
                val = val * val * val;
                return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
            }
            val -= 2f;
            val = val * val * val + 2f;
            return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
        }

        private Vector3 easeInQuart()
        {
            val = this.ratioPassed * this.ratioPassed * this.ratioPassed * this.ratioPassed;
            return diff * val + this.From;
        }

        private Vector3 easeOutQuart()
        {
            val = this.ratioPassed - 1f;
            val = -(val * val * val * val - 1);
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeInOutQuart()
        {
            val = this.ratioPassed * 2f;
            if (val < 1f)
            {
                val = val * val * val * val;
                return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
            }
            val -= 2f;
            //		val = (val * val * val * val - 2f);
            return -this.diffDiv2 * (val * val * val * val - 2f) + this.From;
        }

        private Vector3 easeInQuint()
        {
            val = this.ratioPassed;
            val = val * val * val * val * val;
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeOutQuint()
        {
            val = this.ratioPassed - 1f;
            val = (val * val * val * val * val + 1f);
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeInOutQuint()
        {
            val = this.ratioPassed * 2f;
            if (val < 1f)
            {
                val = val * val * val * val * val;
                return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
            }
            val -= 2f;
            val = (val * val * val * val * val + 2f);
            return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
        }

        private Vector3 easeInSine()
        {
            val = -Mathf.Cos(this.ratioPassed * LeanTween.PI_DIV2);
            return new Vector3(this.diff.x * val + this.diff.x + this.From.x, this.diff.y * val + this.diff.y + this.From.y, this.diff.z * val + this.diff.z + this.From.z);
        }

        private Vector3 easeOutSine()
        {
            val = Mathf.Sin(this.ratioPassed * LeanTween.PI_DIV2);
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeInOutSine()
        {
            val = -(Mathf.Cos(Mathf.PI * this.ratioPassed) - 1f);
            return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
        }

        private Vector3 easeInExpo()
        {
            val = Mathf.Pow(2f, 10f * (this.ratioPassed - 1f));
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeOutExpo()
        {
            val = (-Mathf.Pow(2f, -10f * this.ratioPassed) + 1f);
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeInOutExpo()
        {
            val = this.ratioPassed * 2f;
            if (val < 1) return this.diffDiv2 * Mathf.Pow(2, 10 * (val - 1)) + this.From;
            val--;
            return this.diffDiv2 * (-Mathf.Pow(2, -10 * val) + 2) + this.From;
        }

        private Vector3 easeInCirc()
        {
            val = -(Mathf.Sqrt(1f - this.ratioPassed * this.ratioPassed) - 1f);
            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeOutCirc()
        {
            val = this.ratioPassed - 1f;
            val = Mathf.Sqrt(1f - val * val);

            return new Vector3(this.diff.x * val + this.From.x, this.diff.y * val + this.From.y, this.diff.z * val + this.From.z);
        }

        private Vector3 easeInOutCirc()
        {
            val = this.ratioPassed * 2f;
            if (val < 1f)
            {
                val = -(Mathf.Sqrt(1f - val * val) - 1f);
                return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
            }
            val -= 2f;
            val = (Mathf.Sqrt(1f - val * val) + 1f);
            return new Vector3(this.diffDiv2.x * val + this.From.x, this.diffDiv2.y * val + this.From.y, this.diffDiv2.z * val + this.From.z);
        }

        private Vector3 easeInBounce()
        {
            val = this.ratioPassed;
            val = 1f - val;
            return new Vector3(this.diff.x - LeanTween.easeOutBounce(0, this.diff.x, val) + this.From.x,
                this.diff.y - LeanTween.easeOutBounce(0, this.diff.y, val) + this.From.y,
                this.diff.z - LeanTween.easeOutBounce(0, this.diff.z, val) + this.From.z);
        }

        private Vector3 easeOutBounce()
        {
            val = ratioPassed;
            float valM, valN; // bounce values
            if (val < (valM = 1 - 1.75f * this.overshoot / 2.75f))
            {
                val = 1 / valM / valM * val * val;
            }
            else if (val < (valN = 1 - .75f * this.overshoot / 2.75f))
            {
                val -= (valM + valN) / 2;
                // first bounce, height: 1/4
                val = 7.5625f * val * val + 1 - .25f * this.overshoot * this.overshoot;
            }
            else if (val < (valM = 1 - .25f * this.overshoot / 2.75f))
            {
                val -= (valM + valN) / 2;
                // second bounce, height: 1/16
                val = 7.5625f * val * val + 1 - .0625f * this.overshoot * this.overshoot;
            }
            else
            { // valN = 1
                val -= (valM + 1) / 2;
                // third bounce, height: 1/64
                val = 7.5625f * val * val + 1 - .015625f * this.overshoot * this.overshoot;
            }
            return this.diff * val + this.From;
        }

        private Vector3 easeInOutBounce()
        {
            val = this.ratioPassed * 2f;
            if (val < 1f)
            {
                return new Vector3(LeanTween.easeInBounce(0, this.diff.x, val) * 0.5f + this.From.x,
                    LeanTween.easeInBounce(0, this.diff.y, val) * 0.5f + this.From.y,
                    LeanTween.easeInBounce(0, this.diff.z, val) * 0.5f + this.From.z);
            }
            else
            {
                val = val - 1f;
                return new Vector3(LeanTween.easeOutBounce(0, this.diff.x, val) * 0.5f + this.diffDiv2.x + this.From.x,
                    LeanTween.easeOutBounce(0, this.diff.y, val) * 0.5f + this.diffDiv2.y + this.From.y,
                    LeanTween.easeOutBounce(0, this.diff.z, val) * 0.5f + this.diffDiv2.z + this.From.z);
            }
        }

        private Vector3 easeInBack()
        {
            val = this.ratioPassed;
            val /= 1;
            float s = 1.70158f * this.overshoot;
            return this.diff * (val) * val * ((s + 1) * val - s) + this.From;
        }

        private Vector3 easeOutBack()
        {
            float s = 1.70158f * this.overshoot;
            val = (this.ratioPassed / 1) - 1;
            val = ((val) * val * ((s + 1) * val + s) + 1);
            return this.diff * val + this.From;
        }

        private Vector3 easeInOutBack()
        {
            float s = 1.70158f * this.overshoot;
            val = this.ratioPassed * 2f;
            if ((val) < 1)
            {
                s *= (1.525f) * overshoot;
                return this.diffDiv2 * (val * val * (((s) + 1) * val - s)) + this.From;
            }
            val -= 2;
            s *= (1.525f) * overshoot;
            val = ((val) * val * (((s) + 1) * val + s) + 2);
            return this.diffDiv2 * val + this.From;
        }

        private Vector3 easeInElastic()
        {
            return new Vector3(LeanTween.easeInElastic(this.From.x, this.To.x, this.ratioPassed, this.overshoot, this.period),
                LeanTween.easeInElastic(this.From.y, this.To.y, this.ratioPassed, this.overshoot, this.period),
                LeanTween.easeInElastic(this.From.z, this.To.z, this.ratioPassed, this.overshoot, this.period));
        }

        private Vector3 easeOutElastic()
        {
            return new Vector3(LeanTween.easeOutElastic(this.From.x, this.To.x, this.ratioPassed, this.overshoot, this.period),
                LeanTween.easeOutElastic(this.From.y, this.To.y, this.ratioPassed, this.overshoot, this.period),
                LeanTween.easeOutElastic(this.From.z, this.To.z, this.ratioPassed, this.overshoot, this.period));
        }

        private Vector3 easeInOutElastic()
        {
            return new Vector3(LeanTween.easeInOutElastic(this.From.x, this.To.x, this.ratioPassed, this.overshoot, this.period),
                LeanTween.easeInOutElastic(this.From.y, this.To.y, this.ratioPassed, this.overshoot, this.period),
                LeanTween.easeInOutElastic(this.From.z, this.To.z, this.ratioPassed, this.overshoot, this.period));
        }

        /**
        * Set how far past a tween will overshoot  for certain ease types (compatible:  easeInBack, easeInOutBack, easeOutBack, easeOutElastic, easeInElastic, easeInOutElastic). <br>
        * @method setOvershoot
        * @param {float} overshoot:float how far past the destination it will go before settling in
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setEase( LeanTweenType.easeOutBack ).setOvershoot(2f);
        */
        public LTDescr setOvershoot(float overshoot)
        {
            this.overshoot = overshoot;
            return this;
        }

        /**
        * Set how short the iterations are for certain ease types (compatible: easeOutElastic, easeInElastic, easeInOutElastic). <br>
        * @method setPeriod
        * @param {float} period:float how short the iterations are that the tween will animate at (default 0.3f)
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setEase( LeanTweenType.easeOutElastic ).setPeriod(0.3f);
        */
        public LTDescr setPeriod(float period)
        {
            this.period = period;
            return this;
        }

        /**
        * Set how large the effect is for certain ease types (compatible: punch, shake, animation curves). <br>
        * @method setScale
        * @param {float} scale:float how much the ease will be multiplied by (default 1f)
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setEase( LeanTweenType.punch ).setScale(2f);
        */
        public LTDescr setScale(float scale)
        {
            this.scale = scale;
            return this;
        }

        /**
        * Set the type of easing used for the tween with a custom curve. <br>
        * @method setEase (AnimationCurve)
        * @param {AnimationCurve} easeDefinition:AnimationCurve an <a href="http://docs.unity3d.com/Documentation/ScriptReference/AnimationCurve.html" target="_blank">AnimationCure</a> that describes the type of easing you want, this is great for when you want a unique type of movement
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setEase( LeanTweenType.easeInBounce );
        */
        public LTDescr SetEase(AnimationCurve easeCurve)
        {
            this._optional.animationCurve = easeCurve;
            this.EaseMethod = this.tweenOnCurve;
            this.easeType = LeanTweenType.AnimationCurve;
            return this;
        }

        /**
        * Set the end that the GameObject is tweening towards
        * @method setTo
        * @param {Vector3} to:Vector3 point at which you want the tween to reach
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LTDescr descr = LeanTween.move( cube, Vector3.up, new Vector3(1f,3f,0f), 1.0f ).setEase( LeanTweenType.easeInOutBounce );<br>
        * // Later your want to change your destination or your destiation is constantly moving<br>
        * descr.setTo( new Vector3(5f,10f,3f) );<br>
        */
        public LTDescr setTo(Vector3 to)
        {
            if (this.hasInitiliazed)
            {
                this.To = to;
                this.diff = to - this.From;
            }
            else
            {
                this.To = to;
            }

            return this;
        }

        public LTDescr setTo(Transform to)
        {
            this._optional.toTrans = to;
            return this;
        }

        /**
        * Set the beginning of the tween
        * @method setFrom
        * @param {Vector3} from:Vector3 the point you would like the tween to start at
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LTDescr descr = LeanTween.move( cube, Vector3.up, new Vector3(1f,3f,0f), 1.0f ).setFrom( new Vector3(5f,10f,3f) );<br>
        */
        public LTDescr SetFrom(Vector3 from)
        {
            if (this.trans)
            {
                this.Init();
            }
            this.From = from;
            // this.hasInitiliazed = true; // this is set, so that the "from" value isn't overwritten later on when the tween starts
            this.diff = this.To - this.From;
            this.diffDiv2 = this.diff * 0.5f;
            return this;
        }

        public LTDescr setFrom(float from)
        {
            return SetFrom(new Vector3(from, 0f, 0f));
        }

        public LTDescr setDiff(Vector3 diff)
        {
            this.diff = diff;
            return this;
        }

        public LTDescr setHasInitialized(bool has)
        {
            this.hasInitiliazed = has;
            return this;
        }

        public LTDescr setId(uint id, uint global_counter)
        {
            this._id = id;
            this.counter = global_counter;
            // Debug.Log("Global counter:"+global_counter);
            return this;
        }

        /**
        * Set the point of time the tween will start in
        * @method setPassed
        * @param {float} passedTime:float the length of time in seconds the tween will start in
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * int tweenId = LeanTween.moveX(gameObject, 5f, 2.0f ).id;<br>
        * // Later<br>
        * LTDescr descr = description( tweenId );<br>
        * descr.setPassed( 1f );<br>
        */
        public LTDescr setPassed(float passed)
        {
            this.passed = passed;
            return this;
        }

        /**
        * Set the finish time of the tween
        * @method setTime
        * @param {float} finishTime:float the length of time in seconds you wish the tween to complete in
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * int tweenId = LeanTween.moveX(gameObject, 5f, 2.0f ).id;<br>
        * // Later<br>
        * LTDescr descr = description( tweenId );<br>
        * descr.setTime( 1f );<br>
        */
        public LTDescr setTime(float time)
        {
            float passedTimeRatio = this.passed / this.time;
            this.passed = time * passedTimeRatio;
            this.time = time;
            return this;
        }

        /**
        * Set the finish time of the tween
        * @method setSpeed
        * @param {float} speed:float the speed in unity units per second you wish the object to travel (overrides the given time)
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveLocalZ( gameObject, 10f, 1f).setSpeed(0.2f) // the given time is ignored when speed is set<br>
        */
        public LTDescr SetSpeed(float speed)
        {
            this.speed = speed;
            if (this.hasInitiliazed)
                InitSpeed();
            return this;
        }

        /**
        * Set the tween to repeat a number of times.
        * @method setRepeat
        * @param {int} repeatNum:int the number of times to repeat the tween. -1 to repeat infinite times
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setRepeat( 10 ).setLoopPingPong();
        */
        public LTDescr SetRepeat(int repeat)
        {
            this.loopCount = repeat;
            if ((repeat > 1 && this.loopType == LoopType.Once) || (repeat < 0 && this.loopType == LoopType.Once))
            {
                this.loopType = LoopType.Clamp;
            }
            if (this.type == TweenAction.CallBack || this.type == TweenAction.CallBackColor)
            {
                this.setOnCompleteOnRepeat(true);
            }
            return this;
        }

        public LTDescr SetLoopType(LoopType loopType)
        {
            this.loopType = loopType;
            return this;
        }

        public LTDescr setUseEstimatedTime(bool useEstimatedTime)
        {
            this.useEstimatedTime = useEstimatedTime;
            this.usesNormalDt = false;
            return this;
        }

        /**
        * Set ignore time scale when tweening an object when you want the animation to be time-scale independent (ignores the Time.timeScale value). Great for pause screens, when you want all other action to be stopped (or slowed down)
        * @method setIgnoreTimeScale
        * @param {bool} useUnScaledTime:bool whether to use the unscaled time or not
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setRepeat( 2 ).setIgnoreTimeScale( true );
        */
        public LTDescr setIgnoreTimeScale(bool useUnScaledTime)
        {
            this.useEstimatedTime = useUnScaledTime;
            this.usesNormalDt = false;
            return this;
        }

        /**
        * Use frames when tweening an object, when you don't want the animation to be time-frame independent...
        * @method setUseFrames
        * @param {bool} useFrames:bool whether to use estimated time or not
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setRepeat( 2 ).setUseFrames( true );
        */
        public LTDescr SetUseFrames(bool useFrames)
        {
            this.useFrames = useFrames;
            this.usesNormalDt = false;
            return this;
        }

        public LTDescr SetUseManualTime(bool useManualTime)
        {
            this.useManualTime = useManualTime;
            this.usesNormalDt = false;
            return this;
        }

        public LTDescr SetLoopCount(int loopCount)
        {
            this.loopType = LoopType.Clamp;
            this.loopCount = loopCount;
            return this;
        }

        /**
        * No looping involved, just run once (the default)
        * @method setLoopOnce
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setLoopOnce();
        */
        public LTDescr SetLoopOnce() { this.loopType = LoopType.Once; return this; }

        /**
        * When the animation gets to the end it starts back at where it began
        * @method setLoopClamp
        * @param {int} loops:int (defaults to -1) how many times you want the loop to happen (-1 for an infinite number of times)
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setLoopClamp( 2 );
        */
        public LTDescr SetLoopClamp()
        {
            this.loopType = LoopType.Clamp;
            if (this.loopCount == 0)
                this.loopCount = -1;
            return this;
        }
        public LTDescr SetLoopClamp(int loops)
        {
            this.loopCount = loops;
            return this;
        }

        /**
        * When the animation gets to the end it then tweens back to where it started (and on, and on)
        * @method setLoopPingPong
        * @param {int} loops:int (defaults to -1) how many times you want the loop to happen in both directions (-1 for an infinite number of times). Passing a value of 1 will cause the object to go towards and back from it's destination once.
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setLoopPingPong( 2 );
        */
        public LTDescr SetLoopPingPong()
        {
            loopType = LoopType.PingPong;
            if (loopCount == 0)
                loopCount = -1;
            return this;
        }

        public LTDescr SetLoopPingPong(int loops)
        {
            loopType = LoopType.PingPong;
            loopCount = loops == -1 ? loops : loops * 2;
            return this;
        }

        public LTDescr SetLoopAdd()
        {
            loopType = LoopType.Add;
            if (loopCount == 0)
                loopCount = -1;

            return this;
        }

        /**
        * Have a method called when the tween finishes
        * @method setOnComplete
        * @param {Action} onComplete:Action the method that should be called when the tween is finished ex: tweenFinished(){ }
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnComplete( tweenFinished );
        */
        public LTDescr SetOnComplete(Action onComplete)
        {
            this._optional.onComplete = onComplete;
            this.hasExtraOnCompletes = true;
            return this;
        }

        /**
        * Have a method called when the tween finishes
        * @method setOnComplete (object)
        * @param {Action<object>} onComplete:Action<object> the method that should be called when the tween is finished ex: tweenFinished( object myObj ){ }
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * object tweenFinishedObj = "hi" as object;
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnComplete( tweenFinished, tweenFinishedObj );
        */
        public LTDescr SetOnComplete(Action<object> onComplete)
        {
            this._optional.onCompleteObject = onComplete;
            this.hasExtraOnCompletes = true;
            return this;
        }
        public LTDescr SetOnComplete(Action<object> onComplete, object onCompleteParam)
        {
            this._optional.onCompleteObject = onComplete;
            this.hasExtraOnCompletes = true;
            if (onCompleteParam != null)
                this._optional.onCompleteParam = onCompleteParam;
            return this;
        }

        /**
        * Pass an object to along with the onComplete Function
        * @method setOnCompleteParam
        * @param {object} onComplete:object an object that 
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.delayedCall(1.5f, enterMiniGameStart).setOnCompleteParam( new object[]{""+5} );<br><br>
        * void enterMiniGameStart( object val ){<br>
        * &nbsp;object[] arr = (object [])val;<br>
        * &nbsp;int lvl = int.Parse((string)arr[0]);<br>
        * }<br>
        */
        public LTDescr SetOnCompleteParam(object onCompleteParam)
        {
            this._optional.onCompleteParam = onCompleteParam;
            this.hasExtraOnCompletes = true;
            return this;
        }


        /**
        * Have a method called on each frame that the tween is being animated (passes a float value)
        * @method setOnUpdate
        * @param {Action<float>} onUpdate:Action<float> a method that will be called on every frame with the float value of the tweened object
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnUpdate( tweenMoved );<br>
        * <br>
        * void tweenMoved( float val ){ }<br>
        */
        public LTDescr setOnUpdate(Action<float> onUpdate)
        {
            this._optional.onUpdateFloat = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }
        public LTDescr setOnUpdateRatio(Action<float, float> onUpdate)
        {
            this._optional.onUpdateFloatRatio = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }

        public LTDescr setOnUpdateObject(Action<float, object> onUpdate)
        {
            this._optional.onUpdateFloatObject = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }
        public LTDescr setOnUpdateVector2(Action<Vector2> onUpdate)
        {
            this._optional.onUpdateVector2 = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }
        public LTDescr setOnUpdateVector3(Action<Vector3> onUpdate)
        {
            this._optional.onUpdateVector3 = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }
        public LTDescr setOnUpdateColor(Action<Color> onUpdate)
        {
            this._optional.onUpdateColor = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }
        public LTDescr setOnUpdateColor(Action<Color, object> onUpdate)
        {
            this._optional.onUpdateColorObject = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }

#if !UNITY_FLASH

        public LTDescr setOnUpdate(Action<Color> onUpdate)
        {
            this._optional.onUpdateColor = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }

        public LTDescr setOnUpdate(Action<Color, object> onUpdate)
        {
            this._optional.onUpdateColorObject = onUpdate;
            this.hasUpdateCallback = true;
            return this;
        }

        /**
        * Have a method called on each frame that the tween is being animated (passes a float value and a object)
        * @method setOnUpdate (object)
        * @param {Action<float,object>} onUpdate:Action<float,object> a method that will be called on every frame with the float value of the tweened object, and an object of the person's choosing
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnUpdate( tweenMoved ).setOnUpdateParam( myObject );<br>
        * <br>
        * void tweenMoved( float val, object obj ){ }<br>
        */
        public LTDescr setOnUpdate(Action<float, object> onUpdate, object onUpdateParam = null)
        {
            this._optional.onUpdateFloatObject = onUpdate;
            this.hasUpdateCallback = true;
            if (onUpdateParam != null)
                this._optional.onUpdateParam = onUpdateParam;
            return this;
        }

        public LTDescr setOnUpdate(Action<Vector3, object> onUpdate, object onUpdateParam = null)
        {
            this._optional.onUpdateVector3Object = onUpdate;
            this.hasUpdateCallback = true;
            if (onUpdateParam != null)
                this._optional.onUpdateParam = onUpdateParam;
            return this;
        }

        public LTDescr setOnUpdate(Action<Vector2> onUpdate, object onUpdateParam = null)
        {
            this._optional.onUpdateVector2 = onUpdate;
            this.hasUpdateCallback = true;
            if (onUpdateParam != null)
                this._optional.onUpdateParam = onUpdateParam;
            return this;
        }

        /**
        * Have a method called on each frame that the tween is being animated (passes a float value)
        * @method setOnUpdate (Vector3)
        * @param {Action<Vector3>} onUpdate:Action<Vector3> a method that will be called on every frame with the float value of the tweened object
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnUpdate( tweenMoved );<br>
        * <br>
        * void tweenMoved( Vector3 val ){ }<br>
        */
        public LTDescr setOnUpdate(Action<Vector3> onUpdate, object onUpdateParam = null)
        {
            this._optional.onUpdateVector3 = onUpdate;
            this.hasUpdateCallback = true;
            if (onUpdateParam != null)
                this._optional.onUpdateParam = onUpdateParam;
            return this;
        }
#endif


        /**
        * Have an object passed along with the onUpdate method
        * @method setOnUpdateParam
        * @param {object} onUpdateParam:object an object that will be passed along with the onUpdate method
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnUpdate( tweenMoved ).setOnUpdateParam( myObject );<br>
        * <br>
        * void tweenMoved( float val, object obj ){ }<br>
        */
        public LTDescr setOnUpdateParam(object onUpdateParam)
        {
            this._optional.onUpdateParam = onUpdateParam;
            return this;
        }

        /**
        * While tweening along a curve, set this property to true, to be perpendicalur to the path it is moving upon
        * @method setOrientToPath
        * @param {bool} doesOrient:bool whether the gameobject will orient to the path it is animating along
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.move( ltLogo, path, 1.0f ).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setAxis(Vector3.forward);<br>
        */
        public LTDescr SetOrientToPath(bool doesOrient)
        {
            if (this.type == TweenAction.MoveBezier || this.type == TweenAction.MoveBezierLocal)
            {
                if (this._optional.path == null)
                    _optional.path = new LTBezierPath();
                this._optional.path.orientToPath = doesOrient;
            }
            else
            {
                this._optional.spline.orientToPath = doesOrient;
            }
            return this;
        }

        /**
        * While tweening along a curve, set this property to true, to be perpendicalur to the path it is moving upon
        * @method setOrientToPath2d
        * @param {bool} doesOrient:bool whether the gameobject will orient to the path it is animating along
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.move( ltLogo, path, 1.0f ).setEase(LeanTweenType.easeOutQuad).setOrientToPath2d(true).setAxis(Vector3.forward);<br>
        */
        public LTDescr setOrientToPath2d(bool doesOrient2d)
        {
            SetOrientToPath(doesOrient2d);
            if (this.type == TweenAction.MoveBezier || this.type == TweenAction.MoveBezierLocal)
            {
                this._optional.path.orientToPath2d = doesOrient2d;
            }
            else
            {
                this._optional.spline.orientToPath2d = doesOrient2d;
            }
            return this;
        }

        public LTDescr setRect(LTRect rect)
        {
            this._optional.ltRect = rect;
            return this;
        }

        public LTDescr setRect(Rect rect)
        {
            this._optional.ltRect = new LTRect(rect);
            return this;
        }

        public LTDescr setPath(LTBezierPath path)
        {
            this._optional.path = path;
            return this;
        }

        /**
        * Set the point at which the GameObject will be rotated around
        * @method setPoint
        * @param {Vector3} point:Vector3 point at which you want the object to rotate around (local space)
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.rotateAround( cube, Vector3.up, 360.0f, 1.0f ) .setPoint( new Vector3(1f,0f,0f) ) .setEase( LeanTweenType.easeInOutBounce );<br>
        */
        public LTDescr setPoint(Vector3 point)
        {
            this._optional.point = point;
            return this;
        }

        public LTDescr setDestroyOnComplete(bool doesDestroy)
        {
            this.destroyOnComplete = doesDestroy;
            return this;
        }

        public LTDescr setAudio(object audio)
        {
            this._optional.onCompleteParam = audio;
            return this;
        }

        /**
        * Set the onComplete method to be called at the end of every loop cycle (also applies to the delayedCall method)
        * @method setOnCompleteOnRepeat
        * @param {bool} isOn:bool does call onComplete on every loop cycle
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.delayedCall(gameObject,0.3f, delayedMethod).setRepeat(4).setOnCompleteOnRepeat(true);
        */
        public LTDescr setOnCompleteOnRepeat(bool isOn)
        {
            this.onCompleteOnRepeat = isOn;
            return this;
        }

        /**
        * Set the onComplete method to be called at the beginning of the tween (it will still be called when it is completed as well)
        * @method setOnCompleteOnStart
        * @param {bool} isOn:bool does call onComplete at the start of the tween
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.delayedCall(gameObject, 2f, ()=>{<br> // Flash an object 5 times
        * &nbsp;LeanTween.alpha(gameObject, 0f, 1f);<br>
        * &nbsp;LeanTween.alpha(gameObject, 1f, 0f).setDelay(1f);<br>
        * }).setOnCompleteOnStart(true).setRepeat(5);<br>
        */
        public LTDescr setOnCompleteOnStart(bool isOn)
        {
            this.onCompleteOnStart = isOn;
            return this;
        }

        public LTDescr SetRect(RectTransform rect)
        {
            this.rectTransform = rect;
            return this;
        }

        public LTDescr setSprites(UnityEngine.Sprite[] sprites)
        {
            this.sprites = sprites;
            return this;
        }

        public LTDescr setFrameRate(float frameRate)
        {
            this.time = this.sprites.Length / frameRate;
            return this;
        }

        /**
        * Have a method called when the tween starts
        * @method setOnStart
        * @param {Action<>} onStart:Action<> the method that should be called when the tween is starting ex: tweenStarted( ){ }
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * <i>C#:</i><br>
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnStart( ()=>{ Debug.Log("I started!"); });
        * <i>Javascript:</i><br>
        * LeanTween.moveX(gameObject, 5f, 2.0f ).setOnStart( function(){ Debug.Log("I started!"); } );
        */
        public LTDescr setOnStart(Action onStart)
        {
            this._optional.onStart = onStart;
            return this;
        }

        /**
        * Set the direction of a tween -1f for backwards 1f for forwards (currently only bezier and spline paths are supported)
        * @method setDirection
        * @param {float} direction:float the direction that the tween should run, -1f for backwards 1f for forwards
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.moveSpline(gameObject, new Vector3[]{new Vector3(0f,0f,0f),new Vector3(1f,0f,0f),new Vector3(1f,0f,0f),new Vector3(1f,0f,1f)}, 1.5f).setDirection(-1f);<br>
        */

        public LTDescr setDirection(float direction)
        {
            if (this.direction != -1f && this.direction != 1f)
            {
                Debug.LogWarning("You have passed an incorrect direction of '" + direction + "', direction must be -1f or 1f");
                return this;
            }

            if (this.direction != direction)
            {
                // Debug.Log("reverse path:"+this.path+" spline:"+this._optional.spline+" hasInitiliazed:"+this.hasInitiliazed);
                if (this.hasInitiliazed)
                {
                    this.direction = direction;
                }
                else
                {
                    if (this._optional.path != null)
                    {
                        this._optional.path = new LTBezierPath(LTUtility.reverse(this._optional.path.pts));
                    }
                    else if (this._optional.spline != null)
                    {
                        this._optional.spline = new LTSpline(LTUtility.reverse(this._optional.spline.pts));
                    }
                    // this.passed = this.time - this.passed;
                }
            }

            return this;
        }

        /**
        * Set whether or not the tween will recursively effect an objects children in the hierarchy
        * @method setRecursive
        * @param {bool} useRecursion:bool whether the tween will recursively effect an objects children in the hierarchy
        * @return {LTDescr} LTDescr an object that distinguishes the tween
        * @example
        * LeanTween.alpha(gameObject, 0f, 1f).setRecursive(true);<br>
        */

        public LTDescr setRecursive(bool useRecursion)
        {
            this.useRecursion = useRecursion;

            return this;
        }
    }

    //}
}