using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class RingController : MonoBehaviour
    {
        private RingPieceController[] ringPieceControllers = null;


        public bool IsPaintedAllPieces { private set; get; }


        /// <summary>
        /// Setup this ring and move it down to the rotator.
        /// </summary>
        /// <param name="ringParameters"></param>
        public void OnSetup(RingParametersConfiguration ringParameters)
        {
            IsPaintedAllPieces = false;
            ringPieceControllers = GetComponentsInChildren<RingPieceController>();
            foreach (RingPieceController ringPieceController in ringPieceControllers)
            {
                ringPieceController.OnSetup(ringParameters.PaintedRingPieceMaterial);
            }


            //Enable painted pieces
            List<int> listUsedIndex = new List<int>();
            int paintedPieces = ringParameters.PaintedPieceAmount;
            while (paintedPieces > 0)
            {
                int ringPieceIndex = Random.Range(0, ringPieceControllers.Length);
                while (listUsedIndex.Contains(ringPieceIndex))
                {
                    ringPieceIndex = Random.Range(0, ringPieceControllers.Length);
                }
                listUsedIndex.Add(ringPieceIndex);
                ringPieceControllers[ringPieceIndex].UpdateToPaintedState();
                paintedPieces--;
            }


            //Moving this ring down to the rotator
            StartCoroutine(CRMovingDown());
        }


        /// <summary>
        /// Moving from current position to Vector3.zero. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRMovingDown()
        {
            float t = 0;
            float movingTime = 0.25f;
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(startPos.x, 0, startPos.z);
            while (t < movingTime)
            {
                t += Time.deltaTime;
                float factor = t / movingTime;
                transform.position = Vector3.Lerp(startPos, endPos, EasyType.MatchedLerpType(LerpType.EaseInQuad, factor));
                yield return null;
            }
            EffectManager.Instance.CreateFadingCircleController(transform.position + Vector3.down);
        }


        /// <summary>
        /// Paint all pieces.
        /// </summary>
        public void PaintAllPieces()
        {
            StartCoroutine(CRPaintingAllPieces());
        }


        /// <summary>
        /// Painting all pieces.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRPaintingAllPieces()
        {
            EffectManager.Instance.CreateFadingRingController(transform.position);
            foreach (RingPieceController o in ringPieceControllers)
            {
                if (!o.IsPainted)
                {
                    o.UpdateToPaintedState();
                    yield return new WaitForSeconds(0.01f);
                }
            }
            IsPaintedAllPieces = true;
        }
    }
}
