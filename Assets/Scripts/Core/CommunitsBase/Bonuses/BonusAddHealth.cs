using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase.Bonuses
{
    public class BonusAddHealth : DropBonus
    {
        public override void AddBonus(ShooterData data)
        {
            base.AddBonus(data);
            data.TakeDamage(-bonus);
        }
    }
}
