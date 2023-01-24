using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class HomeViewController : MonoBehaviour
    {
        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private RectTransform gameNameTrans = null;
        [SerializeField] private RectTransform playBtnTrans = null;
        [SerializeField] private RectTransform bottomBarTrans = null;
        [SerializeField] private RectTransform soundButtonsTrans = null;
        [SerializeField] private RectTransform musicButtonsTrans = null;
        [SerializeField] private RectTransform shareBtnTrans = null;
        [SerializeField] private RectTransform removeAdsBtnTrans = null;
        [SerializeField] private Text currentLevelTxt = null;
        [SerializeField] private GameObject soundOnBtn = null;
        [SerializeField] private GameObject soundOffBtn = null;
        [SerializeField] private GameObject musicOnBtn = null;
        [SerializeField] private GameObject musicOffBtn = null;
        [SerializeField] private LeaderboardViewController leaderboardViewController = null;

        private int settingButtonTurn = 1;
        public void OnShow()
        {
            ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0f), 0.75f);
            ViewManager.Instance.MoveRect(bottomBarTrans, bottomBarTrans.anchoredPosition, new Vector2(bottomBarTrans.anchoredPosition.x, 150f), 0.75f);
            ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.zero, Vector2.one, 0.75f);
            ViewManager.Instance.ScaleRect(playBtnTrans, Vector2.zero, Vector2.one, 0.75f);

            leaderboardViewController.gameObject.SetActive(false);
            currentLevelTxt.text = "Level: " + PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, 1).ToString();

            //Handle sound buttons
            if (ServicesManager.Instance.SoundManager.IsSoundOff())
            {
                soundOnBtn.gameObject.SetActive(false);
                soundOffBtn.gameObject.SetActive(true);
            }
            else
            {
                soundOnBtn.gameObject.SetActive(true);
                soundOffBtn.gameObject.SetActive(false);
            }


            //Handle music buttons
            if (ServicesManager.Instance.SoundManager.IsMusicOff())
            {
                musicOnBtn.gameObject.SetActive(false);
                musicOffBtn.gameObject.SetActive(true);
            }
            else
            {
                musicOnBtn.gameObject.SetActive(true);
                musicOffBtn.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 100f);
            playBtnTrans.localScale = Vector2.zero;
            bottomBarTrans.anchoredPosition = new Vector2(bottomBarTrans.anchoredPosition.x, -200f);
        }


        /// <summary>
        /// Handle the Home view when Dailyreward view or Environment view closes.
        /// </summary>
        public void OnSubViewClose()
        {
            ViewManager.Instance.MoveRect(bottomBarTrans, bottomBarTrans.anchoredPosition, new Vector2(bottomBarTrans.anchoredPosition.x, 150f), 0.75f);
            ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.zero, Vector2.one, 0.75f);
            ViewManager.Instance.ScaleRect(playBtnTrans, Vector2.zero, Vector2.one, 0.75f);
        }




        public void PlayBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Ingame", 0.25f);
        }


        public void LeaderboardBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            StartCoroutine(CRHandleLeaderboardBtn());
        }
        private IEnumerator CRHandleLeaderboardBtn()
        {
            if (settingButtonTurn == -1)
                SettingBtn();
            ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.one, Vector2.zero, 0.5f);
            ViewManager.Instance.ScaleRect(playBtnTrans, Vector2.one, Vector2.zero, 0.5f);
            ViewManager.Instance.MoveRect(bottomBarTrans, bottomBarTrans.anchoredPosition, new Vector2(bottomBarTrans.anchoredPosition.x, -200f), 0.5f);
            yield return new WaitForSeconds(0.75f);
            leaderboardViewController.gameObject.SetActive(true);
            leaderboardViewController.OnShow();
        }

        public void RateAppBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            Application.OpenURL(ServicesManager.Instance.ShareManager.AppUrl);
        }


        public void SettingBtn()
        {
            settingButtonTurn *= -1;
            StartCoroutine(CRHandleSettingBtn());
        }
        private IEnumerator CRHandleSettingBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            if (settingButtonTurn == -1)
            {
                ViewManager.Instance.MoveRect(soundButtonsTrans, soundButtonsTrans.anchoredPosition, new Vector2(0, soundButtonsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(shareBtnTrans, shareBtnTrans.anchoredPosition, new Vector2(0, shareBtnTrans.anchoredPosition.y), 0.5f);
                yield return new WaitForSeconds(0.08f);

                ViewManager.Instance.MoveRect(musicButtonsTrans, musicButtonsTrans.anchoredPosition, new Vector2(0, musicButtonsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(removeAdsBtnTrans, removeAdsBtnTrans.anchoredPosition, new Vector2(0, removeAdsBtnTrans.anchoredPosition.y), 0.5f);
            }
            else
            {
                ViewManager.Instance.MoveRect(soundButtonsTrans, soundButtonsTrans.anchoredPosition, new Vector2(-150, soundButtonsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(shareBtnTrans, shareBtnTrans.anchoredPosition, new Vector2(150, shareBtnTrans.anchoredPosition.y), 0.5f);
                yield return new WaitForSeconds(0.08f);

                ViewManager.Instance.MoveRect(musicButtonsTrans, musicButtonsTrans.anchoredPosition, new Vector2(-150, musicButtonsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(removeAdsBtnTrans, removeAdsBtnTrans.anchoredPosition, new Vector2(150, removeAdsBtnTrans.anchoredPosition.y), 0.5f);
            }
        }


        public void ToggleSound()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ServicesManager.Instance.SoundManager.ToggleSound();
            if (ServicesManager.Instance.SoundManager.IsSoundOff())
            {
                soundOnBtn.gameObject.SetActive(false);
                soundOffBtn.gameObject.SetActive(true);
            }
            else
            {
                soundOnBtn.gameObject.SetActive(true);
                soundOffBtn.gameObject.SetActive(false);
            }
        }

        public void ToggleMusic()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ServicesManager.Instance.SoundManager.ToggleMusic();
            if (ServicesManager.Instance.SoundManager.IsMusicOff())
            {
                musicOffBtn.gameObject.SetActive(true);
                musicOnBtn.gameObject.SetActive(false);
            }
            else
            {
                musicOffBtn.gameObject.SetActive(false);
                musicOnBtn.gameObject.SetActive(true);
            }
        }

        public void ShareBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ServicesManager.Instance.ShareManager.NativeShare();
        }

        public void RemoveAdsBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
        }
    }
}
