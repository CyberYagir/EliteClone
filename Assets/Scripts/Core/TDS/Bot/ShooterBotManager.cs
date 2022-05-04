using System.Collections;
using System.Collections.Generic;
using Core.TDS.Bot;
using UnityEngine;

namespace Core
{
    public class ShooterBotManager : MonoBehaviour
    {
        [SerializeField] private List<ShooterBotAgression> units;


        public void Agr(Vector3 startPos, float radius)
        {
            foreach (var unit in units)
            {
                if (Vector3.Distance(unit.transform.position, startPos) <= radius)
                {
                    unit.Agr(true);
                }
            }
        }
    }
}
