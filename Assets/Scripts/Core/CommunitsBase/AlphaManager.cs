using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class AlphaManager : StartupObject
    {
        public static List<RoomData> datas = new List<RoomData>(10);
        public List<AlphaController> allRooms = new List<AlphaController>();
        private RaycastHit[] hits = new RaycastHit[30];
        
        
        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            
            foreach (var room in allRooms)
            {
                if (datas.Find(x => x.Obj == room.transform) == null)
                {
                    datas.Add(new RoomData(room.transform));
                }
            }


            StartCoroutine(Loop());
        }


        IEnumerator Loop()
        {
            while (true)
            {
                if (!ShooterPlayer.Instance.enabled)
                    continue;
                
                for (int i = 0; i < datas.Count; i++)
                {
                    yield return null;
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

                        yield return null;
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
                yield return new WaitForSeconds(1f / 20f);
            }
            
        }


        private void OnDestroy()
        {
            datas.Clear();
        }
    }
}
