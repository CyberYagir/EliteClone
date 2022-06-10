using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class AlphaController : MonoBehaviour
    {
        [System.Serializable]
        public partial class Box
        {
            public Vector3 pos, size;
        }

        public List<Box> casts;

        public bool enable = true;

        public List<Transform> rooms;

        public List<RoomData> datas;


        private RoomData current;
        
        public bool overrideActive;
        
        [System.Serializable]
        public class RoomData
        {
            private MeshRenderer[] renderers;
            private Light[] lights;
            private Transform obj;

            private AlphaController controller;
            
            [SerializeField] private bool stateActive = true;
            public RoomData(Transform holder)
            {
                obj = holder;
                controller = obj.GetComponent<AlphaController>();
                renderers = holder.GetComponentsInChildren<MeshRenderer>();
                lights = holder.GetComponentsInChildren<Light>();
            }

            public bool IsOverride()
            {
                if (controller == null)
                {
                    return false;
                }
                else
                {
                    return controller.overrideActive;
                }
            }

            public void Active(bool state, bool force = false)
            {
                stateActive = state;
                foreach (var n in renderers)
                {
                    n.enabled = state;
                }

                foreach (var l in lights)
                {
                    l.enabled = state;
                }
            }
        }


        [SerializeField] private bool workOnStart;
        private void Start()
        {
            foreach (var room in rooms)
            {
                datas.Add(new RoomData(room));
            }

            current = new RoomData(transform);
            
            if (workOnStart)
            {
                enable = false;
                SetState(true);
            }
        }

        public List<Transform> list;
        private RaycastHit[] hits = new RaycastHit[10];

        private void FixedUpdate()
        {
            enable = true;
            list.Clear();;
            for (int i = 0; i < casts.Count; i++)
            {
                var size = Physics.BoxCastNonAlloc(transform.position + casts[i].pos, casts[i].size / 2f, Vector3.down, hits, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("Main"), QueryTriggerInteraction.Ignore);
                if (size != 0)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (hits[j].transform != null && (hits[j].transform.TryGetComponent(out ShooterPlayer rb) || hits[j].transform.GetComponentInParent<ShooterPlayer>()))
                        {
                            list.Add(hits[j].transform);
                            enable = false;
                        }
                    }
                }
            }

            if (!enable)
            {
                SetState(false);
            }

            if (overrideActive)
            {
                current.Active(true, true);
            }
        }

        public void SetState(bool force)
        {
            for (int j = 0; j < datas.Count; j++)
            {
                if (!datas[j].IsOverride())
                {
                    datas[j].Active(enable, force);
                }
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < casts.Count; i++)
            {
                Gizmos.DrawWireCube((transform.position + casts[i].pos), casts[i].size);
            }
        }
    }
}
