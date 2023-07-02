using System.Collections;
using Core;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EffectsDisabler
{
    public class EffectsDisabler : ModInit
    {
        private bool mustAddEvent;
        private bool isadded;
        private void LateUpdate()
        {
            var player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            if (player != null)
                mustAddEvent = true;
            if (mustAddEvent && player != null && !isadded)
            {
                Player.OnSceneChanged += OnSceneChanges;
                mustAddEvent = false;
                isadded = true;
                OnSceneChanges();
            }

        }

        public void OnSceneChanges()
        {
            VolumeProfile sharedProfile = FindObjectOfType<Volume>().sharedProfile;
            sharedProfile.components.Add(ScriptableObject.CreateInstance<Bloom>());
            sharedProfile.components.Add(ScriptableObject.CreateInstance<AmbientOcclusion>());
            sharedProfile.components.Add(ScriptableObject.CreateInstance<MotionBlur>());
            
            if (sharedProfile.TryGet(out Bloom bloom))
            {
                bloom.intensity.value = 0.0f;
                bloom.intensity.overrideState = true;
            }
            if (sharedProfile.TryGet(out AmbientOcclusion ambientOcclusion))
            {
                ambientOcclusion.intensity.value = 0.0f;
                ambientOcclusion.intensity.overrideState = true;
            }

            if (!sharedProfile.TryGet(out MotionBlur motionBlur))
            {
                motionBlur.intensity.value = 0.0f;
                 motionBlur.intensity.overrideState = true;
            }

            if (!sharedProfile.TryGet(out IndirectLightingController indirect))
            {
                if (indirect != null)
                {
                    indirect.reflectionLightingMultiplier.value = 0;
                }
            }


            StartCoroutine(WaitToDisable());
        }

        IEnumerator WaitToDisable()
        {
            yield return null;
            yield return null;
            yield return null;
            var particles = FindObjectsOfType<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.gameObject.SetActive(false);
            }

            var lines = FindObjectsOfType<RotateAround>();
            foreach (var line in lines)
            {
                line.enabled = false;
                line.GetComponentInChildren<LineRenderer>()?.gameObject.SetActive(false);
            }

            var lights = FindObjectsOfType<HDAdditionalLightData>();

            foreach (var light in lights)
            {
                light.EnableShadows(false);
            }
        }
    }
}
