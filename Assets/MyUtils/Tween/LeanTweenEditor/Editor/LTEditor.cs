using System;
using UnityEditor;
using UnityEngine;

namespace My_Utils.Lean_Tween.Visual
{
    public class LTEditor : object
    {
        public GUIStyle styleCodeTextArea;

        public GUIStyle styleOneCharButton;
        public GUIStyle styleDeleteButton;
        public GUIStyle styleClearAllButton;
        public GUIStyle styleRefreshButton;
        public GUIStyle styleDeleteGroupButton;
        public GUIStyle styleCallbackFoldout;
        public GUIStyle styleGroupFoldout;
        public GUIStyle styleItemFoldout;
        public GUIStyle styleGroupButton;
        public GUIStyle styleItemButton;
        public GUIStyle styleCallbackButton;
        public GUIStyle styleActionButton;
        public GUIStyle styleAreaFoldout;
        public GUIStyle styleExportedBox;

        public Color colorGroupName = new Color(0f / 255f, 162f / 255f, 255f / 255f);
        public Color colorDelete = new Color(255f / 255f, 25f / 255f, 25f / 255f);
        public Color colorDestruct = new Color(255f / 255f, 70f / 255f, 70f / 255f);
        public Color colorRefresh = new Color(100f / 255f, 190f / 255f, 255f / 255f);
        public Color colorAdd = new Color(120f / 255f, 255f / 255f, 180f / 255f);
        public Color colorTweenName = new Color(0f / 255f, 255f / 255f, 30f / 255f);
        public Color colorAddTween = new Color(0f / 255f, 209f / 255f, 25f / 255f);
        public Color colorAddGroup = new Color(0f / 255f, 144f / 255f, 226f / 255f);
        public Color colorEasyPathCreator = new Color(150f / 255f, 180f / 255f, 255f / 255f);
        public Color colorNodes = new Color(190f / 255f, 255f / 255f, 190f / 255f);
        public Color colorImportExport = new Color(255f / 255f, 180f / 255f, 150f / 255f);

        public LTEditor()
        {
            if (EditorGUIUtility.isProSkin)
            {
                colorGroupName = new Color(0f / 255f, 162f / 255f, 255f / 255f) * 2f;
                colorDelete = new Color(255f / 255f, 25f / 255f, 25f / 255f) * 2f;
                colorTweenName = new Color(0f / 255f, 255f / 255f, 30f / 255f) * 2f;
                colorAddTween = new Color(0f / 255f, 209f / 255f, 25f / 255f) * 2f;
                colorAddGroup = new Color(0f / 255f, 144f / 255f, 226f / 255f) * 2f;
            }

            styleCodeTextArea = new GUIStyle(GUI.skin.textArea)
            {
                richText = true
            };

            styleDeleteButton = new GUIStyle(GUI.skin.button);
            styleDeleteButton.margin = new RectOffset(styleDeleteButton.margin.left, styleDeleteButton.margin.right, 2, 0);
            styleDeleteButton.padding = new RectOffset(0, 0, 0, 0);
            styleDeleteButton.fixedHeight = 15;
            styleDeleteButton.fixedWidth = 46;

            styleOneCharButton = new GUIStyle(GUI.skin.button);
            styleOneCharButton.padding = new RectOffset(-2, 0, 0, 0);
            styleOneCharButton.fixedHeight = 15;
            styleOneCharButton.fixedWidth = 20;

            styleClearAllButton = new GUIStyle(styleDeleteButton);
            styleClearAllButton.fixedWidth = 80;

            styleRefreshButton = new GUIStyle(styleDeleteButton);
            styleRefreshButton.fixedWidth = 50;

            styleDeleteGroupButton = new GUIStyle(styleDeleteButton);

            styleActionButton = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 25,
                fixedWidth = 140
            };

            styleGroupFoldout = new GUIStyle(EditorStyles.foldout)
            {
                padding = new RectOffset(0, 0, 0, 0),
                overflow = new RectOffset(0, 0, 0, 0),
                fixedWidth = 80
            };
            styleGroupFoldout.margin = new RectOffset(styleGroupFoldout.margin.left + 12, 0, 0, 0);

            styleItemFoldout = new GUIStyle(EditorStyles.foldout)
            {
                padding = new RectOffset(0, 0, 0, 0),
                overflow = new RectOffset(0, 0, 0, 0),
                fixedWidth = 80
            };
            styleItemFoldout.margin = new RectOffset(styleItemFoldout.margin.left + 20, 0, 0, 0);

