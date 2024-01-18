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
		private DockUpgrade _dockUpgrade;

		private Vector3 _initialDockUpPosition;
		private Sequence _dockUpAnimation;
		private bool hasAnimationPlayed = false;
		
		public GameObject dockUpgradeCanvas;
		public Image dockUpImage;

		public void Initialize()
		{

		}

		private void Start()
		{
			_initialDockUpPosition = dockUpImage.rectTransform.localPosition;
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
			if (!CanUpgradeDock())
			{
				dockUpImage.gameObject.SetActive(false);
			}
	
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

			}
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

			float animationDistance = 0.6f;
			Vector3 initialPosition = _initialDockUpPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + animationDistance, initialPosition.z);

			_dockUpAnimation = DOTween.Sequence();
			_dockUpAnimation.Append(dockUpImage.rectTransform.DOLocalMove(targetPosition, 0.7f).SetEase(Ease.OutQuad));
			_dockUpAnimation.Append(dockUpImage.rectTransform.DOLocalMove(initialPosition, 0.7f).SetEase(Ease.InQuad));

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
			hasAnimationPlayed = false;
		}
	}
}
