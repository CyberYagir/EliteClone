using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Map;
using TMPro;
using UnityEngine;

namespace Core.Map
{
    public class MapDropsList : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        private void Start()
        {
            dropdown.ClearOptions();
            var options = new List<TMP_Dropdown.OptionData>();

            for (int i = 0; i < MapGenerator.Instance.systems.Count; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(MapGenerator.Instance.systems[i]));
            }

            options = options.OrderBy(x => x.text).ToList();
            dropdown.options = options;
        }

        public void SelectCurrent()
        {
            MapSelect.Instance.ChangeSelected(GameObject.Find(dropdown.options[dropdown.value].text));
        }
    }
}
