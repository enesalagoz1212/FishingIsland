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

		private Vector3 _initialHouseUpPosition;
		private bool _hasAnimationPlayed = false;
		private Sequence _houseUpAnimation;

        public GameObject houseUpgradeCanvas;
		public Image houseUpImage;

		private void Start()
		{
			_initialHouseUpPosition= houseUpImage.rectTransform.localPosition;
		}

		void Update()
        {
			if (CanUpgradeHouse() && !_hasAnimationPlayed)
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
		}

		private void OnDisable()
		{
			GameManager.OnCloseButton -= OnCloseButtonAction;
		}

		private void OnCloseButtonAction()
		{
			ResetAnimation();
			if (!CanUpgradeHouse())
			{
				houseUpImage.gameObject.SetActive(false);
			}
		}

		private void OnMouseDown()
		{
			if (CanUpgradeHouse())
			{
				houseUpImage.gameObject.SetActive(false);
				houseUpgradeCanvas.SetActive(true);
				KillHouseUpAnimation();
			}
		}

		private bool CanUpgradeHouse()
		{
			_houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			return MoneyManager.Instance.GetMoney() >= _houseUpgrade.fishWorkerLevelUpgradeCost
				|| MoneyManager.Instance.GetMoney() >= _houseUpgrade.timerLevelUpgradeCost
				|| MoneyManager.Instance.GetMoney() >= _houseUpgrade.capacityLevelUpgradeCost;
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

