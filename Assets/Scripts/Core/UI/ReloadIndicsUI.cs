using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.PlayerScripts;
using Core.PlayerScripts.Weapon;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ReloadIndicsUI : MonoBehaviour
    {
        [SerializeField] private Transform item;
        private Dictionary<Weapon, List<Image>> spawnedItems = new Dictionary<Weapon, List<Image>>();
        public Color activeColor;


        private void Start()
        {
            Init();
        }

        public void Init()
        {
            item.gameObject.SetActive(false);
            UITweaks.ClearHolder(transform);
            var weapons = Player.inst.GetComponentsInChildren<Weapon>(false).ToList();
            spawnedItems = new Dictionary<Weapon, List<Image>>();
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].CurrentID() != -1)
                {
                    var it = Instantiate(item, transform);

                    var circle = it.GetChild(0).GetComponent<Image>();
                    var back = it.GetChild(1).GetComponent<Image>();
                    var weaponCircle = it.GetChild(2).GetComponent<Image>();
                    
                    List<Image> clockwise = new List<Image>(){ weaponCircle, circle};
                    
                    
                    back.sprite = weapons[i].Current().icon;
                    weaponCircle.sprite = weapons[i].Current().icon;
                    
                    spawnedItems.Add(weapons[i], clockwise);
                    it.gameObject.SetActive(true);
                }
            }
        }

        private void Update()
        {
            foreach (var w in spawnedItems)
            {
                var val = w.Key.GetReload();
                w.Value[0].fillAmount = val;
                w.Value[1].fillAmount = 1 - val;

                if (val == 1)
                {
                    w.Value[0].color = Color.Lerp(w.Value[0].color, activeColor, 10 * Time.deltaTime);
                }
                else
                {
                    w.Value[0].color = Color.white;
                }
            }
        }
    }
}
