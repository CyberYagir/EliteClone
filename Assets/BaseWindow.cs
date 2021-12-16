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

    private void Start()
    {
        if (World.Scene == Scenes.Location)
        {
            Init();
        }
        else
        {
            Player.OnSceneChanged += Init;
            gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
        nameText.text = WorldOrbitalStation.Instance.transform.name;
        Player.inst.GetComponent<LandManager>().OnLand += RedrawAll;
        Player.OnSceneChanged -= Init;
    }
    
    private void Update()
    {
        Animation();
        if (Player.inst.land.isLanded)
        {
            var date = DateTime.Now.Date.AddYears(1025);    
            infoText.text = $"Date: {date:d}\n" +
                            $"Time: {DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00")}\n" +
                            $"Credits: Empty";
        }
    }

    public void RedrawAll()
    {
        characters.UpdateList();
    }
    
    
    public IconType GetIcon(Fraction fraction)
    {
        return icons.Find(x => x.fraction == fraction);
    }
    public void Animation()
    {
        rect.sizeDelta = Vector2.Lerp(rect.sizeDelta,  new Vector2(rect.sizeDelta.x, Player.inst.land.isLanded ? height : 0), 5 * Time.deltaTime);
    }
}
