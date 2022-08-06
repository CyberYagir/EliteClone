using System.Linq;
using Core.PlayerScripts;
using Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [System.Serializable]
    public class NavItem
    {
        public ButtonEffect Button;
        public Image Image, Icon;
        public TMP_Text Name, Dist;
        public GalaxyObject SpaceObject;
    }

    public class DrawNavigation : DrawNavList
    {
        
        protected override void Start()
        {
            base.Start();
            Player.inst.targets.OnChangeTarget += UpdateList;
        }

        public override void RedrawList()
        {
            base.RedrawList(); 
            var objects = FindObjectsOfType<WorldSpaceObject>();
            objects = objects.Reverse().ToArray();
            for (int i = 0; i < objects.Length; i++)
            {
                var b = Instantiate(item, holder.transform);

                if (objects[i].transform.parent != null)
                {
                    if (objects[i].transform.parent.GetComponent<WorldSpaceObject>())
                    {
                        var rect = b.GetComponent<RectTransform>();
                        rect.sizeDelta = new Vector2(rect.sizeDelta.x * 0.95f, rect.sizeDelta.y);
                    }
                }

                var navI = new NavItem();
                navI.Button = b.GetComponent<ButtonEffect>();
                navI.SpaceObject = objects[i];
                navI.Image = objects[i].GetComponent<Image>();
                navI.Name = b.transform.GetChild(0).GetComponent<TMP_Text>();
                navI.Dist = b.transform.GetChild(1).GetComponent<TMP_Text>();
                navI.Icon = b.transform.GetChild(2).GetComponent<Image>();
                navI.Icon.sprite = objects[i].icon;
                navI.Name.text = objects[i].transform.name;
                navI.Dist.text = objects[i].dist;
                b.SetActive(true);
                items.Add(navI);
            }
        }
    }
}