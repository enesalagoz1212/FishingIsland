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
        private CharacterManager _characterManager;

        public Button playButton;
        public Button maleButton;
        public Button femaleButton;
        public Image characterSelectionImage;
        public Image menuBackgroundImage;

        private bool _isMaleSelected;

        public void Initialize(GameManager gameManager,CharacterManager characterManager)
		{
            _gameManager = gameManager;
            _characterManager = characterManager;

            playButton.onClick.AddListener(OnPlayButtonClicked);
            maleButton.onClick.AddListener(OnMaleButtonClicked);
            femaleButton.onClick.AddListener(OnFemaleButtonClicked);
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
            characterSelectionImage.gameObject.SetActive(true);
		}

        private void OnGameStart()
		{
            menuBackgroundImage.gameObject.SetActive(false);
		}

        private void OnPlayButtonClicked()
		{
			if (_isMaleSelected)
			{
                _characterManager.InstantiateCharacter(_isMaleSelected);
			}
			else
			{
                _characterManager.InstantiateCharacter(_isMaleSelected);
			}
            _gameManager.OnGameStart();
		}

        private void OnMaleButtonClicked()
		{
            Debug.Log("male");
            _isMaleSelected = true;
            menuBackgroundImage.gameObject.SetActive(true);
            characterSelectionImage.gameObject.SetActive(false);
        }

        private void OnFemaleButtonClicked()
		{
            Debug.Log("female");
            _isMaleSelected = false;
            menuBackgroundImage.gameObject.SetActive(true);
            characterSelectionImage.gameObject.SetActive(false);
		}
    }
}

