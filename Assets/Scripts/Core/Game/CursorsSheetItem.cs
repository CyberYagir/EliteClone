using System;
using System.Collections.Generic;
using Core.Init;
using UnityEngine;

namespace Core.Game
{
    [CreateAssetMenu(fileName = "", menuName = "Game/Cursors", order = 1)]
    public class CursorsSheetItem : ScriptableObject
    {
        [Serializable]
        public class CursorTexture
        {
            public CursorManager.CursorType type;
            public Vector2 hotpost;
            public Texture2D texture;
        }
        public List<CursorTexture> cursors = new List<CursorTexture>();

        public CursorTexture GetTexture(CursorManager.CursorType type)
        {
            return cursors.Find(x => x.type == type);
        }
    }
}