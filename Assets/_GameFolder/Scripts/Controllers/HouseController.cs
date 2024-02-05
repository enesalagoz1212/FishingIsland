using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using UnityEngine.UI;
using DG.Tweening;
using FishingIsland.Managers;

namespace FishingIsland.Controllers
{
	public class HouseController : MonoBehaviour
	{
		private HouseUpgrade _houseUpgrade;
		private UpgradeManager _upgradeManager;

		private Vector3 _initialHouseUpPosition;
		private bool _hasAnimationPlayed = false;
		private Sequence _houseUpAnimation;

		public Image houseUpImage;
		public Image houseLevelPanel;

		private GameObject house;
		private List<GameObject> housePrefabs;

		public void Initialize(UpgradeManager upgradeManager)
		{
			_upgradeManager = upgradeManager;
			_houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			housePrefabs = _houseUpgrade.GetHouseGameObjects();

			Instantiate(housePrefabs[0]);
		}

		private void Start()
		{
			_initialHouseUpPosition = houseUpImage.rectTransform.localPosition;
			houseLevelPanel.gameObject.SetActive(true);
			
		}

		void Update()
		{
			if (!_hasAnimationPlayed)
			{
				AnimateHouseUp();
				_hasAnimationPlayed = true;
			}
			else
			{

			}
		}

		private void OnEnable()
		{
			GameManager.OnCloseButton += OnCloseButtonAction;
			GameManager.OnButtonClickedHouseUpgrade += OnButtonClickedHouseUpgradeAction;
		}

		private void OnDisable()
		{
			GameManager.OnCloseButton -= OnCloseButtonAction;
			GameManager.OnButtonClickedHouseUpgrade -= OnButtonClickedHouseUpgradeAction;
		}

		private void Instantiate(GameObject housePrefab)
		{
			if (house!=null)
			{
				Destroy(house);
			}

			house = Instantiate(housePrefab, transform.position, Quaternion.identity, transform);
		}

		private void OnButtonClickedHouseUpgradeAction()
		{
			int fishWorkerLevel = _houseUpgrade.houseUpgradeData.fishWorkerLevel;
			int speedLevel = _houseUpgrade.houseUpgradeData.speedLevel;
			int capacityLevel = _houseUpgrade.houseUpgradeData.capacityLevel;

			if (fishWorkerLevel >= 10 && speedLevel >= 10 && capacityLevel >= 10)
			{
				Instantiate(housePrefabs[1]);
			}
		}

		private void OnCloseButtonAction()
		{
			ResetAnimation();
			houseUpImage.gameObject.SetActive(false);
		}

		private void OnMouseDown()
		{
			houseUpImage.gameObject.SetActive(false);
			_upgradeManager.ActivateHouseUpgradeCanvas();
			KillHouseUpAnimation();
		}

		public void AnimateHouseUp()
		{
			houseUpImage.gameObject.SetActive(true);

			float animationDistance = 0.6f;
			Vector3 initialPosition = _initialHouseUpPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + animationDistance, initialPosition.z);

			_houseUpAnimation = DOTween.Sequence();
			_houseUpAnimation.Append(houseUpImage.rectTransform.DOLocalMove(targetPosition, 0.7f).SetEase(Ease.OutQuad));
			_houseUpAnimation.Append(houseUpImage.rectTransform.DOLocalMove(initialPosition, 0.7f).SetEase(Ease.InQuad));

			_houseUpAnimation.SetLoops(-1, LoopType.Yoyo);
		}

		private void KillHouseUpAnimation()
		{
			if (_houseUpAnimation != null && _houseUpAnimation.IsActive())
			{
				_houseUpAnimation.Kill();
			}
			houseUpImage.rectTransform.anchoredPosition = _initialHouseUpPosition;
		}

		public void ResetAnimation()
		{
			_hasAnimationPlayed = false;
		}
	}
}

