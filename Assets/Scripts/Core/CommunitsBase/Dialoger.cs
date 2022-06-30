using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
using Core.Dialogs;
using Core.TDS;
using DG.Tweening;
using UnityEngine;
using static Core.Dialogs.ExtendedDialog.NodeReplicaData;

namespace Core.Dialogs.Game
{
    public class Dialoger : MonoBehaviour
    {
        public ExtendedDialog dialog;

        public void Init()
        {
            PlayerTDSCamera.ChangeMode(PlayerTDSCamera.CameraModes.OutsideControl);
            ShooterPlayer.Instance.controller.enabled = false;
            ShooterPlayer.Instance.transform.DOLookAt(new Vector3(transform.position.x, ShooterPlayer.Instance.transform.position.y, transform.position.z), 0.5f);
            var playerpos = ShooterPlayer.Instance.transform.position;
            if (Vector3.Distance(playerpos, transform.position) < 1.5f)
            {
                playerpos = transform.position + transform.forward * 1;
                ShooterPlayer.Instance.transform.DOMove(playerpos, 0.5f);
            }

            var camPos = Vector3.Lerp(playerpos, transform.position, 0.5f);
            camPos += Vector3.up * 1.5f;
            camPos += -transform.right * 2f;
            PlayerTDSCamera.Instance.GetCamera().transform.DOMove(camPos, 0.5f);
            StartCoroutine(DialogLoop());
            StartCoroutine(CameraLoop());
        }

        IEnumerator CameraLoop()
        {
            while (true)
            {
                yield return null;
                PlayerTDSCamera.Instance.GetCamera().transform.DOLookAt(transform.position + Vector3.up, 0.5f);
            }
        }

        IEnumerator DialogLoop()
        {
            OnThrowReplica = new Event<string>();
            OnShowChoice = new Event<List<TextReplica>>();
            OnInit.Run(this);   
            int choice = 0;
            var replicas = dialog.GetConvertedReplicas();
            var currentDialogNode = replicas[0];
            
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(TextThrow(currentDialogNode));
                
                
                OnShowChoice.Run(currentDialogNode.nexts);
                
                switch (currentDialogNode.classname)
                {
                    case ClassName.NodeReplicaData:
                        break;
                    case ClassName.NodeAutoReplicaData:
                        break;
                    case ClassName.NodeMultiReplicaData:
                        break;
                    case ClassName.NodeTriggerData:
                        break;
                    case ClassName.NodeEndData:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        IEnumerator TextThrow(ExtendedDialog.NodeReplicaData currentDialogNode)
        {
            OnThrowReplica.Run(currentDialogNode.text);
            float timer = 0;
            while (timer < (currentDialogNode.text.Length/20f) && !InputM.GetAxisDown(KAction.Interact))
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        public Event<string> OnThrowReplica = new Event<string>();
        public Event<List<TextReplica>> OnShowChoice = new Event<List<TextReplica>>();
        public Event<Dialoger> OnInit = new Event<Dialoger>();
    }
}
