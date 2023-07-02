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

        private void Start()
        {
            Init();
            if (World.Scene != Scenes.Location)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (WorldOrbitalStation.Instance != null)
                {
                    ChangeUI();
                }
            }
        }

        public void Init()
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.land.OnLand += RedrawAll;
            rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
        }

        public void ChangeUI()
        {
            nameText.text = WorldOrbitalStation.Instance.transform.name;
            UpdateCosts();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Animation();
            if (PlayerDataManager.Instance.WorldHandler.ShipPlayer.land.isLanded)
            {
                var date = DateTime.Now.Date.AddYears(1025);    
                infoText.text = $"Date: {date:d}\n" +
                                $"Time: {DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00")}\n" +
                                "Credits: " + PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.GetCredits();
            }

        }

        public void UpdateCosts()
        {
            repairT.text = "Repair: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Curpus) * (PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Health).max - PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Health).value));
            fuelT.text = "Fuel: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Fuel) * (PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Fuel).max - PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Fuel).value));
        }

        public void RedrawAll()
        {
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
                rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(rect.sizeDelta.x, PlayerDataManager.Instance.WorldHandler.ShipPlayer.land.isLanded ? height : 0), 5 * Time.deltaTime);
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
