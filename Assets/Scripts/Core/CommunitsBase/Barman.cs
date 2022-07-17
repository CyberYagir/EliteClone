using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase.Intacts;
using Core.Dialogs;
using Core.Dialogs.Game;
using Core.Game;
using Core.PlayerScripts;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class Barman : MonoBehaviour
    {
        [SerializeField] private Item weapon;
        [SerializeField] private List<CapsuleCollider> toEnable;
        [SerializeField] private List<ShooterInteractor> terminals;

        public void Trigger(Actions action)
        {
            if (action == Actions.Negative)
            {
                ShooterPlayer.Instance.inventory.Add(weapon);
                ShooterPlayer.Instance.weaponSelect.ChangeWeapon(0);
                GetComponent<Dialoger>().enabled = false;
                GetComponent<ShooterInteractor>().enabled = false;

                foreach (var bot in NPCSpawner.botList)
                {
                    bot.GetComponent<CapsuleCollider>().enabled = true;
                }

                foreach (var terminal in terminals)
                {
                    terminal.enabled = true;
                }
                
                foreach (var coll in toEnable)
                {
                    coll.enabled = true;
                }
            }
        }
    }
}
