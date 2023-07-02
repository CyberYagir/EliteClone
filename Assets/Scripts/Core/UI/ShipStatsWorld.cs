using System;
using System.Collections;
using System.Collections.Generic;
using Core.Game;
using Core.PlayerScripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Core.Game.ItemShip.ShipValuesTypes;

namespace Core.UI
{
    public class ShipStatsWorld : MonoUI
    {
        [System.Serializable]
        public class DisplayerStat
        {
            [SerializeField] TMP_Text text;
            [SerializeField] Image fillImage;
            [SerializeField] Image icon;
            [SerializeField] private bool isActived;
            public void Active()
            {
                isActived = true;
                
                
                fillImage?.DOFade(1, 0.5f);
                fillImage?.DOFillAmount(1, 0.5f);
                text?.DOFade(1, 0.5f);
                icon?.DOFade(1, 0.5f);
            }

            public void Disable()
            {
                isActived = false;
                fillImage?.DOFade(0, 0.5f);
                fillImage?.DOFillAmount(0, 0.5f);
                icon?.DOFade(0.1f, 0.5f);
                if (text != null)
                {
                    text.DOFade(0.5f, 0.5f);
                    text.text = "";
                }
            }

            public void SetValue(float val)
            {
                if (isActived && fillImage)
                {
                    fillImage.fillAmount = val;
                }
            }

            public void SetText(string val)
            {
                if (isActived && text)
                {
                    text.text = val;
                }
            }

            public void SetAlpha(Image image, float alpha)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }
        }

        [SerializeField] private DisplayerStat speed;
        [SerializeField] private DisplayerStat warpSpeed;
        [SerializeField] private DisplayerStat temperature;
        [SerializeField] private DisplayerStat fuel;
        [SerializeField] private DisplayerStat cargo;
        [SerializeField] private DisplayerStat health;
        [SerializeField] private DisplayerStat shield;
        
        
        
        private ShipController controller;
        private bool isCanWarp;
        
        
        private void Start()
        {
            controller = PlayerDataManager.Instance.WorldHandler.ShipPlayer.control;
            speed.Active();
            warpSpeed.Disable();
            temperature.Active();
            fuel.Active();
            cargo.Active();
            health.Active();
            shield.Active();
            
            
            controller.warp.OnChangeWarp += OnWarpOnOff;
        }
        
        public override void OnUpdate()
        {
            speed.SetValue(controller.speedPercent);
            speed.SetText((controller.speed * World.unitSize).ToString("F2"));

            warpSpeed.SetValue(controller.warp.warpSpeed/controller.warp.maxWarpSpeed);
            warpSpeed.SetText((controller.warp.warpSpeed * World.unitSize).ToString("F2"));

            var temperatureVal = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Temperature);
            temperature.SetValue(temperatureVal.percent);
            temperature.SetText((int)(temperatureVal.value * 50) + " K");
            
            
            var fuelVal = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Fuel);
            fuel.SetValue(fuelVal.percent);
            fuel.SetText((int)(fuelVal.value) + " t.");
            
            
            
            var cargoVal = PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.tons/PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().data.maxCargoWeight;
            cargo.SetValue(cargoVal);
            cargo.SetText((int)(PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.tons) + " t.");
            
            var healthVal = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Health);
            health.SetValue(healthVal.percent);
            health.SetText((int)(healthVal.value) + " %");
            
            
            var shieldVal = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Shields);
            shield.SetValue(shieldVal.percent);
            shield.SetText((int)(shieldVal.value) + " %");

            CheckWarp();
            base.OnUpdate();
        }


        public void CheckWarp()
        {
            if (controller.speed > WarpManager.speedToWarp && isCanWarp == false)
            {
                warpSpeed.Active();
                isCanWarp = true;
            }else
            if (controller.speed <= WarpManager.speedToWarp && isCanWarp == true)
            {
                warpSpeed.Disable();
                isCanWarp = false;
            }
        }

        public void OnWarpOnOff(bool state)
        {
            if (!state)
            {
                speed.Active();
            }
            else
            {
                speed.Disable();
            }
        }
    }
}
