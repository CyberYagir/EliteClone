using System.Collections.Generic;
using Core.Init;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class InitChangeLogSubItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text typeText;
        [SerializeField] private TMP_Text item;


        public void Init(InitChangelog.Version.Change changes)
        {
            typeText.text = changes.type.ToString();

            SpawnList(changes.Gameplay);
            SpawnList(changes.Location);
            SpawnList(changes.Menu);
            SpawnList(changes.System);
            
        }

        public void SpawnList(List<string> str)
        {
            item.gameObject.SetActive(true);

            for (int i = 0; i < str.Count; i++)
            {
                Instantiate(item, item.transform.parent).text = str[i];
            }
            
            item.gameObject.SetActive(false);
        }
    }
}