using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Garage
{
    public class FaderMultiScenes : MonoBehaviour
    {
        [SerializeField] private Image image;


        public void LoadLocation()
        {
            GarageDataCollect.Instance.Save();
            LoadScene(Scenes.Location);
        }
    
        public void LoadScene(Scenes scene)
        {
            StartCoroutine(LoadAsync());
        }

        IEnumerator LoadAsync()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            image.DOFade(1, 1f);
            yield return new WaitForSeconds(1);
            var process = World.LoadLevelAsync(Scenes.Location);

            while (!process.isDone)
            {
                yield return null;
            }
        
            image.DOFade(1, 0f);
            yield return new WaitForSeconds(1);
        
            Destroy(gameObject);
        }
    }
}