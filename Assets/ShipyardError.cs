using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipyardError : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public void ThrowError(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
    }
}
