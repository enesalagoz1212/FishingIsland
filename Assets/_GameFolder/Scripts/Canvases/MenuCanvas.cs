using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;

namespace FishingIsland.Canvases
{
    public class MenuCanvas : MonoBehaviour
    {
        private GameManager _gameManager;
        public Button playButton;
        public Image menuBackgroundImage;

        public void Initialize(GameManager gameManager)
		{
            _gameManager = gameManager;
            playButton.onClick.AddListener(OnPlayButtonClicked);
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
		}

        private void OnGameStart()
		{
            menuBackgroundImage.gameObject.SetActive(false);
		}

        private void OnPlayButtonClicked()
		{
            _gameManager.OnGameStart();
		}
    }
}

