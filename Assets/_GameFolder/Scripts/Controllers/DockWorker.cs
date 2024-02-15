using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Controllers
{
	public enum DirectionDockWorker
	{
        DockWorkerEast,
        DockWorkerWest,
	}
    public class DockWorker : MonoBehaviour
    {
        public DirectionDockWorker Direction { get; private set; }

        private Vector3 lastPosition;

        void Start()
        {
            Direction = DirectionDockWorker.DockWorkerEast;

            lastPosition = transform.position;
        }


        void Update()
        {
            UpdateDirectionFromPosition();
        }

        private void SetRotation()
        {
            switch (Direction)
            {
                case DirectionDockWorker.DockWorkerEast:
                    transform.rotation = Quaternion.Euler(0f, 0, 0f);
                    break;
                case DirectionDockWorker.DockWorkerWest:
                    transform.rotation = Quaternion.Euler(0f, 180, 0f);
                    break;
                default:
                    break;
            }
        }

        private void UpdateDirectionFromPosition()
        {
            Vector3 currentPosition = transform.position;

            Vector3 direction = currentPosition - lastPosition;

            if (direction.z < 0)
            {
                Direction = DirectionDockWorker.DockWorkerEast;
            }
            else if (direction.z > 0)
            {
                Direction = DirectionDockWorker.DockWorkerWest;
            }

            SetRotation();
            lastPosition = currentPosition;
           Direction = DirectionDockWorker.DockWorkerEast;
        }
    }
}

