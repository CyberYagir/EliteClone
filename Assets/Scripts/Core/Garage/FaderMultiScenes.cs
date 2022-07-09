using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Garage
{
    public class FaderMultiScenes : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Scenes scene = Scenes.Location;

        public void LoadLocation()
        {
            if (GarageDataCollect.Instance != null)
            {
                GarageDataCollect.Instance?.Save();
            }
            LoadScene(Scenes.Location);
        }
    
        public void LoadScene(Scenes scene)
        {
            this.scene = scene;
            StartCoroutine(LoadAsync());
        }

        IEnumerator LoadAsync()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            image.DOFade(1, 1f);
            yield return new WaitForSeconds(1);
            var process = World.LoadLevelAsync(scene);

            while (!process.isDone)
            {
                yield return null;
            }

            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }
        
            image.DOFade(0, 1f);
            yield return new WaitForSeconds(1);
        
            Destroy(gameObject);
        }

        public void SetScene(int i)
        {
            scene = (Scenes) i;
        }
    }
}
