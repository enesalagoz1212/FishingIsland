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

		public TextMeshProUGUI fishCapacityText;
		public void Initialize()
		{
			_initialPosition = transform.position;
		}

		private void Awake()
		{
			BoatState = BoatState.InThePort;
		}

		public void ChangeState(BoatState boatState)
		{
			BoatState = boatState;
			Debug.Log($"BoatState: {boatState}");
		}



		private void Update()
		{
			if (GameManager.Instance.GameState == GameState.Playing)
			{
				switch (BoatState)
				{
					case BoatState.InThePort:
						transform.position = _initialPosition;
						break;
					case BoatState.GoingFishing:
						MoveToPosition(new Vector3(-40f, 0f, 0f), 3f);
						DOVirtual.DelayedCall(3f, () =>
						{
							ChangeState(BoatState.Fishing);
						});
						break;
					case BoatState.Fishing:
						if (_fishCapacity < _maxFishCapacity)
						{
							_fishCapacity++;
							UpdateFishCapacityText();
						}

						else
						{
							ChangeState(BoatState.ReturningToPort);
						}
						break;
					case BoatState.ReturningToPort:
						MoveToPosition(_initialPosition, 3f);
						break;
				}
			}
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
			fishCapacityText.text = $" {_maxFishCapacity}";
		}
	}

}
