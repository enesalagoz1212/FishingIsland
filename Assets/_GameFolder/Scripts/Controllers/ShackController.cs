using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FishingIsland.UpgradeScriptableObjects;
using FishingIsland.Managers;
using UnityEngine.UI;
using DG.Tweening;

namespace FishingIsland.Controllers
{
	public class ShackController : MonoBehaviour
	{
		public static ShackController Instance;

		private UpgradeManager _upgradeManager;
		private HouseUpgrade _houseUpgrade;
		private ShackUpgrade _shackUpgrade;

		private Vector3 _initialShackUpPosition;
		private int _fishCapacity;
		private int _shackFishCount = 0;
		private float _timePerFish;
		private bool _isFishCollectionCompletedShack = false;
		private bool _hasAnimationPlayed = false;

		private GameObject shack;
		private List<GameObject> shackPrefabs;
		private int _currentShackIndex = 0;

		private Sequence _shackUpAnimation;

		public TextMeshProUGUI shackFishCountText;
		public Image shackUpImage;
		//public Image levelPanel;
		public Image shackFishPanel;
		
		public bool HasFishShack => _shackFishCount > 0;
		public bool IsFishCollectionCompleted => _isFishCollectionCompletedShack;
		public int startingFishCount;

		public static Action<FishWorkerController> OnFishWorkerArrivedBox;
		public static Action<DockWorkerController> OnDockWorkerArrivedShack;

	
		public void Initialize(UpgradeManager upgradeManager)
		{
			_shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_upgradeManager = upgradeManager;
			shackPrefabs = _shackUpgrade.GetShackGameObjects();

			LoadCurrentShackIndex();
			Instantiate(shackPrefabs[_currentShackIndex]);
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			_initialShackUpPosition = shackUpImage.rectTransform.localPosition;
			shackFishPanel.gameObject.SetActive(true);
			//levelPanel.gameObject.SetActive(true);
		}

		private void OnEnable()
		{
			OnFishWorkerArrivedBox += OnFishWorkerArrivedBoxAction;
			OnDockWorkerArrivedShack += OnFishWorkerArrivedShackAction;
			GameManager.OnCloseButton += OnCloseButtonAction;
			GameManager.OnButtonClickedShackUpgrade += OnButtonClickedShackUpgradeAction;
			GameManager.OnGameReset += OnGameResetAction;
		}

		private void OnDisable()
		{
			OnFishWorkerArrivedBox -= OnFishWorkerArrivedBoxAction;
			OnDockWorkerArrivedShack -= OnFishWorkerArrivedShackAction;
			GameManager.OnCloseButton -= OnCloseButtonAction;
			GameManager.OnButtonClickedShackUpgrade -= OnButtonClickedShackUpgradeAction;
			GameManager.OnGameReset += OnGameResetAction;
		}

		private void Update()
		{
			if (!_hasAnimationPlayed)
			{
				AnimateShackUp();
				_hasAnimationPlayed = true;
			}
			else
			{

			}
		}

		private void OnMouseDown()
		{
			shackUpImage.gameObject.SetActive(false);
			KillShackUpAnimation();
			_upgradeManager.ActivateShackUpgradeCanvas();
		}

		private void OnGameResetAction()
		{
			_currentShackIndex++;
			SaveCurrentShackIndex();

			_shackFishCount = 0;
			UpdateFishCountText();
		}

		public int GetTotalFishCount()
		{
			return _shackFishCount;
		}

		public void SetTotalFishCount(int totalFishCount)
		{
			_shackFishCount = totalFishCount;
			UpdateFishCountText();
		}

		private void OnCloseButtonAction()
		{
			ResetAnimation();

			shackUpImage.gameObject.SetActive(false);
		}

		private void OnButtonClickedShackUpgradeAction()
		{
			int dockWorkerLevel = _shackUpgrade.shackUpgradeData.dockWorkerLevel;
			int speedLevel = _shackUpgrade.shackUpgradeData.speedLevel;
			int capacityLevel = _shackUpgrade.shackUpgradeData.capacityLevel;

			if (dockWorkerLevel >= 10 && speedLevel >= 10 && capacityLevel >= 10)
			{
				Destroy(shack);
				Instantiate(shackPrefabs[1]);
			}
		}

