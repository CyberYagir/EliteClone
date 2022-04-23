using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EffectsDisabler
{
    public class EffectsDisabler : ModInit
    {
        private bool mustAddEvent;

        private void LateUpdate()
        {
            if (Player.inst != null)
                this.mustAddEvent = true;
            if (!this.mustAddEvent || Player.inst != null)
                return;
            // ISSUE: method pointer
            Player.OnSceneChanged += OnSceneChanges;
            this.mustAddEvent = false;
            this.OnSceneChanges();
        }

        public void OnSceneChanges()
        {
            VolumeProfile sharedProfile = FindObjectOfType<Volume>().sharedProfile;
            sharedProfile.components.Add((VolumeComponent) ScriptableObject.CreateInstance<Bloom>());
            sharedProfile.components.Add((VolumeComponent) ScriptableObject.CreateInstance<AmbientOcclusion>());
            sharedProfile.components.Add((VolumeComponent) ScriptableObject.CreateInstance<MotionBlur>());
            
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
        }
    }
}
