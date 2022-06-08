using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class AlphaController : MonoBehaviour
    {
        [SerializeField] private List<Renderer> renderesList;
        [Range(0, 1f)]
        [SerializeField] private float alpha = 1;

        private List<List<Material>> batchedMaterials = new List<List<Material>>();
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        private void Start()
        {
            foreach (var rn in renderesList)
            {
                batchedMaterials.Add(rn.sharedMaterials.ToList());
            }
        }

        public void StartFade()
        {
            if (alpha == 1f)
            {
                StopAllCoroutines();
                StartCoroutine(Fade());
            }
        }

        public void DisableFade()
        {
            StopAllCoroutines();
            StartCoroutine(UnFade());
        }

        IEnumerator Fade()
        {
            while (alpha > 0.05f)
            {
                alpha -= Time.deltaTime * 5f;
                for (int i = 0; i < renderesList.Count; i++)
                {
                    var mats = renderesList[i].materials;
                    for (int j = 0; j < mats.Length; j++)
                    {
                        var color = mats[j].GetColor(BaseColor);
                        mats[j].SetColor(BaseColor, new Color(color.r, color.g, color.b, alpha));
                    }
                    renderesList[i].materials = mats;
                }
                yield return null;
            }
        }

        IEnumerator UnFade()
        {
            while (alpha < 1f)
            {
                alpha += Time.deltaTime * 5f;
                for (int i = 0; i < renderesList.Count; i++)
                {
                    var mats = renderesList[i].materials;
                    for (int j = 0; j < mats.Length; j++)
                    {
                        var color = mats[j].GetColor(BaseColor);
                        mats[j].SetColor(BaseColor, new Color(color.r, color.g, color.b, alpha));
                    }
                    renderesList[i].materials = mats;
                }

                yield return null;
            }
            
            for (int i = 0; i < renderesList.Count; i++)
            {
                var mats = batchedMaterials[i].ToArray();
            }

            alpha = 1;
        }
        
    }
}
