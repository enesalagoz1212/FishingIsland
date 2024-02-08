using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Controllers;

namespace FishingIsland.Managers
{


	[System.Serializable]
	public class StateSoundSetBoat
	{
		public AudioClip idleSoundBoat;
		public AudioClip collectFishSoundBoat;
		public AudioClip goToSellFishSoundBoat;
		public AudioClip returnFromSellingFishSoundBoat;		
	}

	[System.Serializable]
	public class StateSoundSetDockWorker
	{
		public AudioClip idleSoundDockWorker;
		public AudioClip goToCollectFishSoundDockWorker;
		public AudioClip collectingFishSoundDockWorker;
		public AudioClip returningFromCollectingFishSoundDockWorker;
	}

	[System.Serializable]
	public class StateSoundSetFishWorker
	{
		public AudioClip IdleSoundFishWorker;
		public AudioClip CollectingFishSoundFishWorker;
		public AudioClip GoingToSellFishSoundFishWorker;
		public AudioClip ReturnsFromSellingFishSoundFishWorker;
	}


	public class SoundManager : MonoBehaviour
	{
		public static SoundManager Instance { get; private set; }
		public AudioSource audioSource;

		public StateSoundSetBoat boatSounds;
		public StateSoundSetDockWorker dockWorkerSounds;
		public StateSoundSetFishWorker fishWorkerSounds;


		public void Initialize()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void PlayBoatStateSound(BoatState state)
		{
			PlaySound(GetBoatSoundForState(state));
		}

		public void PlayFishWorkerStateSound(FishWorkerState state)
		{
			PlaySound(GetFishWorkerSoundForState(state));
		}

		public void PlayDockWorkerStateSound(DockWorkerState state)
		{
			PlaySound(GetDockWorkerSoundForState(state));
		}

		private AudioClip GetBoatSoundForState(BoatState state)
		{
			switch (state)
			{
				case BoatState.InThePort:
					return boatSounds.idleSoundBoat;
				case BoatState.GoingFishing:
					return boatSounds.collectFishSoundBoat;
				case BoatState.Fishing:
					return boatSounds.goToSellFishSoundBoat;
				case BoatState.ReturningToPort:
					return boatSounds.returnFromSellingFishSoundBoat;
				default:
					return null;
			}
		}

		private AudioClip GetFishWorkerSoundForState(FishWorkerState state)
		{
			switch (state)
			{
				case FishWorkerState.Idle:
					return fishWorkerSounds.IdleSoundFishWorker;
				case FishWorkerState.CollectingFish:
					return fishWorkerSounds.CollectingFishSoundFishWorker;
				case FishWorkerState.GoingToSellFish:
					return fishWorkerSounds.GoingToSellFishSoundFishWorker;
				case FishWorkerState.ReturnsFromSellingFish:
					return fishWorkerSounds.ReturnsFromSellingFishSoundFishWorker;
				default:
					return null;
			}
		}


		private AudioClip GetDockWorkerSoundForState(DockWorkerState state)
		{
			switch (state)
			{
				case DockWorkerState.Idle:
					return dockWorkerSounds.idleSoundDockWorker;
				case DockWorkerState.GoToCollectFish:
					return dockWorkerSounds.goToCollectFishSoundDockWorker;
				case DockWorkerState.CollectingFish:
					return dockWorkerSounds.collectingFishSoundDockWorker;
				case DockWorkerState.ReturningFromCollectingFish:
					return dockWorkerSounds.returningFromCollectingFishSoundDockWorker;
				default:
					return null;
			}
		}

		private void PlaySound(AudioClip clip)
		{
			if (audioSource != null && clip != null)
			{
				audioSource.PlayOneShot(clip);
			}
		}
	}
}

