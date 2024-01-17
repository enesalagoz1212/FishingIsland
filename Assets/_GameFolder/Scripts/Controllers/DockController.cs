using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FishingIsland.Managers;
using FishingIsland.UpgradeScriptableObjects;

namespace FishingIsland.Controllers
{
	public class DockController : MonoBehaviour
	{
		public GameObject dockUpgradeCanvas;
		private DockUpgrade _dockUpgrade;
		private Vector3 _initialDockUpPosition;
		public Image dockUpImage;
		private Sequence _dockUpAnimation;

		private bool hasAnimationPlayed = false;

		public void Initialize()
		{

		}
		private void Update()
		{
			if (CanUpgradeDock() && !hasAnimationPlayed)
			{
				AnimateDockUp();
				hasAnimationPlayed = true;
			}
			else
			{
			//	Debug.Log($"Money: {MoneyManager.Instance.GetMoney()} - Can Upgrade Dock: {CanUpgradeDock()}");
			}

		}
		public bool Animation()
		{
			return hasAnimationPlayed;
		}

		private void OnMouseDown()
		{
			if (CanUpgradeDock())
			{
				dockUpImage.gameObject.SetActive(false);
				dockUpgradeCanvas.SetActive(true);
				KillDockUpAnimation();
			}
		
		}

		private bool CanUpgradeDock()
		{
			_dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();
			return MoneyManager.Instance.GetMoney() >= _dockUpgrade.boatLevelUpgradeCost
				|| MoneyManager.Instance.GetMoney() >= _dockUpgrade.timerLevelUpgradeCost
				|| MoneyManager.Instance.GetMoney() >= _dockUpgrade.capacityLevelUpgradeCost;
		}

		public void AnimateDockUp()
		{
			dockUpImage.gameObject.SetActive(true);

			float animationDistance = 0.7f;
			Vector3 initialPosition = dockUpImage.rectTransform.localPosition;
			_initialDockUpPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + animationDistance, initialPosition.z);

			_dockUpAnimation = DOTween.Sequence();
			_dockUpAnimation.Append(dockUpImage.rectTransform.DOLocalMove(targetPosition, 1.0f).SetEase(Ease.OutQuad));
			_dockUpAnimation.Append(dockUpImage.rectTransform.DOLocalMove(initialPosition, 1.0f).SetEase(Ease.InQuad));

			_dockUpAnimation.SetLoops(-1, LoopType.Yoyo);

		}

		private void KillDockUpAnimation()
		{
			if (_dockUpAnimation != null && _dockUpAnimation.IsActive())
			{
				_dockUpAnimation.Kill();
			}
			dockUpImage.rectTransform.anchoredPosition = _initialDockUpPosition;
		}

		public void ResetAnimation()
		{
			Debug.Log("ResetAnimation calisti");
			hasAnimationPlayed = false;
		}
	}
}
