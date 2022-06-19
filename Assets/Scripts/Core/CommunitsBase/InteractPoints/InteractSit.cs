using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class InteractSit : MonoBehaviour
    {
        [SerializeField] private InteractPoint point;


        public void OnArrive()
        {
            var person = point.GetLastPerson();
            if (person != null)
            {
                person.DOMove(point.transform.position, 1f);
                person.DORotateQuaternion(point.transform.rotation, 1f);
            }
        }
    }
}
