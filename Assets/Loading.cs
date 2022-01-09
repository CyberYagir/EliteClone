using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Transform loading;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Load());
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Load()
    { 
        AsyncOperation loadingOperation = World.LoadLevelAsync(Scenes.Init);
        while (!loadingOperation.isDone)
        {
            yield return new WaitForSeconds(2);
        }

        
        yield return new WaitForSeconds(2);
        loading.DOLocalMove(new Vector3(-2048, 0, 0), 1f);
        while (image.color.a > 0.01f)
        {
            image.color = Color.Lerp(image.color, new Color(0, 0, 0, 0), Time.deltaTime * 3);

            if (image.color.a < 0.4f)
            {
                image.raycastTarget = false;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
