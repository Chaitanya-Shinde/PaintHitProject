using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace OnefallGames
{
    public class SceneLoader : MonoBehaviour
    {
        private static string targetScene = string.Empty;

        private void Start()
        {
            ViewManager.Instance.OnShowView(ViewType.LOADING_VIEW);
            StartCoroutine(LoadingScene());
        }

        private IEnumerator LoadingScene()
        {
            int temp = 0;
            float loadingAmount = 0f;
            while (loadingAmount < 0.95f)
            {
                yield return new WaitForSeconds(0.01f);
                loadingAmount += 0.02f;
                ViewManager.Instance.LoadingViewController.SetLoadingAmount(loadingAmount);

                temp++;
                if (temp == 1)
                    ViewManager.Instance.LoadingViewController.SetLoadingText("LOADING.");
                else if (temp == 2)
                    ViewManager.Instance.LoadingViewController.SetLoadingText("LOADING..");
                else
                {
                    ViewManager.Instance.LoadingViewController.SetLoadingText("LOADING...");
                    temp = 0;
                }
            }

            AsyncOperation asyn = SceneManager.LoadSceneAsync(targetScene);
            while (!asyn.isDone)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Set target scene.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void SetTargetScene(string sceneName)
        {
            targetScene = sceneName;

        }
    }
}
