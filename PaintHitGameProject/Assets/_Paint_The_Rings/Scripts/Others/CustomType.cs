using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OnefallGames
{
    #region Ingame enums
    public enum IngameState
    {
        Prepare = 0,
        Playing = 1,
        Revive = 2,
        GameOver = 3,
        CompletedLevel = 4,
    }


    public enum PlayerState
    {
        Prepare = 0,
        Living = 1,
        CompletedLevel = 2,
        Died = 3,
    }

    public enum ItemType
    {
        COIN = 0,
        SHIELD = 1,
        MAGNET = 2,
    }


    public enum PlatformType
    {
        FOREST_PLATFORM = 0,
        DESERT_PLATFORM = 1,
        CEMETERY_PLATFORM = 2,
        SNOWY_PLATFORM = 3,
        SPRING_PLATFORM = 4,
    }


    public enum ObstacleType
    {
        FOREST_OBSTACLE = 0,
        DESERT_OBSTACLE = 1,
        CEMETERY_OBSTACLE = 2,
        SNOWY_OBSTACLE  = 3,
        SPRING_OBSTACLE = 4,
    }


    public enum DayItem
    {
        DAY_1 = 0,
        DAY_2 = 1,
        DAY_3 = 2,
        DAY_4 = 3,
        DAY_5 = 4,
        DAY_6 = 5,
        DAY_7 = 6,
        DAY_8 = 7,
        DAY_9 = 8,
    }

    #endregion


    #region Ads enums
    public enum BannerAdType
    {
        NONE = 0,
        UNITY = 1,
        ADMOB = 2,
    }

    public enum InterstitialAdType
    {
        UNITY = 0,
        ADMOB = 1,
    }
    public enum RewardedAdType
    {
        UNITY = 0,
        ADMOB = 1,
    }
    #endregion


    #region View enums
    public enum ViewType
    {
        HOME_VIEW = 1,
        INGAME_VIEW = 2,
        LOADING_VIEW = 3,
    }
    #endregion


    #region Classes
    [System.Serializable]
    public class LevelConfiguration
    {
        [Header("Level Number Configuration")]
        [SerializeField] private int minLevel = 1;
        public int MinLevel { get { return minLevel; } }
        [SerializeField] private int maxLevel = 5;
        public int MaxLevel { get { return maxLevel; } }


        [Header("Background Music Configuration")]
        [SerializeField] private Color backgroundTopColor = Color.white;
        public Color BackgroundTopColor { get { return backgroundTopColor; } }
        [SerializeField] private Color backgroundBottomColor = Color.white;
        public Color BackgroundBottomColor { get { return backgroundBottomColor; } }
        [SerializeField] private SoundClip backgroundMusicClip = null;
        public SoundClip BackgroundMusicClip { get { return backgroundMusicClip; } }


        [Header("Time To Complete Level Configuration")]
        [SerializeField] [Range(10, 600)] private int minTimeToCompleteLevel = 60;
        public int MinTimeToCompleteLevel { get { return minTimeToCompleteLevel; } }
        [SerializeField] [Range(10, 600)] private int maxTimeToCompleteLevel = 180;
        public int MaxTimeToCompleteLevel { get { return maxTimeToCompleteLevel; } }



        [Header("Rotator Configuration")]
        [SerializeField] [Range(20, 500)] private int minRotatingSpeed = 20;
        public int MinRotatingSpeed { get { return minRotatingSpeed; } }
        [SerializeField] [Range(20, 500)] private int maxRotatingSpeed = 20;
        public int MaxRotatingSpeed { get { return maxRotatingSpeed; } }

        [SerializeField] [Range(0, 360)] private int minRotatingAmount = 50;
        public int MinRotatingAmount { get { return minRotatingAmount; } }
        [SerializeField] [Range(0, 360)] private int maxRotatingAmount = 300;
        public int MaxRotatingAmount { get { return maxRotatingAmount; } }


        [SerializeField] private LerpType[] lerpTypes = null;
        public LerpType[] LerpTypes { get { return lerpTypes; } }


        [Header("Rings Configuration")]
        [SerializeField] private int minRingAmount = 1;
        public int MinRingAmount { get { return minRingAmount; } }
        [SerializeField] private int maxRingAmount = 5;
        public int MaxRingAmount { get { return maxRingAmount; } }

        [SerializeField] [Range(1, 16)] private int minBallAmount = 1;
        public int MinBallAmount { get { return minBallAmount; } }
        [SerializeField] [Range(1, 16)] private int maxBallAmount = 3;
        public int MaxBallAmount { get { return maxBallAmount; } }


        [SerializeField] [Range(0, 16)] private int minPaintedPieceAmount = 0;
        public int MinPaintedPieceAmount { get { return minPaintedPieceAmount; } }
        [SerializeField] [Range(1, 16)] private int maxPaintedPieceAmount = 0;
        public int MaxPaintedPieceAmount { get { return maxPaintedPieceAmount; } }


        [SerializeField] private Color normalRingPieceColor = Color.gray;
        public Color NormalRingPieceColor { get { return normalRingPieceColor; } }
        [SerializeField] private Color[] paintedRingPieceColors = null;
        public Color[] PaintedRingPieceColors { get { return paintedRingPieceColors; } }
    }


    public class RotatorParametersConfiguration
    {
        public int MinRotatingSpeed { private set; get; }
        public void SetMinRotatingSpeed(int minRotatingSpeed)
        {
            MinRotatingSpeed = minRotatingSpeed;
        }


        public int MaxRotatingSpeed { private set; get; }
        public void SetMaxRotatingSpeed(int maxRotatingSpeed)
        {
            MaxRotatingSpeed = maxRotatingSpeed;
        }


        public int MinRotatingAmount { private set; get; }
        public void SetMinRotatingAmount(int minRotatingAmount)
        {
            MinRotatingAmount = minRotatingAmount;
        }


        public int MaxRotatingAmount { private set; get; }
        public void SetMaxRotatingAmount(int maxRotatingAmount)
        {
            MaxRotatingAmount = maxRotatingAmount;
        }

        public LerpType[] LerpTypes { private set; get; }
        public void SetLerpTypes(LerpType[] lerpTypes)
        {
            LerpTypes = lerpTypes;
        }
    }



    public class RingParametersConfiguration
    {
        public int PaintedPieceAmount { private set; get; }
        public void SetPaintedPieceAmount(int paintedPieceAmount)
        {
            PaintedPieceAmount = paintedPieceAmount;
        }


        public int BallAmount { private set; get; }
        public void SetBallAmount(int ballAmount)
        {
            BallAmount = ballAmount;
        }

        public Material PaintedRingPieceMaterial { private set; get; }
        public void SetPaintedRingPieceMaterial(Material paintedRingPieceMaterial)
        {
            PaintedRingPieceMaterial = paintedRingPieceMaterial;
        }
    }



    [System.Serializable]
    public class SoundClip
    {
        [SerializeField] private AudioClip audioClip = null;
        public AudioClip AudioClip { get { return audioClip; } }
    }

    [System.Serializable]
    public class InterstitialAdConfig
    {
        public IngameState GameStateForShowingAd = IngameState.Prepare;
        public int GameStateCountForShowingAd = 3;
        public float ShowAdDelay = 0.2f;
        public List<InterstitialAdType> ListInterstitialAdType = new List<InterstitialAdType>();
    }


    public class PlatformParameterData
    {
        public int ObstacleAmount { private set; get; }
        public void SetObstacleAmount(int obstacleAmount)
        {
            ObstacleAmount = obstacleAmount;
        }

        public int CoinAmount { private set; get; }
        public void SetCoinAmount(int coinAmount)
        {
            CoinAmount = coinAmount;
        }

        public float ShieldFrequency { private set; get; }
        public void SetShieldFrequency(float shieldFrequency)
        {
            ShieldFrequency = shieldFrequency;
        }

        public float MagnetFrequency { private set; get; }
        public void SetMagnetFrequency(float magnetFrequency)
        {
            MagnetFrequency = magnetFrequency;
        }
    }



    public class PlayerLeaderboardData
    {
        public string Name { private set; get; }
        public void SetName(string name)
        {
            Name = name;
        }

        public int Level { private set; get; }
        public void SetLevel(int level)
        {
            Level = level;
        }
    }
    #endregion
}
