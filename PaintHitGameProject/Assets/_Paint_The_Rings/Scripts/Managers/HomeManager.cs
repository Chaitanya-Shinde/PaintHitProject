using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class HomeManager : MonoBehaviour
    {
        public static HomeManager Instance { private set; get; }

        [Header("HomeManager References")]
        [SerializeField] private Transform ring_1_Trans = null;
        [SerializeField] private Transform ring_2_Trans = null;
        [SerializeField] private Transform ring_3_Trans = null;
        [SerializeField] private Transform ring_4_Trans = null;

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



        private void Start()
        {
            Application.targetFrameRate = 60;
            ViewManager.Instance.OnShowView(ViewType.HOME_VIEW);

            //Set default current level
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.PPK_SAVED_LEVEL))
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, 1);
            }
        }

        private void Update()
        {
            ring_1_Trans.localEulerAngles += Vector3.up * 100f * Time.deltaTime;
            ring_3_Trans.localEulerAngles += Vector3.up * 100f * Time.deltaTime;
            ring_2_Trans.localEulerAngles += Vector3.down * 100f * Time.deltaTime;
            ring_4_Trans.localEulerAngles += Vector3.down * 100f * Time.deltaTime;
        }
    }
}