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
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.TargetsManager.ContactsChanges += UpdateList;
        }

        public override void RedrawList()
        {
            base.RedrawList();
            var objects = PlayerDataManager.Instance.WorldHandler.ShipPlayer.TargetsManager.contacts;
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    if (!objects[i].GetComponent<Player>())
                    {
                        var b = Instantiate(item, holder.transform).GetComponent<DrawContactsItem>();
                        var icon = b.Icon;
                        var iconFraction = b.IconFraction;
                        
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

                            var sprite = WorldDataItem.GetData().fractions.IconByID(bot.Fraction);
                            
                            iconFraction.sprite = sprite;
                        }
                        else
                            iconFraction.gameObject.SetActive(false);


                        navI.Icon.sprite = objects[i].icon;
                        navI.Name.text = objects[i].transform.name;
                        b.gameObject.SetActive(true);
                        items.Add(navI);
                    }
                }
            }
        }
    }
}
