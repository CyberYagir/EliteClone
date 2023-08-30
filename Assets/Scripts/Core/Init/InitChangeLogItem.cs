using System.Collections;
using System.Collections.Generic;
using Core.Init;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class InitChangeLogItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text versionText;
        [SerializeField] private InitChangeLogSubItem subItem;


        public void Init(InitChangelog.Version version)
        {
            versionText.text = "v. " + version.version;

            subItem.gameObject.SetActive(true);
            foreach (var changes in version.changes)
            {
                Instantiate(subItem, subItem.transform.parent).Init(changes);
            }
            subItem.gameObject.SetActive(false);
        }
    }
}
