using System;
using System.Collections;
using System.Collections.Generic;
using Core.Bot;
using Core.Game;
using Core.PlayerScripts.Weapon;
using UnityEngine;

namespace Core.PlayerScripts.Weapon
{
    public class WeaponCannon : Weapon
    {
        private bool isPlayer;

        private BotBuilder bot;
        protected override void InitData()
        {
            isPlayer = GetComponentInParent<Player>() != null;

            if (!isPlayer)
            {
                bot = GetComponentInParent<BotBuilder>();
            }
        }

        private void Update()
        {
            time += Time.deltaTime;
        }


        protected override void Attack()
        {
            if (time >= cooldown)
            {
                var rocket = Instantiate(options.GetObject("Rocket") as GameObject, transform.position, transform.rotation).GetComponent<Rocket>();
                Transform target = Player.inst.GetTarget() == null ? null : Player.inst.GetTarget().transform;
                rocket.Init(isPlayer ? target : Player.inst.transform, damage, isPlayer);
                time = 0;
            }
        }
    }
}
