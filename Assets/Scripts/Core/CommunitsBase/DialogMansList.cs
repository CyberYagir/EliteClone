using System;
using System.Collections;
using System.Collections.Generic;
using Core.Dialogs.Game;
using Core.PlayerScripts;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class DialogMansList : MonoBehaviour
    {
        public List<Dialoger> dialogers;

        public void KillDeads()
        {
            var tutor = PlayerDataManager.Instance.Services.TutorialsManager.TutorialData;
            if (tutor.CommunitsBaseStats != null)
            {
                foreach (var dialogs in tutor.CommunitsBaseStats.killedDialogs)
                {
                    var dialoger = dialogers.Find(x => x.Dialog.name == dialogs);
                    Destroy(dialoger.gameObject);
                }
            }

            dialogers.RemoveAll(x => x == null);
        }

        private void Start()
        {
            KillDeads();
        }

        public List<string> GetDead()
        {
            List<string> deads = new List<string>();
            for (int i = 0; i < dialogers.Count; i++)
            {
                if (dialogers[i].GetComponent<RagdollActivator>().isActived)
                {
                    deads.Add(dialogers[i].Dialog.name);
                }
            }

            return deads;
        }
    }
}
