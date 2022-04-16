using System;
using Core.Game;
using UnityEngine;

namespace Core.Init
{
    [Serializable]
    public class CursorManager: MonoBehaviour
    {
        public enum CursorType
        {
            Normal, Action, Drag
        }

        public static CursorsSheetItem currentSheet;
        public static CursorType currentType;
        private void Awake()
        {
            SetCurrentSheet(Resources.LoadAll<CursorsSheetItem>("")[0]);
        }

        public static void SetCurrentSheet(CursorsSheetItem newSheet)
        {
            currentSheet = newSheet;
            ChangeCursor(CursorType.Normal);
        }

        public static void ChangeCursor(CursorType type)
        {
            if (currentSheet != null)
            {
                var cursor = currentSheet.GetTexture(type);
                Cursor.SetCursor(cursor.texture, cursor.hotpost, CursorMode.ForceSoftware);
                currentType = type;
            }
        }
    }
}
