using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Controllers;
using UnityEngine.EventSystems;

namespace FishingIsland.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private CameraController _cameraController;

        private Vector3 _lastTouchPosition;
        private Vector3 _firstTouchPosition;
        private bool _isDragging;
        public bool isInputEnabled { get; private set; } = true;

		public void Initialize(CameraController cameraController)
		{
			_cameraController = cameraController;
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		public void OnScreenTouch(PointerEventData eventData)
		{
			if (!isInputEnabled)
			{
				return;
			}
			if (!_isDragging)
			{
				_isDragging = true;
				_firstTouchPosition = Input.mousePosition;
			}
		}

		public void OnScreenDrag(PointerEventData eventData)
		{
			if (!isInputEnabled)
			{
				return;
			}

			if (GameManager.Instance.GameState != GameState.Playing)
			{
				return;
			}

			_lastTouchPosition = Input.mousePosition;

			float deltaY = _lastTouchPosition.y - _firstTouchPosition.y;
			_cameraController.MoveCamera(deltaY);
		}

		public void OnScreenUp(PointerEventData eventData)
		{
			_isDragging = false;
		}
	}
}

