using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class IngameManager : MonoBehaviour
    {

        public static IngameManager Instance { private set; get; }
        public static event System.Action<IngameState> IngameStateChanged = delegate { };

        public IngameState IngameState
        {
            get
            {
                return ingameState;
            }
            private set
            {
                if (value != ingameState)
                {
                    ingameState = value;
                    IngameStateChanged(ingameState);
                }
            }
        }

        [Header("Enter a number of level to test. Set back to 0 to disable this feature.")]
        [SerializeField] private int testingLevel = 0;


        [Header("Ingame Configuration")]
        [SerializeField] private int reviveCountDownTime = 5;
        [SerializeField] private float ballShootingSpeed = 100f;

        [Header("Level Configuration")]
        [SerializeField] private List<LevelConfiguration> listLevelConfiguration = new List<LevelConfiguration>();


        [Header("Ingame References")]
        [SerializeField] private RotatorController rotatorController = null;
        [SerializeField] private Material backgroundMaterial = null;
        [SerializeField] private Material normalRingPieceMaterial = null;
        [SerializeField] private Material paintedRingPieceMaterial = null;
        [SerializeField] private ParticleSystem[] completedLevelEffects = null;


        public RotatorController RotatorController { get { return rotatorController; } }
        public int ReviveCountDownTime { get { return reviveCountDownTime; } }
        public bool IsRevived { private set; get; }
        public int CurrentLevel { private set; get; }


        private IngameState ingameState = IngameState.GameOver;
        private List<RingParametersConfiguration> listRingParameters = new List<RingParametersConfiguration>();
        private List<BallController> listBallController = new List<BallController>();
        private LevelConfiguration currentLevelConfig = null;
        private RingController currentRingController = null;
        private SoundClip backgroundMusic = null;
        private float timeToCompleteLevel = 0;
        private int ringParametersIndex = 0;
        private int totalRingAmount = 0;
        private int paintedRingAmount = 0;
        private bool isAbleToShoot = true;


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
            ViewManager.Instance.OnShowView(ViewType.INGAME_VIEW);
            IngameState = IngameState.Prepare;
            ingameState = IngameState.Prepare;


            //Add other actions here
            IsRevived = false;
            completedLevelEffects[0].transform.root.gameObject.SetActive(false);
            ringParametersIndex = 0;
            totalRingAmount = 0;
            paintedRingAmount = 0;
            isAbleToShoot = false;

            //Load level parameters
            CurrentLevel = (testingLevel != 0) ? testingLevel : PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, 1);
            foreach (LevelConfiguration levelConfigs in listLevelConfiguration)
            {
                if (CurrentLevel >= levelConfigs.MinLevel && CurrentLevel < levelConfigs.MaxLevel)
                {
                    //Setup parameters
                    currentLevelConfig = levelConfigs;
                    backgroundMusic = levelConfigs.BackgroundMusicClip;
                    //backgroundMaterial.SetColor("_TopColor", currentLevelConfig.BackgroundTopColor);
                    //backgroundMaterial.SetColor("_BottomColor", currentLevelConfig.BackgroundBottomColor);
                    normalRingPieceMaterial.color = levelConfigs.NormalRingPieceColor;
                    timeToCompleteLevel = Random.Range(currentLevelConfig.MinTimeToCompleteLevel, currentLevelConfig.MaxTimeToCompleteLevel);

                    //Setup rotator
                    RotatorParametersConfiguration rotatorConfiguration = new RotatorParametersConfiguration();
                    rotatorConfiguration.SetMinRotatingSpeed(levelConfigs.MinRotatingSpeed);
                    rotatorConfiguration.SetMaxRotatingSpeed(levelConfigs.MaxRotatingSpeed);
                    rotatorConfiguration.SetMinRotatingAmount(levelConfigs.MinRotatingAmount);
                    rotatorConfiguration.SetMaxRotatingAmount(levelConfigs.MaxRotatingAmount);
                    rotatorConfiguration.SetLerpTypes(levelConfigs.LerpTypes);
                    rotatorController.OnSetup(rotatorConfiguration);

                    //Setup ring parameters
                    int ringAmount = Random.Range(levelConfigs.MinRingAmount, levelConfigs.MaxRingAmount);
                    for(int i = 0; i < ringAmount; i++)
                    {
                        RingParametersConfiguration ringParameters = new RingParametersConfiguration();
                        ringParameters.SetPaintedPieceAmount(Random.Range(levelConfigs.MinPaintedPieceAmount, levelConfigs.MaxPaintedPieceAmount));
                        ringParameters.SetBallAmount(Random.Range(levelConfigs.MinBallAmount, levelConfigs.MaxBallAmount));
                        Material paintedRingPieceMat = new Material(paintedRingPieceMaterial);
                        paintedRingPieceMat.color = levelConfigs.PaintedRingPieceColors[Random.Range(0, levelConfigs.PaintedRingPieceColors.Length)];
                        ringParameters.SetPaintedRingPieceMaterial(paintedRingPieceMat);
                        listRingParameters.Add(ringParameters);
                        totalRingAmount++;
                    }
                    break;
                }
            }


            Invoke(nameof(PlayingGame), 0.15f);
        }


        private void Update()
        {
            if (ingameState == IngameState.Playing)
            {
                if(Input.GetMouseButtonUp(0) && isAbleToShoot)
                {
                    //Shoot ball
                    isAbleToShoot = false;
                    listBallController[0].Shoot(ballShootingSpeed);
                }
            }
        }


        /// <summary>
        /// Actual start the game (call Playing event) and handle other actions.
        /// </summary>
        public void PlayingGame()
        {
            //Fire event
            IngameState = IngameState.Playing;
            ingameState = IngameState.Playing;

            //Add other actions here
            if (IsRevived)
            {
                ResumeBackgroundMusic(0.5f);
                //StartCoroutine(CRDecreasingTimeToCompleteLevel());
                rotatorController.StopRotate(false);
                listBallController[0].transform.position = new Vector3(0, 0, -10);
                listBallController[0].gameObject.SetActive(true);
                isAbleToShoot = true;
            }
            else
            {
                PlayBackgroundMusic(0.5f);
                //StartCoroutine(CRDecreasingTimeToCompleteLevel());
                StartCoroutine(CRCreatingNextRingAndBalls());
            }
        }


        /// <summary>
        /// Call Revive event and handle other actions.
        /// </summary>
        public void Revive()
        {
            //Fire event
            IngameState = IngameState.Revive;
            ingameState = IngameState.Revive;

            //Add other actions here
            PauseBackgroundMusic(0.5f);
        }



        /// <summary>
        /// Call CompleteLevel event and handle other actions.
        /// </summary>
        public void CompleteLevel()
        {
            //Fire event
            IngameState = IngameState.CompletedLevel;
            ingameState = IngameState.CompletedLevel;

            //Add other actions here
            StopBackgroundMusic(0.5f);
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.levelCompleted);
            ServicesManager.Instance.ShareManager.CreateScreenshot();
            completedLevelEffects[0].transform.root.gameObject.SetActive(true);
            foreach (ParticleSystem o in completedLevelEffects)
            {
                o.Play();
            }

            //Save level
            if (testingLevel == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL) + 1);

                //Report level to leaderboard
                string username = PlayerPrefs.GetString(PlayerPrefsKeys.PPK_SAVED_USER_NAME);
                if (!string.IsNullOrEmpty(username))
                {
                    ServicesManager.Instance.LeaderboardManager.SetPlayerLeaderboardData();
                }
            }
        }



        /// <summary>
        /// Call GameOver event and handle other actions.
        /// </summary>
        public void GameOver()
        {
            //Fire event
            IngameState = IngameState.GameOver;
            ingameState = IngameState.GameOver;

            //Add other actions here
            StopBackgroundMusic(0.5f);
            ServicesManager.Instance.ShareManager.CreateScreenshot();
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.levelFailed);
        }


        private void PlayBackgroundMusic(float delay)
        {
            StartCoroutine(CRPlayBGMusic(delay));
        }

        private IEnumerator CRPlayBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            ServicesManager.Instance.SoundManager.PlayMusic(backgroundMusic, 0.5f);
        }

        private void StopBackgroundMusic(float delay)
        {
            StartCoroutine(CRStopBGMusic(delay));
        }

        private IEnumerator CRStopBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            ServicesManager.Instance.SoundManager.StopMusic(0.5f);
        }

        private void PauseBackgroundMusic(float delay)
        {
            StartCoroutine(CRPauseBGMusic(delay));
        }

        private IEnumerator CRPauseBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            ServicesManager.Instance.SoundManager.PauseMusic();
        }

        private void ResumeBackgroundMusic(float delay)
        {
            StartCoroutine(CRResumeBGMusic(delay));
        }

        private IEnumerator CRResumeBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            ServicesManager.Instance.SoundManager.ResumeMusic();
        }


        /// <summary>
        /// Decrease TimeToCompleteLevel.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRDecreasingTimeToCompleteLevel()
        {
            float totalTimeTemp = timeToCompleteLevel;
            float currentTimeTemp = timeToCompleteLevel;
            while (ingameState == IngameState.Playing && currentTimeTemp > 0)
            {
                currentTimeTemp -= 0.02f;
                yield return new WaitForSeconds(0.02f);
                ViewManager.Instance.IngameViewController.PlayingViewController.UpdateTimeProgressSlider(currentTimeTemp, totalTimeTemp);
                if (ingameState != IngameState.Playing)
                {
                    yield break;
                }
            }

            if (ingameState == IngameState.Playing)
            {
                HandleGameOver();
            }
        }


        /// <summary>
        /// Creating a ring at top and balls.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRCreatingNextRingAndBalls()
        {
            //Stop rotation of the rotator
            rotatorController.StopRotate(true);

            //Create a ring at the top and move it down
            currentRingController = PoolManager.Instance.GetRingController();
            currentRingController.transform.position = new Vector3(0, 10, 0);
            currentRingController.gameObject.SetActive(true);
            currentRingController.OnSetup(listRingParameters[ringParametersIndex]);
            yield return new WaitForSeconds(0.25f);
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.ringMovedDown);
            currentRingController.transform.SetParent(rotatorController.transform);

            //Enable rotation of the rotator
            rotatorController.StopRotate(false);

            //Create balls
            for (int i = 0; i < listRingParameters[ringParametersIndex].BallAmount; i++)
            {
                BallController ballController = PoolManager.Instance.GetBallController();
                ballController.transform.position = new Vector3(0, 0, -10) + new Vector3(0, 0, -2f) * i;
                ballController.gameObject.SetActive(true);
                ballController.SetMaterial(listRingParameters[ringParametersIndex].PaintedRingPieceMaterial);
                listBallController.Add(ballController);
            }
            yield return null;
            ViewManager.Instance.IngameViewController.PlayingViewController.UpdateRingCount(paintedRingAmount, totalRingAmount);
            isAbleToShoot = true;
        }


        /// <summary>
        /// Updating the next ball in the list to shoot.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRUpdatingNextBall()
        {
            listBallController.RemoveAt(0);
            yield return null;
            if (listBallController.Count > 0) //There's still balls left in the list to shoot
            {
                foreach (BallController ballController in listBallController)
                {
                    ballController.MoveForward();
                }
                yield return new WaitForSeconds(0.2f);
                isAbleToShoot = true;
            }
            else //No ball to shoot
            {
                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.finishedRing);
                rotatorController.StopRotate(true);
                currentRingController.PaintAllPieces();
                while (!currentRingController.IsPaintedAllPieces)
                {
                    yield return null;
                }


                //Update UI
                paintedRingAmount++;
                ViewManager.Instance.IngameViewController.PlayingViewController.UpdateRingCount(paintedRingAmount, totalRingAmount);


                //Handle next ring
                if (ringParametersIndex == listRingParameters.Count - 1)
                {
                    //Player completed level
                    CompleteLevel();
                }
                else
                {
                    ringParametersIndex++;
                    rotatorController.MoveDown();
                    yield return new WaitForSeconds(0.25f);
                    StartCoroutine(CRCreatingNextRingAndBalls());
                }
            }
        }


        //////////////////////////////////////////////////Publish functions


        /// <summary>
        /// Set the game back to Playing state
        /// </summary>
        public void SetContinueGame()
        {
            IsRevived = true;
            PlayingGame();
        }



        /// <summary>
        /// Handle actions when run out of time or hit a painted ball.
        /// </summary>
        public void HandleGameOver()
        {
            rotatorController.StopRotate(true);
            if (IsRevived || !ServicesManager.Instance.AdManager.IsRewardedVideoAdReady())
            {
                GameOver();
            }
            else
            {
                Revive();
            }
        }



        /// <summary>
        /// Update the next ball in the list to shoot.
        /// </summary>
        public void UpdateNextBall()
        {
            isAbleToShoot = false;
            StartCoroutine(CRUpdatingNextBall());
        }
    }
}
