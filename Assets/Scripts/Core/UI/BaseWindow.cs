using System;
using System.Collections.Generic;
using Core.Location;
using Core.Systems;
using TMPro;
using UnityEngine;
using static Core.Game.ItemShip.ShipValuesTypes;

namespace Core.UI
{
    public class BaseWindow : MonoBehaviour
    {
        [System.Serializable]
        public class IconType
        {
            public Sprite icon;
            public int fraction;
        }
        private RectTransform rect;
        [SerializeField] private float height = 1400;
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
            Player.Player.inst.land.OnLand += RedrawAll;
            rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
        }

        public void ChangeUI()
        {
            nameText.text = WorldOrbitalStation.Instance.transform.name;
            UpdateCosts();
        }

        public void UpdateCosts()
        {
            repairT.text = "Repair: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Curpus) * (Player.Player.inst.Ship().GetValue(Health).max - Player.Player.inst.Ship().GetValue(Health).value));
            fuelT.text = "Fuel: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Fuel) * (Player.Player.inst.Ship().GetValue(Fuel).max - Player.Player.inst.Ship().GetValue(Fuel).value));
        }
        private void Update()
        {
            Animation();
            if (Player.Player.inst.land.isLanded)
            {
                var date = DateTime.Now.Date.AddYears(1025);    
                infoText.text = $"Date: {date:d}\n" +
                                $"Time: {DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00")}\n" +
                                $"Credits: " + Player.Player.inst.cargo.GetCredits();
            }
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
                rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(rect.sizeDelta.x, Player.Player.inst.land.isLanded ? height : 0), 5 * Time.deltaTime);
            }
        }

        public void Refill(int type)
        {
            StationRefiller.Instance.Fill((StationRefiller.Refiller.RefillType)type);
            ChangeUI(); 
        }
    }
}
