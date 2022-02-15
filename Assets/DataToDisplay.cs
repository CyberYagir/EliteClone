using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataToDisplay : MonoBehaviour
{
    [System.Serializable]
    public class DataLabel
    {
        public string name, info;
    }

    public List<DataLabel> lables { get; private set; } = new List<DataLabel>();

    public void AddLabel(string name, string label)
    {
        lables.Add(new DataLabel() {name = name, info = label});
    }

    public string GetText()
    {
        string data = "";
        for (int i = 0; i < lables.Count; i++)
        {
            if (i != 0)
            {
                data += "<color=#FF7100>|</color>";
            }
            data += lables[i].name + ": " + lables[i].info + "\n";
        }

        return data;
    }
}
