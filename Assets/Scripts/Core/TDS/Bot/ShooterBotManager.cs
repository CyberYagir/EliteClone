using System.Collections;
using System.Collections.Generic;
using Core.TDS;
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
                var dist = Vector3.Distance(unit.transform.position, startPos);
                if (dist != 0 && dist <= radius / 2)
                {
                    if (!unit.GetComponent<Shooter>().isDead)
                    {
                        unit.Agr(true);
                    }
                }
            }
        }
    }
}
