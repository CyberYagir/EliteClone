using UnityEngine;

namespace UI
{
    public class UITweaks : MonoBehaviour
    {
        public static void ClearHolder(Transform holder)
        {
            foreach (Transform item in holder)
            {
                if (item.gameObject.active)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}