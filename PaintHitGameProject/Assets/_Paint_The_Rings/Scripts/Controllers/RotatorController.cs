using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class RotatorController : MonoBehaviour
    {
        private bool isStopRotate = false;


        /// <summary>
        /// Stop or enable rotation of this rotator.
        /// </summary>
        /// <param name="stop"></param>
        public void StopRotate(bool stop)
        {
            isStopRotate = stop;
        }



        /// <summary>
        /// Setup this roatator.
        /// </summary>
        /// <param name="parameters"></param>
        public void OnSetup(RotatorParametersConfiguration parameters)
        {
            StopRotate(true);
            StartCoroutine(CRRotating(parameters));
        }


        /// <summary>
        /// Rotating this rotator using parametersConfiguration.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRRotating(RotatorParametersConfiguration parameters)
        {
            while (true)
            {
                Vector3 currentAngles = transform.eulerAngles;
                int randomYAngle = Random.Range(parameters.MinRotatingAmount, parameters.MaxRotatingAmount);
                Vector3 endAngles = Vector3.zero;
                if (Random.value <= 0.5f)
                    endAngles = new Vector3(0, currentAngles.y + randomYAngle, 0);
                else
                    endAngles = new Vector3(0, currentAngles.y - randomYAngle, 0);
                float rotatingTime = randomYAngle / Random.Range(parameters.MinRotatingSpeed, parameters.MaxRotatingSpeed);
                LerpType lerpType = parameters.LerpTypes[Random.Range(0, parameters.LerpTypes.Length)];

                float t = 0;
                while (t < rotatingTime)
                {
                    while (isStopRotate)
                    {
                        yield return null;
                    }
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / rotatingTime);
                    transform.eulerAngles = Vector3.Lerp(currentAngles, endAngles, factor);
                    yield return null;
                }
            }
        }



        /// <summary>
        /// Move down to make some space for the comming ring.
        /// </summary>
        public void MoveDown()
        {
            StopRotate(true);
            StartCoroutine(CRMovingDown());
        }


        /// <summary>
        /// Moving down to make some space for the comming ring.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRMovingDown()
        {
            float movingTime = 0.25f;
            float t = 0;
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + Vector3.down * 2f;
            while (t < movingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.Liner, t / movingTime);
                transform.position = Vector3.Lerp(startPos, endPos, factor);
                yield return null;
            }
            StopRotate(false);
        }
    }
}
