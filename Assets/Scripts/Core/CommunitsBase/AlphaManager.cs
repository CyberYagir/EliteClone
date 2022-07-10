using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;
using static Core.CommunistsBase.AlphaController;

namespace Core.CommunistsBase
{
    public class AlphaManager : MonoBehaviour
    {
        public static List<RoomData> datas = new List<RoomData>(10);
        public List<AlphaController> allRooms = new List<AlphaController>();
        private RaycastHit[] hits = new RaycastHit[30];
        private void Start()
        {
            foreach (var room in allRooms)
            {
                if (datas.Find(x => x.Obj == room.transform) == null)
                {
                    datas.Add(new RoomData(room.transform));
                }
            }
        }


        private void FixedUpdate()
        {
            if (!ShooterPlayer.Instance.enabled) return;
            for (int i = 0; i < datas.Count; i++)
            {
                var room = datas[i];
                var casts = room.controller.casts;
                room.controller.players.Clear();
                for (int j = 0; j < casts.Count; j++)
                {
                    var size = Physics.BoxCastNonAlloc(transform.position + casts[j].pos, casts[j].size / 2f, Vector3.down, hits, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("Main"), QueryTriggerInteraction.Ignore);
                    for (int k = 0; k < size; k++)
                    {
                        if (hits[k].transform.GetComponent<ShooterPlayer>())
                        {
                            room.controller.players.Add(hits[k].transform);
                            break;
                        }
                    }
                }

                if (room.controller.players.Count != 0 || room.controller.overrideActive)
                {
                    room.Active(true);
                }
                else
                {
                    room.Active(false);
                }
            }
        }

        private void OnDestroy()
        {
            datas = new List<RoomData>(10);
        }
    }
}
