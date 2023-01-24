using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class PlayerController : MonoBehaviour
    {

        public static PlayerController Instance { private set; get; }
        public static event System.Action<PlayerState> PlayerStateChanged = delegate { };

        public PlayerState PlayerState
        {
            get
            {
                return playerState;
            }

            private set
            {
                if (value != playerState)
                {
                    value = playerState;
                    PlayerStateChanged(playerState);
                }
            }
        }


        //[Header("Player Configuration")]

        //[Header("Player References")]
        //[SerializeField] private MeshCollider meshCollider = null;

        private PlayerState playerState = PlayerState.Died;
        private bool isStopControl = false;


        private void OnEnable()
        {
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
        }
        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }

        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            if (obj == IngameState.Playing)
            {
                PlayerLiving();
            }
        }



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
            //Fire event
            PlayerState = PlayerState.Prepare;
            playerState = PlayerState.Prepare;

            ////Update character
            //CharacterInforController charInfor = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
            //skaterMeshFilter.sharedMesh = charInfor.SkaterMeshFilter.sharedMesh;
            //skaterMeshRenderer.sharedMaterial = charInfor.SkaterMeshRenderer.sharedMaterial;
            //skateboardMeshFilter.sharedMesh = charInfor.SkateboardMeshFilter.sharedMesh;
            //skateboardMeshRenderer.sharedMaterial = charInfor.SkateboardMeshRenderer.sharedMaterial;
            //skaterMeshCollider.sharedMesh = charInfor.SkaterMeshFilter.sharedMesh;
            //skateboardMeshCollider.sharedMesh = charInfor.SkateboardMeshFilter.sharedMesh;
            //meshCollider.sharedMesh = charInfor.SkateboardMeshFilter.sharedMesh;

            //Add another actions here
            isStopControl = true;
        }



        private void Update()
        {
            if (playerState == PlayerState.Living && !isStopControl)
            {
                
            }
        }


        /// <summary>
        /// Call PlayerState.Living even and handle other actions.
        /// </summary>
        private void PlayerLiving()
        {
            //Fire event
            PlayerState = PlayerState.Living;
            playerState = PlayerState.Living;

            //Add another actions here
            if (IngameManager.Instance.IsRevived)
            {
                StartCoroutine(CRHandlingActionsAfterRevived());
            }
            else
            {
                isStopControl = false;
            }
        }


        /// <summary>
        /// Call PlayerState.Died even and handle other actions.
        /// </summary>
        private void PlayerDied()
        {
            //Fire event
            PlayerState = PlayerState.Died;
            playerState = PlayerState.Died;

            //Add another actions here
            ServicesManager.Instance.ShareManager.CreateScreenshot();
            isStopControl = true;
        }



        /// <summary>
        /// Call PlayerState.CompletedLevel even and handle other actions.
        /// </summary>
        private void PlayerCompleteLevel()
        {
            //Fire event
            PlayerState = PlayerState.CompletedLevel;
            playerState = PlayerState.CompletedLevel;

            //Add another actions here
            isStopControl = true;
            ServicesManager.Instance.ShareManager.CreateScreenshot();
        }


        /// <summary>
        /// Handling actions after player revived.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRHandlingActionsAfterRevived()
        {
            //Reset parameters
            transform.position = new Vector3(0f, 0f, transform.position.z);
            transform.localEulerAngles = Vector3.zero;
            yield return new WaitForSeconds(0.75f);
            isStopControl = false;
        }


        //////////////////////////////////////////////////Publish functions


        
    }
}
