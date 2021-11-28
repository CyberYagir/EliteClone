using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPShieldsCanvas : MonoBehaviour
{
    [System.Serializable]
    public class Bar
    {
        public Image img;
        public TMP_Text text;
    }
    public Bar hp, shield;
    private void Update()
    {
        hp.img.fillAmount = (Player.inst.Ship().hp.value / Player.inst.Ship().hp.max);
        hp.text.text = "Corpus " + (int)(hp.img.fillAmount * 100) + "%";
        shield.img.fillAmount = (Player.inst.Ship().shields.value / Player.inst.Ship().shields.max);
        shield.text.text = "Shields " + (int)(shield.img.fillAmount * 100) + "%";
    }
}
