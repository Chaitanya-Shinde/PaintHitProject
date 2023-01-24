using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace OnefallGames
{
    public class EffectManager : MonoBehaviour
    {

        public static EffectManager Instance { private set; get; }

        [SerializeField] private ParticleSystem paintedBallExplodeEffectPrefab = null;
        [SerializeField] private FadingCircleController fadingCircleControllerPrefab = null;
        [SerializeField] private FadingRingController fadingRingControllerPrefab = null;

        private List<ParticleSystem> listPaintedBallExplodeEffect = new List<ParticleSystem>();
        private List<FadingCircleController> listFadingCircleController = new List<FadingCircleController>();
        private List<FadingRingController> listFadingRingController = new List<FadingRingController>();

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
        /// Play the given particle then disable it 
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private IEnumerator CRPlayParticle(ParticleSystem par)
        {
            par.Play();
            yield return new WaitForSeconds(par.main.startLifetimeMultiplier);
            par.gameObject.SetActive(false);
        }


        /// <summary>
        /// Create a painted ball explode effect at given position and color.
        /// </summary>
        /// <param name="pos"></param>
        public void CreatePaintedBallExplodeEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem paintedBallExplodeEffect = listPaintedBallExplodeEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (paintedBallExplodeEffect == null)
            {
                //Didn't find one -> create new one
                paintedBallExplodeEffect = Instantiate(paintedBallExplodeEffectPrefab, pos, Quaternion.identity);
                paintedBallExplodeEffect.gameObject.SetActive(false);
                listPaintedBallExplodeEffect.Add(paintedBallExplodeEffect);
            }

            paintedBallExplodeEffect.transform.position = pos;
            paintedBallExplodeEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(paintedBallExplodeEffect));
        }



        /// <summary>
        /// Create a FadingCircleController object at given position then scale it up and fade it out.
        /// </summary>
        /// <param name="pos"></param>
        public void CreateFadingCircleController(Vector3 pos)
        {
            //Find in the list
            FadingCircleController fadingCircleController = listFadingCircleController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (fadingCircleController == null)
            {
                //Didn't find one -> create new one
                fadingCircleController = Instantiate(fadingCircleControllerPrefab, pos, Quaternion.identity);
                fadingCircleController.gameObject.SetActive(false);
                listFadingCircleController.Add(fadingCircleController);
            }

            fadingCircleController.gameObject.SetActive(true);
            fadingCircleController.transform.position = pos;
            fadingCircleController.ScaleAndFade();
        }


        /// <summary>
        /// Create a FadingRingController object at given position then scale it up and fade it out.
        /// </summary>
        /// <param name="pos"></param>
        public void CreateFadingRingController(Vector3 pos)
        {
            //Find in the list
            FadingRingController fadingRingController = listFadingRingController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (fadingRingController == null)
            {
                //Didn't find one -> create new one
                fadingRingController = Instantiate(fadingRingControllerPrefab, pos, Quaternion.identity);
                fadingRingController.gameObject.SetActive(false);
                listFadingRingController.Add(fadingRingController);
            }

            fadingRingController.gameObject.SetActive(true);
            fadingRingController.transform.position = pos;
            fadingRingController.ScaleAndFade();
        }
    }
}
