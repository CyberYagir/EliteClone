using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class ReputationManager : MonoBehaviour
    {
        public static ReputationManager Instance;
        public Dictionary<string, int> ratings = new Dictionary<string, int>();
        public Event OnChangeRating = new Event();
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void AddRating(int fractionID, int scores)
        {
            var fractionName = WorldDataItem.Fractions.NameByID(fractionID);
            AddRating(fractionName, scores);
        }

        public void AddRating(string fractionName, int scores)
        {
            if (ratings == null)
                ratings = new Dictionary<string, int>();
            if (!ratings.ContainsKey(fractionName))
            {
                ratings.Add(fractionName, 0);
            }
            ratings[fractionName] += scores;
            OnChangeRating.Run();
        }

        public int GetMax()
        {
            if (ratings == null)
                ratings = new Dictionary<string, int>();
            var max = 0;

            foreach (var rt in ratings)
            {
                if (rt.Value > max)
                {
                    max = rt.Value;
                }
            }

            return max;
        }
    }
}
