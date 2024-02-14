using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using FishingIsland.LevelScriptableObject;

namespace FishingIsland.Managers
{
	public class LevelManager : MonoBehaviour
	{
		public static LevelManager Instance { get; private set; }
		public LevelData levelData;

		private SaveLoadManager _saveLoadManager;
		private DockUpgradeManager _dockUpgradeManager;
		private ShackUpgradeManager _shackUpgradeManager;
		private HouseUpgradeManager _houseUpgradeManager;

		private List<GameObject> activeLevels = new List<GameObject>();
		private int _currentLevelIndex = 0;


		public List<Transform> boatSpawnPoints = new List<Transform>();

		public List<Transform> fishWorkerSellPath = new List<Transform>();
		public List<Transform> fishWorkerReturnPath = new List<Transform>();

		public List<Transform> dockWorkerGoesFishing = new List<Transform>();

		public List<Transform> BoatSpawnPoints
		{
			get { return boatSpawnPoints; }
		}

		private void OnEnable()
		{
			GameManager.OnButtonClickedDockUpgrade += OnButtonClickedDockUpgradeAction;
			GameManager.OnButtonClickedShackUpgrade += OnButtonClickedShackUpgradeAction;
			GameManager.OnButtonClickedHouseUpgrade += OnButtonClickedHouseUpgradeAction;
			GameManager.OnGameReset += OnGameResetAction;
		}

		private void OnDisable()
		{
			GameManager.OnButtonClickedDockUpgrade -= OnButtonClickedDockUpgradeAction;
			GameManager.OnButtonClickedShackUpgrade -= OnButtonClickedShackUpgradeAction;
			GameManager.OnButtonClickedHouseUpgrade -= OnButtonClickedHouseUpgradeAction;
			GameManager.OnGameReset -= OnGameResetAction;
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

		public void Initialize(DockUpgradeManager dockUpgradeManager, ShackUpgradeManager shackUpgradeManager, HouseUpgradeManager houseUpgradeManager ,SaveLoadManager saveLoadManager)
		{
			LoadCurrentLevelIndex();


			_dockUpgradeManager = dockUpgradeManager;
			_shackUpgradeManager = shackUpgradeManager;
			_houseUpgradeManager = houseUpgradeManager;
		}

		private void Start()
		{
			StartLevelSequence();
		}
		public Transform GetRandomBoatSpawnPoint()
		{
			if (boatSpawnPoints.Count == 0)
			{
				Debug.LogError("No boat spawn points available ");
				return null;
			}

			int randomIndex = Random.Range(0, boatSpawnPoints.Count);
			return boatSpawnPoints[randomIndex];
		}

		private void StartLevelSequence()
		{
			if (_currentLevelIndex < levelData.levelObjects.Count)
			{
				GameObject newLevelObject = Instantiate(levelData.levelObjects[_currentLevelIndex]);
				newLevelObject.SetActive(true);
				activeLevels.Add(newLevelObject);
			}
			else
			{
				Debug.LogError("All levels are completed!");
			}
		}

		private void OnButtonClickedDockUpgradeAction()
		{
			LevelCompleted();
		}

		private void OnButtonClickedShackUpgradeAction()
		{
			LevelCompleted();
		}

		private void OnButtonClickedHouseUpgradeAction()
		{
			LevelCompleted();
		}

		private void OnGameResetAction()
		{
			DestroyCurrentLevel();
			_currentLevelIndex++;
			SetCurrentLevelIndex(_currentLevelIndex);
			StartLevelSequence();
		}

		public void LevelCompleted()
		{

			int boatLevelDockUpgrade = _dockUpgradeManager.GetBoatLevel();
			int speedLevelDockUpgrade = _dockUpgradeManager.GetSpeedLevel();
			int capacityLevelDockUpgrade = _dockUpgradeManager.GetCapacityLevel();

			int dockWorkerLevelShackUpgrade = _shackUpgradeManager.GetDockWorkerLevel();
			int speedLevelShackUpgrade = _shackUpgradeManager.GetSpeedLevel();
			int capacityLevelShackUpgrade = _shackUpgradeManager.GetCapacityLevel();

			int fishWorkerLevelHouseUpgrade = _houseUpgradeManager.GetFishWorkerLevel();
			int speedLevelHouseUpgrade = _houseUpgradeManager.GetSpeedLevel();
			int capacityLevelHouseUpgrade = _houseUpgradeManager.GetCapacityLevel();

			if (boatLevelDockUpgrade == 20 && speedLevelDockUpgrade == 20 && capacityLevelDockUpgrade == 20 && dockWorkerLevelShackUpgrade == 20 && speedLevelShackUpgrade == 20 && capacityLevelShackUpgrade == 20 && fishWorkerLevelHouseUpgrade == 20 && speedLevelHouseUpgrade == 20 && capacityLevelHouseUpgrade == 20)
			{
				GameManager.OnGameLevelCompleted?.Invoke(true);
			}
		}

		public int GetCurrentLevelIndex()
		{
			return _currentLevelIndex;
		}

		public void SetCurrentLevelIndex(int index)
		{
			_currentLevelIndex = index;
			PlayerPrefs.SetInt("CurrentLevelIndex", _currentLevelIndex);
			PlayerPrefs.Save();
		}

		private void LoadCurrentLevelIndex()
		{
			_currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
			
		}


		private void DestroyCurrentLevel()
		{
			if (_currentLevelIndex < activeLevels.Count)
			{
				Destroy(activeLevels[_currentLevelIndex]);
				activeLevels.RemoveAt(_currentLevelIndex);
			}
		}
	}
}

