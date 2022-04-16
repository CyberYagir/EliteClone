using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReputationManager))]
public class ReputationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (ReputationManager.Instance != null)
        {
            if (ReputationManager.Instance.ratings == null)
            {
                ReputationManager.Instance.ratings = new Dictionary<string, int>();
            }
            foreach (var ratings in ReputationManager.Instance.ratings)
            {
                GUILayout.Label(ratings.Key + ": " + ratings.Value);
            }
        }
        else
        {
            GUILayout.Label("Start game to see reputations");
        }
    }
}
