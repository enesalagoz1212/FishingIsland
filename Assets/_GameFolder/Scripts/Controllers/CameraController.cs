using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public float moveSpeed;
        public float maxDistance;
        public float minDistance;
 
        public void Initialize()
		{

		}

        public void MoveCamera(float deltaY)
        {
            Vector3 cameraPosition = transform.position;
            cameraPosition.z = Mathf.Clamp(cameraPosition.z + deltaY * moveSpeed * Time.deltaTime, minDistance, maxDistance);
            transform.position = cameraPosition;
        }
    }
}