		private void Instantiate(GameObject shackPrefab)
		{
			if (shack!=null)
			{
				Destroy(shack);
			}

			shack = Instantiate(shackPrefab, transform.position, Quaternion.Euler(0,90,0), transform);
		}

		private IEnumerator StartFishTransferFromFishWorker(FishWorkerController fishWorkerController)
		{
			_houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			_fishCapacity = _houseUpgrade.ReturnFishWorkerFishCapacity();

			_isFishCollectionCompletedShack = false;

			int fishCount = 0;

			float oneFishGatherSpeed = _houseUpgrade.UpdateHouseUpgradeSpeed(_houseUpgrade.houseUpgradeData.speedLevel);
			float oneFishGatherTime;
			float timer = 0f;


			while (fishCount < _houseUpgrade.ReturnFishWorkerFishCapacity() && HasFishShack && !_isFishCollectionCompletedShack)
			{
				if (oneFishGatherSpeed != _houseUpgrade.UpdateHouseUpgradeSpeed(_houseUpgrade.houseUpgradeData.speedLevel))
				{
					oneFishGatherSpeed = _houseUpgrade.UpdateHouseUpgradeSpeed(_houseUpgrade.houseUpgradeData.speedLevel);
				}

				oneFishGatherTime = 1 / oneFishGatherSpeed;
				timer += Time.deltaTime;

				if (timer >= oneFishGatherTime)
				{
					fishWorkerController.OnFishTransferredToFishWorker();
					DecreaseFishCount(1);
					fishCount++;
					timer = 0;
				}

				if (fishCount >= _houseUpgrade.ReturnFishWorkerFishCapacity() || !HasFishShack)
				{
					fishWorkerController.OnFishCollectionCompleted();
				}

				yield return null;

			}
			_isFishCollectionCompletedShack = true;
		}

		private IEnumerator StartFishCollectFromDockWorker(DockWorkerController dockWorkerController)
		{
			while (dockWorkerController.CollectedFishCount > 0)
			{
				dockWorkerController.OnFishTransferedFishShack();
				IncreaseFishCount(1);
				yield return new WaitForSeconds(0.02f);
			}
		}

		private void UpdateFishCountText()
		{
			shackFishCountText.text = $" {_shackFishCount}";
		}

		private void OnFishWorkerArrivedBoxAction(FishWorkerController fishWorkerController)
		{
			StartCoroutine(StartFishTransferFromFishWorker(fishWorkerController));
		}

		private void OnFishWorkerArrivedShackAction(DockWorkerController dockWorkerController)
		{
			StartCoroutine(StartFishCollectFromDockWorker(dockWorkerController));
		}

		private void DecreaseFishCount(int amount)
		{
			_shackFishCount -= amount;
			UpdateFishCountText();
		}

		private void IncreaseFishCount(int amount)
		{
			_shackFishCount += amount;
			UpdateFishCountText();
		}

		public void AnimateShackUp()
		{
			shackUpImage.gameObject.SetActive(true);

			float animationDistance = 0.7f;
			Vector3 initialPosition = _initialShackUpPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + animationDistance, initialPosition.z);

			_shackUpAnimation = DOTween.Sequence();
			_shackUpAnimation.Append(shackUpImage.rectTransform.DOLocalMove(targetPosition, 0.7f).SetEase(Ease.OutQuad));
			_shackUpAnimation.Append(shackUpImage.rectTransform.DOLocalMove(initialPosition, 0.7f).SetEase(Ease.InQuad));

			_shackUpAnimation.SetLoops(-1, LoopType.Yoyo);

		}

		private void KillShackUpAnimation()
		{
			if (_shackUpAnimation != null && _shackUpAnimation.IsActive())
			{
				_shackUpAnimation.Kill();
			}
			shackUpImage.rectTransform.anchoredPosition = _initialShackUpPosition;
		}

		public void ResetAnimation()
		{
			_hasAnimationPlayed = false;
		}

		public void Reset()
		{
			_shackFishCount = 0;
			UpdateFishCountText();
		}

		private void SaveCurrentShackIndex()
		{
			PlayerPrefs.SetInt("CurrentShackIndex", _currentShackIndex);
			PlayerPrefs.Save();
		}

		private void LoadCurrentShackIndex()
		{
			_currentShackIndex = PlayerPrefs.GetInt("CurrentShackIndex", 0);
		}
	}

}
