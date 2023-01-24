using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { private set; get; }

        [Header("Camera Configuration")]
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private float shakeAmount = 0.25f;
        [SerializeField] private float decreaseFactor = 1.5f;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        /// <summary>
        /// Shake this camera.
        /// </summary>
        public void Shake()
        {
            StartCoroutine(CRShaking());
        }
        private IEnumerator CRShaking()
        {
            Vector3 originalPos = transform.position;
            float shakeDurationTemp = shakeDuration;
            while (shakeDurationTemp > 0)
            {
                Vector3 newPos = originalPos + Random.insideUnitSphere * shakeAmount;
                newPos.z = originalPos.z;
                transform.position = newPos;
                shakeDurationTemp -= Time.deltaTime * decreaseFactor;
                yield return null;
            }

            transform.position = originalPos;
        }
    }
}
