using System.IO;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class SaveLoadScriptable<T> : MonoBehaviour where T : ScriptableObject
    {
        [SerializeField] protected T scriptable;
        protected string path;


        public virtual void Init(string path)
        {
            this.path = path;
        }
        public void Reload()
        {
            Clear();
            Load();
        }
        public virtual void Clear()
        {
            scriptable = null;
        }
        public virtual void Load()
        {
            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<T>();
            }

            if (File.Exists(path))
            {
                JsonUtility.FromJsonOverwrite(File.ReadAllText(path), scriptable);
            }
            else
            {
                Save();
            }
        }

        public virtual void Save()
        {
            File.WriteAllText(path, JsonUtility.ToJson(scriptable));
        }
    }
}