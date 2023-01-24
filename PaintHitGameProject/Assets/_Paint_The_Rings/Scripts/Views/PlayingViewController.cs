using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class PlayingViewController : MonoBehaviour
    {
        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private Image timeProgressSlider = null;
        [SerializeField] private Text currentLevelTxt = null;
        [SerializeField] private Text ringCountTxt = null;


        public void OnShow()
        {
            ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0f), 1f);
            currentLevelTxt.text = "Level: " + IngameManager.Instance.CurrentLevel.ToString();
        }

        private void OnDisable()
        {
            topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 150f);
        }



        /// <summary>
        /// Update the fill amount of timeProgressSlider.
        /// </summary>
        /// <param name="currentTime"></param>
        /// <param name="totalTime"></param>
        public void UpdateTimeProgressSlider(float currentTime, float totalTime)
        {
            timeProgressSlider.fillAmount = currentTime / totalTime;
        }



        /// <summary>
        /// Update the amount of painted ring.
        /// </summary>
        /// <param name="paintedRingAmount"></param>
        /// <param name="totalRingAmount"></param>
        public void UpdateRingCount(int paintedRingAmount, int totalRingAmount)
        {
            ringCountTxt.text = paintedRingAmount.ToString() + "/" + totalRingAmount.ToString();
        }
    }
}
