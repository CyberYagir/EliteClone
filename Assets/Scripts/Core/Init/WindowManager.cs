using System.Collections.Generic;
using UnityEngine;

namespace Core.Init
{
    public class WindowManager : Singleton<WindowManager>
    {
        [SerializeField] private List<CustomAnimate> customAnimate;
        private void Awake()
        {
            Single(this);
        }

        public void FixedUpdate()
        {
            foreach (var animations in customAnimate)
            {
                animations.CustomUpdate();
            }
        }

        public void OpenWindow(CustomAnimate animate)
        {
            foreach (var item in customAnimate)
            {
                if (item == animate)
                {
                    item.reverse = !item.reverse;
                }
                else
                {
                    item.reverse = true;
                }
            }
        }
        public void CloseWindow(CustomAnimate animate)
        {
            animate.reverse = true;
        }
    }
}
