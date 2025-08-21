using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player{
    public class PlayerCameraFollow : MonoBehaviour{
        [SerializeField] private CinemachineVirtualCamera vCam;
        [SerializeField] private Transform target;

        [SerializeField] private float distance = 8f;
        [SerializeField] private float height = 10f;

        [SerializeField] private float smoothSpeed = 5;
        [SerializeField] private float orbitAngle = 90f;
        private CinemachineTransposer transposer;

        private void Awake() {
            transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            if (!transposer) {
                enabled = false;
            }
        }

        private void LateUpdate() {
            if (!target) return;
            ApplyOffsetAndLook();
        }

        private void ApplyOffsetAndLook() {
            if (!transposer) return;

            Vector3 local = new Vector3(0f, height, -distance);
            Vector3 worldOffset = Quaternion.Euler(0f, orbitAngle, 0f) * local;

            transposer.m_FollowOffset = worldOffset;
            Vector3 pos = vCam.transform.position;
            Vector3 dir = (target.position - pos);
            if (dir.sqrMagnitude > 0.01f) {
                var quar = Quaternion.LookRotation(dir.normalized, Vector3.up);
                vCam.transform.rotation =
                    Quaternion.Lerp(vCam.transform.rotation, quar, Time.deltaTime * smoothSpeed);
            }
        }
    }
}