using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace OnefallGames
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { private set; get; }

        [SerializeField] private BallController ballControllerPrefab = null;
        [SerializeField] private RingController ringControllerPrefab = null;

        private List<BallController> listBallController = new List<BallController>();
        private List<RingController> listRingController = new List<RingController>();
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
        /// Get an inactive BallController object.
        /// </summary>
        /// <returns></returns>
        public BallController GetBallController()
        {
            //Find an inactive BallController object
            BallController ballController = listBallController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (ballController == null)
            {
                //Didn't find one -> create new one
                ballController = Instantiate(ballControllerPrefab, Vector3.zero, Quaternion.identity);
                ballController.gameObject.SetActive(false);
                listBallController.Add(ballController);
            }

            return ballController;
        }



        /// <summary>
        /// Get an inactive RingController object.
        /// </summary>
        /// <returns></returns>
        public RingController GetRingController()
        {
            //Find an inactive RingController object
            RingController paintedBallController = listRingController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (paintedBallController == null)
            {
                //Didn't find one -> create new one
                paintedBallController = Instantiate(ringControllerPrefab, Vector3.zero, Quaternion.identity);
                paintedBallController.gameObject.SetActive(false);
                listRingController.Add(paintedBallController);
            }

            return paintedBallController;
        }

    }
}
