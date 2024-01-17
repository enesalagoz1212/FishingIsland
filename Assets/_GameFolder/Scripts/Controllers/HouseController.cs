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

        public GameObject houseUpgradeCanvas;
		private HouseUpgrade _houseUpgrade;


		private Vector3 _initialHouseUpPosition;
		public Image houseUpImage;
		private Sequence _houseUpAnimation;

		private bool hasAnimationPlayed = false;
		void Start()
        {

        }


        void Update()
        {
			if (CanUpgradeHouse() && !hasAnimationPlayed)
			{
				Debug.Log("888");
				AnimateHouseUp();
				hasAnimationPlayed = true;
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
			Debug.Log("HouseController tetiklendi");
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

			float animationDistance = 0.7f;
			Vector3 initialPosition = houseUpImage.rectTransform.localPosition;
			_initialHouseUpPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + animationDistance, initialPosition.z);

			_houseUpAnimation = DOTween.Sequence();
			_houseUpAnimation.Append(houseUpImage.rectTransform.DOLocalMove(targetPosition, 1.0f).SetEase(Ease.OutQuad));
			_houseUpAnimation.Append(houseUpImage.rectTransform.DOLocalMove(initialPosition, 1.0f).SetEase(Ease.InQuad));

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
			hasAnimationPlayed = false;
		}
	}
}

