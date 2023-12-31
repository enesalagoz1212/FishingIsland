using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

namespace FishingIsland.Controllers
{
	public enum BoatState
	{
		InThePort,
		GoingFishing,
		Fishing,
		ReturningToPort,
	}
	public class BoatController : MonoBehaviour
	{
		public BoatState BoatState { get; private set; }
		private Vector3 _initialPosition;

		private int _maxFishCapacity = 10;
		public int FishCount { get; private  set; }

		public TextMeshProUGUI timerText;
		private bool _isTimerRunning = false;
		private float _currentTime = 8;
		public TextMeshProUGUI fishCapacityText;
		public GameObject dockTimerPanel;
		public TextMeshProUGUI boxFishText;

		private bool _canClick = true;
		public Image boatDownOkImage;
		public GameObject boatFishPanel;
		public void Initialize()
		{
			_initialPosition = transform.position;
			ChangeState(BoatState.InThePort);
		}

		private void Awake()
		{
			boatDownOkImage.gameObject.SetActive(true);
		}

		private void Start()
		{
			
		}

		public void ChangeState(BoatState boatState)
		{
			BoatState = boatState;
			Debug.Log($"BoatState: {boatState}");
			if (GameManager.Instance.GameState == GameState.Playing)
			{
				switch (BoatState)
				{
					case BoatState.InThePort:
						transform.position = _initialPosition;
						boatDownOkImage.gameObject.SetActive(true);
						boatFishPanel.gameObject.SetActive(false);
						_canClick = true;
						break;
					case BoatState.GoingFishing:			
						_currentTime = 8f;
						dockTimerPanel.gameObject.SetActive(true);
						_isTimerRunning = true;

						Transform randomSpawnPoint = LevelManager.Instance.GetRandomBoatSpawnPoint();
						if (randomSpawnPoint != null)
						{
							MoveToPosition(randomSpawnPoint.position, 2f);					
						}
						DOVirtual.DelayedCall(3f, () =>
						{
							ChangeState(BoatState.Fishing);
						});
						break;
					case BoatState.Fishing:
						UpdateCollectTheFishText();
						break;
					case BoatState.ReturningToPort:
						MoveToPosition(_initialPosition, 1f);
						_isTimerRunning = false;

						DOVirtual.DelayedCall(1f, () =>
						{
							dockTimerPanel.gameObject.SetActive(false);
							FishBoxController.OnBoatArrivedBox?.Invoke(this);
						});
						break;
				}

			}
		}

		private void Update()
		{
			TimerControl();
		}

		private void MoveToPosition(Vector3 targetPosition, float duration )
		{
			transform.DOMove(targetPosition, duration);
			

		}

		private void OnMouseDown()
		{
			if (_canClick)
			{
				Debug.Log("OnMouseDown");
				ChangeState(BoatState.GoingFishing);
				boatDownOkImage.gameObject.SetActive(false);
				_canClick = false;
			}	
		}

		public void OnFishTransferredToFishBox()
		{
			FishCount--;
			UpdateFishCapacityText();

			if(FishCount <= 0)
			{
				ChangeState(BoatState.InThePort);
			}
		}

		private void UpdateFishCapacityText()
		{
			fishCapacityText.text = $" {FishCount}";
		}


		private void UpdateCollectTheFishText()
		{
			StartCoroutine(CollectFish());
		}

		private void TimerControl()
		{
			if (_isTimerRunning && _currentTime > 0)
			{
				_currentTime -= Time.deltaTime;
				timerText.text = ((int)_currentTime).ToString();
			}
			if (_isTimerRunning && _currentTime <= 1)
			{
				ChangeState(BoatState.ReturningToPort);
			}
		}

		private IEnumerator CollectFish()
		{
			boatFishPanel.SetActive(true);
			for (int i = 0; i < _maxFishCapacity; i++)
			{
				yield return new WaitForSeconds(0.3f);
				FishCount++;
				UpdateFishCapacityText();
			}
		}
	}
}
