using OnefallGames;
using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class BallController : MonoBehaviour
    {

        [SerializeField] private MeshRenderer meshRenderer = null;
        [SerializeField] private LayerMask ringPieceLayerMask = new LayerMask();

        /// <summary>
        /// Set material for this ball.
        /// </summary>
        /// <param name="material"></param>
        public void SetMaterial(Material material)
        {
            meshRenderer.material = material;
        }



        /// <summary>
        /// Shoot this ball foward.
        /// </summary>
        /// <param name="speed"></param>
        public void Shoot(float speed)
        {
            StartCoroutine(CRShooting(speed));
        }

        /// <summary>
        /// Shooting this ball foward.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        private IEnumerator CRShooting(float speed)
        {
            float time = 4.5f / speed;
            float t = 0;
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(startPos.x, startPos.y, -5.5f);
            while (t < time)
            {
                t += Time.deltaTime;
                float factor = t / time;
                transform.position = Vector3.Lerp(startPos, endPos, factor);
                yield return null;
                if (IngameManager.Instance.IngameState != IngameState.Playing)
                {
                    gameObject.SetActive(false);
                }
            }


            //Detect the ring piece ahead
            Ray rayForward = new Ray(transform.position, Vector3.forward);
            RaycastHit hit;
            while (!Physics.Raycast(rayForward, out hit, 1f, ringPieceLayerMask))
            {
                yield return null;
            }

            //Explode this ball, change the deteted ring piece to a painted one and update the next ball
            EffectManager.Instance.CreatePaintedBallExplodeEffect(transform.position);
            RingPieceController ringPieceController = hit.collider.GetComponent<RingPieceController>();
            if (ringPieceController.IsPainted)
            {
                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.hitPaitedPiece);
                CameraController.Instance.Shake();
                IngameManager.Instance.HandleGameOver();
            }
            else
            {
                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.hitNormalPiece);
                ringPieceController.UpdateToPaintedState();
                IngameManager.Instance.UpdateNextBall();
            }
            gameObject.SetActive(false);
        }



        /// <summary>
        /// Move this ball forward.
        /// </summary>
        public void MoveForward()
        {
            StartCoroutine(CRMovingForward());
        }

        /// <summary>
        /// Moving this ball forward.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRMovingForward()
        {
            float time = 0.15f;
            float t = 0;
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + Vector3.forward * 2f;
            while (t < time)
            {
                t += Time.deltaTime;
                float factor = t / time;
                transform.position = Vector3.Lerp(startPos, endPos, factor);
                yield return null;
            }
        }
    }
}
