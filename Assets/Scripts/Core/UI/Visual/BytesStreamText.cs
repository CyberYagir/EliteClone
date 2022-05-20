using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.UI.Visuals
{
    public class BytesStreamText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private string data;
        private string keyStirng = "123456890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm_";
        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                data += keyStirng[Random.Range(0, keyStirng.Length)];
                if (data.Length > 22 * 4)
                {
                    data = data.Remove(0, 1);
                }

                text.text = data;
            }
        }
    }
}
