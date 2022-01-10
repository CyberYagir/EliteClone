using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    [System.Serializable]
    public class IconType
    {
        public Sprite icon;
        public Fraction fraction;
    }
    private RectTransform rect;
    [SerializeField] private float height = 1400;
    [SerializeField] private TMP_Text infoText, nameText;
    [SerializeField] private List<IconType> icons;
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
        Player.inst.land.OnLand += RedrawAll;
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
        repairT.text = "Repair: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Curpus) * (Player.inst.Ship().hp.max - Player.inst.Ship().hp.value));
        fuelT.text = "Fuel: " + (StationRefiller.Instance.GetRefillerValue(StationRefiller.Refiller.RefillType.Fuel) * (Player.inst.Ship().fuel.max - Player.inst.Ship().fuel.value));
    }
    private void Update()
    {
        Animation();
        if (Player.inst.land.isLanded)
        {
            var date = DateTime.Now.Date.AddYears(1025);    
            infoText.text = $"Date: {date:d}\n" +
                            $"Time: {DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00")}\n" +
                            $"Credits: " + Player.inst.cargo.GetCredits();
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


    public IconType GetIcon(Fraction fraction)
    {
        return icons.Find(x => x.fraction == fraction);
    }

    public void Animation()
    {
        if (rect)
        {
            rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(rect.sizeDelta.x, Player.inst.land.isLanded ? height : 0), 5 * Time.deltaTime);
        }
    }

    public void Refill(int type)
    {
        StationRefiller.Instance.Fill((StationRefiller.Refiller.RefillType)type);
        RedrawAll(); 
    }
}
