using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
using Core.CommunistsBase.Intacts;
using Core.Dialogs;
using Core.TDS;
using DG.Tweening;
using UnityEngine;
using static Core.Dialogs.ExtendedDialog.NodeReplicaData;

namespace Core.Dialogs.Game
{
    public class Dialoger : MonoBehaviour
    {
        [SerializeField] private ExtendedDialog dialog;
        private List<ExtendedDialog.NodeReplicaData> replicas = new List<ExtendedDialog.NodeReplicaData>();


        public Event<Actions> OnTrigger = new Event<Actions>();
        
        private void Start()
        {
            replicas = dialog.GetConvertedReplicas();
        }

        public void Init()
        {
            StopAllCoroutines();
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
            character = Characters.Second;
            StartCoroutine(DialogLoop());
            cameraLoop = CameraLoop();
            StartCoroutine(cameraLoop);
          
        }

        private IEnumerator cameraLoop;

        private Characters character;
        IEnumerator CameraLoop()
        {
            while (true)
            {
                yield return null;
                if (character == Characters.Second)
                {
                    PlayerTDSCamera.Instance.GetCamera().transform.DOLookAt(transform.position + Vector3.up, 0.5f);
                }
                else
                {
                    PlayerTDSCamera.Instance.GetCamera().transform.DOLookAt(ShooterPlayer.Instance.transform.position + Vector3.up, 0.5f);
                }
            }
        }

        private int choice = 0;
        IEnumerator DialogLoop()
        {
            OnThrowReplica = new Event<string>();
            OnShowChoice = new Event<List<TextReplica>>();
            OnEnd = new Event();
            OnInit.Run(this);   
            var currentDialogNode = replicas[0];
            
            while (true)
            {
                yield return null;
                if (currentDialogNode == null)
                {
                    OnEnd.Run();
                    yield break;
                }

                character = currentDialogNode.character;
                if (currentDialogNode.type == NodeType.Dialog)
                {
                    yield return StartCoroutine(TextThrow(currentDialogNode.text));
                }



                switch (currentDialogNode.classname)
                {
                    case ClassName.NodeAutoReplicaData:
                        currentDialogNode = NextNode(currentDialogNode.nextGUID);
                        print("NodeAutoReplicaData");
                        break;
                    case ClassName.NodeMultiReplicaData:
                        print("NodeMultiReplicaData");
                        OnShowChoice.Run(currentDialogNode.nexts);
                        choice = 0;
                        OnChangeChoice.Run(choice);
                        character = Characters.Main;
                        yield return StartCoroutine(SelectChoice(currentDialogNode.nexts));
                        yield return null;
                        yield return StartCoroutine(TextThrow(currentDialogNode.nexts[choice].replica));
                        character = Characters.Second;
                        currentDialogNode = NextNode(currentDialogNode.nexts[choice].nextGUID);
                        break;
                    case ClassName.NodeTriggerData:
                        print("NodeTriggerData");
                        OnTrigger.Run(currentDialogNode.action);
                        OnEnd.Run();
                        yield break;
                        break;
                    case ClassName.NodeEndData:
                        
                        print("NodeEndData");
                        PlayerTDSCamera.ChangeMode(PlayerTDSCamera.CameraModes.Control);
                        ShooterPlayer.Instance.controller.enabled = true;
                        StopCoroutine(cameraLoop);
                        ShooterPlayerInteractor.interacted = false;
                        PlayerTDSCamera.Instance.GetCamera().transform.DOKill();
                        if (currentDialogNode.triggerGUID != "")
                        {
                            currentDialogNode = NextNode(currentDialogNode.triggerGUID);
                        }
                        else
                        {
                            OnEnd.Run();
                            yield break;
                        }
                        break;
                }
            }
        }

        public ExtendedDialog.NodeReplicaData NextNode(string guid)
        {
            var next = replicas.Find(x => x.GUID == guid);
            return next;
        }
        
        IEnumerator SelectChoice(List<TextReplica> replicas)
        {
            while (true)
            {
                yield return null;
                if (InputM.GetAxisDown(KAction.TabsVertical))
                {
                    choice -= InputM.GetAxisRaw(KAction.TabsVertical);
                    if (choice >= replicas.Count)
                    {
                        choice = 0;
                    }
                    if (choice < 0)
                    {
                        choice = replicas.Count - 1;
                    }
                }

                if (InputM.GetAxisDown(KAction.Interact))
                {
                    break;
                }
                OnChangeChoice.Run(choice);
            }
        }
        IEnumerator TextThrow(string text)
        {
            OnThrowReplica.Run(text);
            float timer = 0;
            float time = text.Length / 10f;
            if (time < 2)
            {
                time = 2;
            }

            SetAnim(true);
            while (timer < time && !InputM.GetAxisDown(KAction.Interact))
            {
                timer += Time.deltaTime;
                yield return null;
            }

            SetAnim(false);
        }

        public void SetAnim(bool state)
        {
            if (character == Characters.Main)
            {
                ShooterPlayer.Instance.animator.Get().SetBool(IsTalk, state);
                StartCoroutine(ShooterAnimator.ChangeLayer(ShooterPlayer.Instance.animator.Get(), 2, 2, 0, state));
            }
            else
            {
                GetComponent<TDSPointsWaker>().SetAnimBool(IsTalk, state);
            }
        }
        
        public Event<string> OnThrowReplica = new Event<string>();
        public Event<List<TextReplica>> OnShowChoice = new Event<List<TextReplica>>();
        public Event<Dialoger> OnInit = new Event<Dialoger>();
        public Event<int> OnChangeChoice = new Event<int>();
        public Event OnEnd = new Event();
        private static readonly int IsTalk = Animator.StringToHash("IsTalk");
    }
}
