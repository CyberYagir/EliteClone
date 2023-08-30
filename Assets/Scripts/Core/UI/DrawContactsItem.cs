using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class DrawContactsItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image iconFraction;

        public Image IconFraction => iconFraction;

        public Image Icon => icon;
    }
}
