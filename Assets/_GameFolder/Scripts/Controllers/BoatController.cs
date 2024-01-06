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

		public TextMeshProUGUI timerText;
		private bool _isTimerRunning = false;
		private float _currentTime = 8;
		public TextMeshProUGUI fishCapacityText;
		public GameObject dockTimerPanel;
		public TextMeshProUGUI boxFishText;
		private int _totalFishCount = 0;

		private bool _canClick = true;
		public Image boatDownOkImage;
		public GameObject boatFishPanel;
		public void Initialize()
		{
			_initialPosition = transform.position;
			ChangeState(BoatState.InThePort);
			Debug.Log("23232323");
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
						MoveToPosition(new Vector3(-40f, 0f, 0f), 2f);
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
							UpdateTransferTheFishText();
							StartCoroutine(FishBoxController.Instance.CollectFish(_maxFishCapacity));
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
			if (_canClick)
			{
				Debug.Log("OnMouseDown");
				ChangeState(BoatState.GoingFishing);
				boatDownOkImage.gameObject.SetActive(false);
				_canClick = false;
			}
		
		}

		private void UpdateFishCapacityText()
		{
			fishCapacityText.text = $" {_fishCapacity}";
		}

		private void UpdateTransferTheFishText()
		{
			StartCoroutine(TransferFish());
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
				_fishCapacity++;
				UpdateFishCapacityText();
			}
		}

		private IEnumerator TransferFish()
		{
			for (int i = 1; i <= _maxFishCapacity; i++)
			{
				yield return new WaitForSeconds(0.4f);
				_fishCapacity--;
				UpdateFishCapacityText();
			}
			ChangeState(BoatState.InThePort);
		
		}
	}
}