            styleCallbackFoldout = new GUIStyle(EditorStyles.foldout)
            {
                padding = new RectOffset(0, 0, 0, 0),
                overflow = new RectOffset(0, 0, 0, 0),
                fixedWidth = 80
            };
            styleCallbackFoldout.margin = new RectOffset(styleCallbackFoldout.margin.left + 43, 0, 0, 0);

            styleGroupButton = new GUIStyle(GUI.skin.button)
            {
                margin = new RectOffset(0, 0, 2, 0),
                padding = new RectOffset(0, 0, 0, 0),
                fixedHeight = 15
            };

            styleItemButton = new GUIStyle(GUI.skin.button)
            {
                margin = new RectOffset(15, 0, 2, 0),
                padding = new RectOffset(0, 0, 0, 0),
                fixedHeight = 15
            };

            styleCallbackButton = new GUIStyle(GUI.skin.button)
            {
                margin = new RectOffset(0, 60, 2, 0),
                padding = new RectOffset(0, 0, 0, 0),
                fixedHeight = 15
            };

            styleAreaFoldout = new GUIStyle(EditorStyles.foldout);
            styleAreaFoldout.margin = new RectOffset(styleAreaFoldout.margin.left + 16, 0, styleAreaFoldout.margin.top, 0);
            styleAreaFoldout.fontStyle = FontStyle.Bold;
        }

        private static LTEditor _sharedInstance;

        public static LTEditor Shared
        {
            get
            {
                if (_sharedInstance == null)
                {
                    // Debug.Log("creating instance once...");
                    _sharedInstance = new LTEditor();
                }
                return _sharedInstance;
            }
        }

        // Icon creation script
        const string iconString = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAC4ElEQVQ4EUWT3WsUZxTGf+/M7G6yJmsSs9muxlTzUYJGEtGIYiGVNgjeae5KoUKgoPRfaOlF/4oWirYX3kVEpAhVlFZBUaOtiZouSUmMGJGQxKSbmd2Z6fPObirD4R3O53Oec45pKHwbG+OBxJhU8tb+t3T2dQFr11sXg0uMIy2hfow+R2+Y/BEbBdSkZjNEsayRI0lRjdKyumTSoRLE4f/OYegSVDxSaUinavpNP4Ojqs1NPvnWZXYXl+nZvcT+vpe4ToQXK4GtH0YuLbk1Phl+xMKbnTx42i2dx5nR3xkbvU8x/5ZC+wo7WjfJNUEmB5cuH3vfQuC7fDX2K+Njjzn7zXkqmw693Yt8d+4nPmiHd/+SdLb6zmF2oY2yn+PnKydtCxHVMKa9bZnjQ894/CLPw+mduKmAzo4lLl4ZZaq0l+EDJb7+/Cbf/3CaHyfOJIRGcUotiLgwyHB4X4nOgs+Fq/2sr3k0NpW5+6SPW/cHobKNT49OsrJmuH5nGD9waciIQA2lxoFTZeTQFH4F/njUg3ECiBBJAekGyO1Y48jAU/HyIXOLHTSkywkCDQanWjHsKrxlqH+ev+dbBDdPSvAtMjviSmD4qGueYnuZG/cO4osrqIoQa7dTkMORgVnybSETNwV/qRmyMtrlshxr9of2P2OjDHcm9+F5mwqO67tj1EY64NhgiQ2x3FVYZvyL60IgbMbRLsBv94YY7Cvx54sisy+FzvOV2KLQomnhvL49r+ntesP6Bko0w8jhGRuLK59IPDz/p0hP5yt+ufZZAj+brVgAye7oBvBOfTxN23b1KgIdBYYKcuRgq/81U6A5u0HztpC7k/1KauGruK1eF3PixEjclK1K7yiBdtKWT+Ab5l51aGSNInmV2w8PyEcH5dijsgdWew2NX4oR25MkMdj/mgOeTsxxicIMmcZIN1HX14Otv+eJRHuJtTPVeJRo62y3KpGKMLGWNun8PXwF8R9jRAcsz/JsQQAAAABJRU5ErkJggg==";
        static Texture2D icon;

        public static Texture2D EditorIcon()
        {
            if (icon == null)
            {
                icon = new Texture2D(1, 1)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                icon.LoadImage(Convert.FromBase64String(iconString));
            }

            return icon;
        }
    }
}
