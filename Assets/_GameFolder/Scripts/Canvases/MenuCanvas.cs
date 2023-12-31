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
        private CharacterManager _characterManager;

        public Button playButton;

        public Image characterSelectionImage;
        public Image menuBackgroundImage;

        private bool _isMaleSelected;

        public void Initialize(GameManager gameManager,CharacterManager characterManager)
		{
            _gameManager = gameManager;
            _characterManager = characterManager;

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

