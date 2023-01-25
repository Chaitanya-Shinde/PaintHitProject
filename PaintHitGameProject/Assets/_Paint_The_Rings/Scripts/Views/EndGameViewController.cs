using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class EndGameViewController : MonoBehaviour
    {
        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private RectTransform levelResultPanelTrans = null;
        [SerializeField] private RectTransform bottomBarTrans = null;
        [SerializeField] private RectTransform replayBtnTrans = null;
        [SerializeField] private Text levelCompletedText = null;
        [SerializeField] private Text levelFailedText = null;
        [SerializeField] private Text currentLevelTxt = null;
        public void OnShow()
        {
            //ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0f), 0.75f);
            ViewManager.Instance.ScaleRect(replayBtnTrans, Vector2.zero, Vector2.one, 0.75f);
            //ViewManager.Instance.MoveRect(bottomBarTrans, bottomBarTrans.anchoredPosition, new Vector2(bottomBarTrans.anchoredPosition.x, 150f), 0.75f);
            ViewManager.Instance.ScaleRect(levelResultPanelTrans, Vector2.zero, Vector2.one, 0.75f);

            currentLevelTxt.text = "Next Level: " + PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL).ToString();
            if (IngameManager.Instance.IngameState == IngameState.CompletedLevel)
            {
                levelCompletedText.gameObject.SetActive(true);
                levelFailedText.gameObject.SetActive(false);
            }
            else
            {
                levelCompletedText.gameObject.SetActive(false);
                levelFailedText.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            //topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 100);
            //bottomBarTrans.anchoredPosition = new Vector2(bottomBarTrans.anchoredPosition.x, -200);
            replayBtnTrans.localScale = Vector2.zero;
            levelResultPanelTrans.localScale = Vector2.zero;
        }


        public void ReplayBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Ingame", 0.25f);
        }
        public void ShareBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ServicesManager.Instance.ShareManager.NativeShare();
        }
        public void RateAppBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            Application.OpenURL(ServicesManager.Instance.ShareManager.AppUrl);
        }
        public void HomeBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Home", 0.25f);
        }
    }
}
