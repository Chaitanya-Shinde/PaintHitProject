using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class FadingCircleController : MonoBehaviour
    {
        private MeshRenderer meshRenderer = null;

        /// <summary>
        /// Scale this object up and fade it out.
        /// </summary>
        public void ScaleAndFade()
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            StartCoroutine(CRScalingAndFading());
        }


        /// <summary>
        /// Scaling this object and fading it out.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRScalingAndFading()
        {
            float scalingTime = 0.5f;
            Vector3 startScale = transform.localScale;
            Vector3 endScale = startScale * 3f;
            Color startColor = meshRenderer.material.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            float t = 0;
            while (t < scalingTime)
            {
                t += Time.deltaTime;
                float factor = t / scalingTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, factor);
                meshRenderer.material.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }

            transform.localScale = Vector3.one;
            meshRenderer.material.color = startColor;
            gameObject.SetActive(false);
        }
    }
}
