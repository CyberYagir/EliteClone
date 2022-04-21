using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.Map
{
    public class MapPathUI : MonoBehaviour
    {
        public enum PathType
        {
            None, FirstPoint, SecondPoint
        }

        public PathType type;

        [SerializeField] private GameObject cancel;
        [SerializeField] private TMP_Text text;
        
        public void Next()
        {
            if (type == PathType.None)
            {
                text.text = "Select First";
                cancel.SetActive(true);
                type = PathType.FirstPoint;
            }else if (type == PathType.FirstPoint)
            {
                text.text = "Select Second";
                MapPathfinder.Instance.SetStartPath(MapSelect.selected.name);
                type = PathType.SecondPoint;
            }else if (type == PathType.SecondPoint)
            {
                text.text = "Path";
                MapPathfinder.Instance.SetEndPath(MapSelect.selected.name);
                MapPathfinder.Instance.FindPath();
                type = PathType.None;
                
                cancel.SetActive(false);
            }
        }

        public void Cancel()
        {
            type = PathType.None;
            text.text = "Path";
            cancel.SetActive(false);
        }
    }
}
