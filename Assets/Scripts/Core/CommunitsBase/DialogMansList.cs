using System;
using System.Collections;
using System.Collections.Generic;
using Core.Dialogs.Game;
using Core.PlayerScripts;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class DialogMansList : StartupObject
    {
        [SerializeField] private List<Dialoger> dialogers;
        private PlayerDataManager playerDataManager;

        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            
            this.playerDataManager = playerDataManager;
            
            KillDeads();
        }
        
        
        public void KillDeads()
        {
            
            var tutor = playerDataManager.Services.TutorialsManager.TutorialData;
            if (!tutor.ValuesData.HaveWatchDemo(Demos.BaseDemo))
            {
                tutor.MainBaseData.SetKilled(new List<string>());
            }
            foreach (var dialogs in tutor.MainBaseData.KilledDialogs)
            {
                var dialoger = dialogers.Find(x => x.Dialog.name == dialogs);
                Destroy(dialoger.gameObject);
            }

            dialogers.RemoveAll(x => x == null);
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
