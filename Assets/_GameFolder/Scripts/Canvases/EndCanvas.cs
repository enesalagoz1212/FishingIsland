using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;

namespace FishingIsland.Canvases
{
	public class EndCanvas : MonoBehaviour
	{
		public Image levelCompleteImage;
		public Button nextButton;

		public void Initialize()
		{
			Debug.Log("EndCanvas");
			nextButton.onClick.AddListener(NextButton);
		}

		private void OnEnable()
		{
			GameManager.OnGameLevelCompleted += OnGameLevelCompleteAction;
		}

		private void OnDisable()
		{
			GameManager.OnGameLevelCompleted -= OnGameLevelCompleteAction;

		}

		private void OnGameLevelCompleteAction(bool isTheLevelCompleted)
		{
			if (isTheLevelCompleted)
			{
				levelCompleteImage.gameObject.SetActive(true);
			}
		}

		private void NextButton()
		{
			Debug.Log("Next button clicked");
			GameManager.OnGameReset?.Invoke();
			levelCompleteImage.gameObject.SetActive(false);
		}
	}

}

