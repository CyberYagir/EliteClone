using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase.Bonuses
{
    public class BonusAddEnergy : DropBonus
    {
        public override void AddBonus(ShooterData data)
        {
            base.AddBonus(data);
            data.RemoveEnergy(-bonus);
        }
    }
}
