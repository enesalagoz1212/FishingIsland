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
		private UpgradeManager _upgradeManager;

		private Vector3 _initialDockUpPosition;
		private Sequence _dockUpAnimation;
		private bool hasAnimationPlayed = false;

		public Image dockUpImage;
		public Image dockLevelPanel;

		private GameObject dock;
		private List<GameObject> dockPrefabs;

		public void Initialize(UpgradeManager upgradeManager)
		{
			_upgradeManager = upgradeManager;
			_dockUpgrade=DockUpgradeManager.Instance.GetDockUpgrade();
			dockPrefabs = _dockUpgrade.GetDockGameObjects();
		}

		private void Start()
		{
			_initialDockUpPosition = dockUpImage.rectTransform.localPosition;
			InstantiateDock(dockPrefabs[0]);
			dockLevelPanel.gameObject.SetActive(true);
		}

		private void OnEnable()
		{
			GameManager.OnCloseButton += OnCloseButtonAction;
			GameManager.OnButtonClickedDockUpgrade += OnButtonClickedDockUpgradeAction;
		}

		private void OnDisable()
		{
			GameManager.OnCloseButton -= OnCloseButtonAction;
			GameManager.OnButtonClickedDockUpgrade -= OnButtonClickedDockUpgradeAction;
		}

		private void OnButtonClickedDockUpgradeAction()
		{
			int boatLevel = _dockUpgrade.dockUpgradeData.boatLevel;
			int speedLevel = _dockUpgrade.dockUpgradeData.speedLevel;
			int capacityLevel = _dockUpgrade.dockUpgradeData.capacityLevel;

			if (boatLevel == 10 && speedLevel == 10 && capacityLevel == 10)
			{
				Destroy(dock);
				InstantiateDock(dockPrefabs[1]);
			}
		}

		private void OnCloseButtonAction()
		{
			ResetAnimation();
			dockUpImage.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (!hasAnimationPlayed)
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
			dockUpImage.gameObject.SetActive(false);
			_upgradeManager.ActivateDockUpgradeCanvas();
			KillDockUpAnimation();
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

		private void InstantiateDock(GameObject dockPrefab)
		{
			if (dock != null)
			{
				Destroy(dock);
			}

			dock = Instantiate(dockPrefab, transform.position, Quaternion.identity, transform);
		}
	}
}
