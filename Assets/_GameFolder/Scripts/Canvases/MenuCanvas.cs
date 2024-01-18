using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;
using DG.Tweening;

namespace FishingIsland.Canvases
{
    public class MenuCanvas : MonoBehaviour
    {
        private GameManager _gameManager;

        [Header("Button")]
        public Button playButton;

        [Header("Images")]
        public Image characterSelectionImage;
        public Image menuBackgroundImage;

        public void Initialize(GameManager gameManager )
		{
            _gameManager = gameManager;
        }

		private void OnEnable()
		{
            GameManager.OnMenuOpen += OnMenuOpen;
            GameManager.OnGameStarted += OnGameStart;
		}

		private void OnDisable()
		{
            GameManager.OnMenuOpen -= OnMenuOpen;
            GameManager.OnGameStarted -= OnGameStart;			
		}

		private void OnMenuOpen()
		{
            menuBackgroundImage.gameObject.SetActive(true);
            DOVirtual.DelayedCall(1.5f, () =>
            {
                _gameManager.OnGameStart();
            });
        }

        private void OnGameStart()
		{
            menuBackgroundImage.gameObject.SetActive(false);
		}     
    }
}

