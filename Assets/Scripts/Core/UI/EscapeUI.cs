using System.Collections;
using System.Collections.Generic;
using Core.Game;
using Core.Garage;
using Core.PlayerScripts;
using Core.Systems;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class EscapeUI : MonoBehaviour
    {
        [SerializeField] private float min, max;
        [SerializeField] private RectTransform holder;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image background, pausePlay;
        [SerializeField] private Sprite playS, pauseS;
        [SerializeField] private GameObject selfDestructionButton;
        [SerializeField] private bool isPause;

        private void Awake()
        {
            holder.anchoredPosition = new Vector2(min, 0);
            background.DOFade(0, 0);
            background.raycastTarget = false;
            pausePlay.transform.localScale = Vector3.one * 3;
            pausePlay.DOFade(0, 0);
            canvas.enabled = false;
        }

        private const float speed = 0.3f;
        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && World.Scene != Scenes.Init)
            {
                isPause = !isPause;
                SetPause(isPause);
            }
        }

        public void SetPause(bool pause)
        {
            StopAllCoroutines();
            isPause = pause;
            if (isPause)
            {
                canvas.enabled = true;
                selfDestructionButton.SetActive(Player.inst != null);

                background.raycastTarget = true;
                background.DOFade(0.95f, speed).SetUpdate(true);
                holder.DOAnchorPosX(max, speed).SetUpdate(true);
                pausePlay.transform.DOScale(Vector3.one, speed).SetUpdate(true);
                pausePlay.DOFade(1, speed).SetUpdate(true);
                pausePlay.sprite = pauseS;
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                background.raycastTarget = false;
                background.DOFade(0, speed).SetUpdate(true);
                holder.DOAnchorPosX(min, speed).SetUpdate(true);
                pausePlay.transform.DOScale(Vector3.one * 3, speed).SetUpdate(true);
                pausePlay.DOFade(0, speed/2f).SetUpdate(true);
                pausePlay.sprite = playS;
                StartCoroutine(CanvasDisabler());
                Time.timeScale = 1;
            }
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        IEnumerator CanvasDisabler()
        {
            yield return new WaitForSeconds(speed);
            canvas.enabled = false;
        }

        public void SelfDestruct()
        {
            if (Player.inst != null)
            {
                Player.inst.TakeDamageHeath(Mathf.Infinity);
                PlayerDataManager.SaveAll();
                SetPause(false);
            }
        }

        public void GoToMenu()
        {
            PlayerDataManager.SaveAll();
            if (Player.inst != null)
            {
                Destroy(Player.inst.gameObject);
            }
            SetPause(false);
            World.LoadLevel(Scenes.Init);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
