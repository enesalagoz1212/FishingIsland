using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

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
		private int _fishCapacity = 0;
		private int _maxFishCapacity = 10;

		public Image countDown;
		public TextMeshProUGUI timerText;
		private bool _isTimerRunning = false;
		private float _currentTime = 8;
		public TextMeshProUGUI fishCapacityText;
		public GameObject dockTimerPanel;
		public TextMeshProUGUI boxFishText;
		private int _totalFishCount = 0;
		public void Initialize()
		{
			_initialPosition = transform.position;


		}

		private void Awake()
		{
			ChangeState(BoatState.InThePort);
			TimerText();
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
					
						break;
					case BoatState.GoingFishing:
						_currentTime = 9f;
						dockTimerPanel.gameObject.SetActive(true);
						TimerText();
						_isTimerRunning = true;
						MoveToPosition(new Vector3(-40f, 0f, 0f), 2f);
						DOVirtual.DelayedCall(3f, () =>
						{
							ChangeState(BoatState.Fishing);
						});
						break;
					case BoatState.Fishing:
						if (_fishCapacity < _maxFishCapacity)
						{

							UpdateFishCapacityText();
						}
						break;
					case BoatState.ReturningToPort:
						MoveToPosition(_initialPosition, 1f);
						_isTimerRunning = false;
						
						DOVirtual.DelayedCall(1f, () =>
						{
							dockTimerPanel.gameObject.SetActive(false);
							UpdateTransferTheFishText();
							UpdateGetTheFish();
						});

						break;
				}

			}
		}

		private void Update()
		{
			TimerControl();

		}

		private void MoveToPosition(Vector3 targetPosition, float duration)
		{
			transform.DOMove(targetPosition, duration);
		}

		private void OnMouseDown()
		{
			Debug.Log("OnMouseDown");
			ChangeState(BoatState.GoingFishing);
		}

		private void UpdateFishCapacityText()
		{
			_fishCapacity++;
			fishCapacityText.text = $" {_maxFishCapacity}";
		}

		private void UpdateTransferTheFishText()
		{
			_fishCapacity= 0;
			fishCapacityText.text = $" {_fishCapacity}";
		}

		private void UpdateGetTheFish()
		{
			_totalFishCount += _maxFishCapacity;
			boxFishText.text = $" {_totalFishCount}";
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

		private void TimerText()
		{
			timerText.text = _currentTime.ToString();
		}
	}

}
