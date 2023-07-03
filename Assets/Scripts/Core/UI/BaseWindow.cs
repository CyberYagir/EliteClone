    using System;
using Core.Location;
using Core.PlayerScripts;
using TMPro;
using UnityEngine;
using static Core.Game.ItemShip.ShipValuesTypes;

namespace Core.UI
{
    public class BaseWindow : MonoUI
    {
        private RectTransform rect;
        [SerializeField] private float height = 1400;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text infoText, nameText;
        [SerializeField] private CharacterList characters;
        [SerializeField] private TMP_Text repairT, fuelT;
        private Player player;

        public override void Init()
        {
            base.Init();
            
            player = WorldDataHandler.ShipPlayer;
            
            BaseInit();
            if (World.Scene != Scenes.Location)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (WorldDataHandler.CurrentLocationGenerator.CurrentLocationData.OrbitStation != null)
                {
                    ChangeUI();
                }
            }
        }


        public void BaseInit()
        {
            player.land.OnLand += RedrawAll;
            
            rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
            RedrawAll();
        }

        public void ChangeUI()
        {
            nameText.text = WorldDataHandler.CurrentLocationGenerator.CurrentLocationData.OrbitStation.transform.name;
            UpdateCosts();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Animation();
            if (player.land.isLanded)
            {
                var date = DateTime.Now.Date.AddYears(1025);    
                infoText.text = $"Date: {date:d}\n" +
                                $"Time: {DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00")}\n" +
                                "Credits: " + player.cargo.GetCredits();
            }

        }

        public void UpdateCosts()
        {
            repairT.text = "Repair: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Curpus) * (player.Ship().GetValue(Health).max - player.Ship().GetValue(Health).value));
            fuelT.text = "Fuel: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Fuel) * (player.Ship().GetValue(Fuel).max - player.Ship().GetValue(Fuel).value));
        }

        public void RedrawAll()
        {
            if (LocationGenerator.CurrentSave == null) return;
            if (LocationGenerator.CurrentSave.type == LocationPoint.LocationType.Station)
            {
                ChangeUI();
                characters.UpdateList();
            }
        }
        
        public void Animation()
        {
            if (rect)
            {
                rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(rect.sizeDelta.x, player.land.isLanded ? height : 0), 5 * Time.deltaTime);
                canvas.enabled = rect.sizeDelta.y >= 0.1f;
            }
        }

        public void Refill(int type)
        {
            StationRefiller.Instance.Fill((StationRefiller.Refiller.RefillType)type);
            ChangeUI(); 
        }
    }
}
