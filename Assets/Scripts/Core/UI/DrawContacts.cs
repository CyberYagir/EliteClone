using Core.Bot;
using Core.Game;
using Core.PlayerScripts;
using Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class DrawContacts : DrawNavList
    {
        protected override void Start()
        {
            base.Start();
            Player.inst.targets.ContactsChanges += UpdateList;
        }

        public override void RedrawList()
        {
            base.RedrawList();
            var objects = Player.inst.targets.contacts;
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    if (!objects[i].GetComponent<Player>())
                    {
                        var b = Instantiate(item, holder.transform);
                        var icon = b.transform.GetChild(1).GetComponent<Image>();
                        var iconFraction = b.transform.GetChild(2).GetComponent<Image>();
                        
                        
                        var navI = new NavItem();
                        navI.Button = b.GetComponent<ButtonEffect>();
                        navI.SpaceObject = objects[i].GetComponent<GalaxyObject>();
                        navI.Image = objects[i].GetComponent<Image>();
                        navI.Name = b.transform.GetComponentInChildren<TMP_Text>();
                        navI.Icon = icon;
                        var bot = objects[i].GetComponent<BotBuilder>();

                        if (bot)
                        {
                            iconFraction.gameObject.SetActive(true);
                            iconFraction.sprite = WorldDataItem.GetData().fractions.IconByID(bot.Fraction);
                        }
                        else
                            iconFraction.gameObject.SetActive(false);


                        navI.Icon.sprite = objects[i].icon;
                        navI.Name.text = objects[i].transform.name;
                        b.SetActive(true);
                        items.Add(navI);
                    }
                }
            }
        }
    }
}
