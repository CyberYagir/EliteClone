using Core.PlayerScripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class DisplayInfoText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private Camera camera;
        private LayerMask mask;
        private void Start()
        {
            camera = Player.inst.GetCamera();
            mask = LayerMask.GetMask("Main");
        }

        private void LateUpdate()
        {
            if (World.Scene == Scenes.Location)
            {
                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 500, mask, QueryTriggerInteraction.Ignore))
                {
                    var data = hit.transform.GetComponent<DataToDisplay>();
                    if (data)
                    {
                        text.text = data.GetText();
                        text.transform.position = camera.WorldToScreenPoint(data.transform.position);
                        text.transform.position = new Vector3(text.transform.position.x, text.transform.position.y);

                        if (text.transform.localScale.magnitude <= 1f)
                        {
                            text.transform.DOScale(Vector3.one, 0.1f);
                        }
                        return;
                    }
                }
            }
            if (text.transform.localScale.magnitude > 0)
            {
                text.transform.DOScale(Vector3.zero, 0.1f);
            }
        }
    }
}
