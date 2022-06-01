using System;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public Transform cameraTransform;
        public Transform target;
        public float smoothSpeed;
        public Vector3 offsetToTarget;


        public void SetTarget(Transform t)
        {
            
            target = t;
            cameraTransform.position = target.position + offsetToTarget;
        }

        private void FixedUpdate()
        {
            if (target == null) return;
            
            var to = target.position + offsetToTarget;
            var smooth = Vector3.Lerp(cameraTransform.position, to, smoothSpeed);

            cameraTransform.position = smooth;
        }
    }
}