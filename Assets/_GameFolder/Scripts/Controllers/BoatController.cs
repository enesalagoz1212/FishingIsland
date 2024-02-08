using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
using FishingIsland.UpgradeScriptableObjects;

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
		private DockUpgrade dockUpgrade;
		private DockUpgradeData dockUpgradeData;

		private Vector3 _initialPosition;
		private Vector3 _initialBoatDownPosition;
		private int _boatFishCapacity;
		private float currentProgress;
		private bool _canClick = true;
		public int FishCount { get; private set; }
		public GameObject boatFishPanel;
		public Image boatDownOkImage;
		public Image boatBarImage;
		public Image circularProgressBar;
		public TextMeshProUGUI fishCapacityText;
		public TextMeshProUGUI boxFishText;

		private GameObject boat;
		private List<GameObject> boatPrefabs;
		private Sequence _boatDownAnimation;

		public void Initialize()
		{
			_initialPosition = transform.position;
			dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();
			boatPrefabs = dockUpgrade.GetBoatGameObjects();
			InstantiateBoat(boatPrefabs[0]);
			ChangeState(BoatState.InThePort);
		}

		private void Start()
		{

		}

		private void OnEnable()
		{
			GameManager.OnButtonClickedDockUpgrade += OnButtonClickedDockUpgradeAction;
		}

		private void OnDisable()
		{
			GameManager.OnButtonClickedDockUpgrade -= OnButtonClickedDockUpgradeAction;

		}

		public void ChangeState(BoatState boatState)
		{
			BoatState = boatState;
			switch (BoatState)
			{
				case BoatState.InThePort:
					SoundManager.Instance.PlayBoatStateSound(BoatState.InThePort);
					AnimateBoatDown();
					transform.position = _initialPosition;
					boatFishPanel.gameObject.SetActive(false);
					_canClick = true;
					break;
				case BoatState.GoingFishing:
					SoundManager.Instance.PlayBoatStateSound(BoatState.GoingFishing);
					Transform randomSpawnPoint = LevelManager.Instance.GetRandomBoatSpawnPoint();
					if (randomSpawnPoint != null)
					{
						MoveToPosition(randomSpawnPoint.position, 1f, () =>
						{
							ChangeState(BoatState.Fishing);
						});
					}
					break;
				case BoatState.Fishing:
					SoundManager.Instance.PlayBoatStateSound(BoatState.Fishing);
					boatBarImage.gameObject.SetActive(true);
					UpdateCollectTheFishText();
					break;
				case BoatState.ReturningToPort:
					SoundManager.Instance.PlayBoatStateSound(BoatState.ReturningToPort);
					boatBarImage.gameObject.SetActive(false);
					circularProgressBar.fillAmount = 0f;
					MoveToPosition(_initialPosition, 1f, () =>
					{
						FishBoxController.OnBoatArrivedBox?.Invoke(this);
					});
					break;
			}
		}

		private void AnimateBoatDown()
		{
			boatDownOkImage.gameObject.SetActive(true);

			float animationDistance = 0.6f;
			Vector3 initialPosition = boatDownOkImage.rectTransform.localPosition;
			_initialBoatDownPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y - animationDistance, initialPosition.z);

			_boatDownAnimation = DOTween.Sequence();
			_boatDownAnimation.Append(boatDownOkImage.rectTransform.DOLocalMove(targetPosition, 0.7f).SetEase(Ease.OutQuad));
			_boatDownAnimation.Append(boatDownOkImage.rectTransform.DOLocalMove(initialPosition, 0.7f).SetEase(Ease.InQuad));

			_boatDownAnimation.SetLoops(-1, LoopType.Yoyo);
		}

		private void KillBoatDownAnimation()
		{
			if (_boatDownAnimation != null && _boatDownAnimation.IsActive())
			{
				_boatDownAnimation.Kill();
			}
			boatDownOkImage.rectTransform.anchoredPosition = _initialBoatDownPosition;
		}

		private void MoveToPosition(Vector3 targetPosition, float duration, Action onComplete)
		{
			dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();
			float updatedBoatSpeed = dockUpgrade.UpdateDockUpgradeBoatLevel(dockUpgrade.dockUpgradeData.boatLevel);

			transform.DOMove(targetPosition, duration * updatedBoatSpeed).OnComplete(() =>
			{
				onComplete?.Invoke();
			});
		}

		private void OnMouseDown()
		{
			if (_canClick)
			{
				ChangeState(BoatState.GoingFishing);
				KillBoatDownAnimation();
				boatDownOkImage.gameObject.SetActive(false);
				_canClick = false;
			}
		}

		public void OnFishTransferredToFishBox()
		{
			_boatFishCapacity = dockUpgrade.ReturnBoatFishCapacity();
			if (FishCount > 0)
			{
				FishCount--;
				UpdateFishCapacityText(FishCount);
			}

			if (FishCount <= 0)
			{
				ChangeState(BoatState.InThePort);
			}
		}

		private void UpdateFishCapacityText(int fishAmount)
		{
			fishCapacityText.text = $" {fishAmount}";
		}

		private void UpdateCollectTheFishText()
		{
			StartCoroutine(CollectFish());
		}

		private IEnumerator CollectFish()
		{
			dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();
			_boatFishCapacity = dockUpgrade.ReturnBoatFishCapacity();
			boatFishPanel.SetActive(true);

			float oneFishGatherSpeed = dockUpgrade.UpdateDockUpgradeSpeedLevel(dockUpgrade.dockUpgradeData.speedLevel);
			float oneFishGatherTime;
			float timer = 0f;
			currentProgress = 0;


			while (FishCount < dockUpgrade.ReturnBoatFishCapacity())
			{
				if (oneFishGatherSpeed != dockUpgrade.UpdateDockUpgradeSpeedLevel(dockUpgrade.dockUpgradeData.speedLevel))
				{
					oneFishGatherSpeed = dockUpgrade.UpdateDockUpgradeSpeedLevel(dockUpgrade.dockUpgradeData.speedLevel);
				}

				oneFishGatherTime = 1 / oneFishGatherSpeed;


				timer += Time.deltaTime;

				if (timer >= oneFishGatherTime)
				{
					FishCount++;
					UpdateFishCapacityText(FishCount);

					UpdateCircularProgressBar();
					timer = 0f;
				}

				yield return null;

				if (FishCount >= dockUpgrade.ReturnBoatFishCapacity())
				{
					ChangeState(BoatState.ReturningToPort);
				}
			}
		}

		private void UpdateCircularProgressBar()
		{
			if (circularProgressBar != null)
			{
				dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();
				int maxCapacity = dockUpgrade.ReturnBoatFishCapacity();

				currentProgress = (float)FishCount / maxCapacity;

				circularProgressBar.fillAmount = currentProgress;
			}

		}

		private void OnButtonClickedDockUpgradeAction()
		{
			int boatLevel = dockUpgrade.dockUpgradeData.boatLevel;
			int speedLevel = dockUpgrade.dockUpgradeData.speedLevel;
			int capacityLevel = dockUpgrade.dockUpgradeData.capacityLevel;

			if (boatLevel >= 10 && speedLevel >= 10 && capacityLevel >= 10)
			{
				Destroy(boat);
				InstantiateBoat(boatPrefabs[1]);
			}
		}

		private void InstantiateBoat(GameObject boatPrefab)
		{
			if (boat != null)
			{
				Destroy(boat);
			}

			boat = Instantiate(boatPrefab, transform.position, Quaternion.identity, transform);
		}
	}
}

