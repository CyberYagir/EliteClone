using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticlesRemover
{
    public class ParticlesRemoverIniter : ModInit
    {
        private bool mustAddEvent;

        private void LateUpdate()
        {
            if (Player.inst == null)
            {
                mustAddEvent = true;
            }

            if (mustAddEvent)
            {
                if (Player.inst != null)
                {
                    Player.OnSceneChanged += OnSceneChanges;
                    mustAddEvent = false;
                    OnSceneChanges();
                }
            }
        }

        public void OnSceneChanges()
        {
            if (World.Scene == Scenes.System || World.Scene == Scenes.Location)
            {
                var suns = FindObjectsOfType<SunTexture>();
                foreach (var sun in suns)
                {
                    var particles = sun.GetComponentsInChildren<ParticleSystem>();
                    for (int i = 0; i < particles.Length; i++)
                    {
                        particles[i].gameObject.SetActive(false);
                    }
                }
            }

            StartCoroutine(WaitForBotSpawn());
        }

        IEnumerator WaitForBotSpawn()
        {
            yield return null;
            if (World.Scene == Scenes.Location)
            {
                var particles = FindObjectsOfType<EngineParticles>();
                foreach (var particle in particles)
                {
                    particle.gameObject.SetActive(false);
                }
            }
        }
    }
}
