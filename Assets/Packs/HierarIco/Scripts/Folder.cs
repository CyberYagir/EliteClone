using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class Folder : MonoBehaviour
{
    
#if UNITY_EDITOR
    [MenuItem("GameObject/Create Folder", false, -1)]
    public static void CreateFolder()   
    {
        GameObject folder = new GameObject("Folder");
        if (Selection.activeObject != null)
        {
            if (Selection.activeObject is GameObject)
            {
                folder.transform.parent = (Selection.activeObject as GameObject)?.transform;
            }
        }

        folder.transform.gameObject.AddComponent<Folder>();
        folder.transform.localPosition = Vector3.zero;

        Selection.activeObject = folder;
    }
    private void LateUpdate()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = Vector3.zero;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Folder Icon");
    }
#endif
}
