using UnityEngine;

namespace UI
{
    public class UITweaks : MonoBehaviour
    {
        public static void ClearHolder(Transform holder)
        {
            if (holder != null)
            {
                foreach (Transform item in holder)
                {
                    if (item.gameObject.activeSelf)
                    {
                        Destroy(item.gameObject);
                    }
                }
            }
            else
            {
                Debug.LogError("Holder null!!");
            }
        }
    }
}