using UnityEditor;
using UnityEditor.Callbacks;

namespace Core.Dialogs
{
    public class MyAssetHandler
    {
        [OnOpenAsset(1)]
        public static bool Open(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is ExtendedDialog)
            {
                ExtendedDialogsGraph.OpenWindow();
                ExtendedDialogsGraph.GetGraph().SetFile(obj as ExtendedDialog);
                ExtendedDialogsGraph.GetGraph().DataOperation(false);
            }
            return false; // we did not handle the open
        }

    }
}