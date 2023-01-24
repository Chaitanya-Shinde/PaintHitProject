using UnityEngine;

namespace OnefallGames
{
    public class RingPieceController : MonoBehaviour
    {
        public bool IsPainted { private set; get; }

        private MeshRenderer meshRenderer = null;
        private Material paintedMatrial = null;


        /// <summary>
        /// Setup this ring piece.
        /// </summary>
        /// <param name="paintedMat"></param>
        public void OnSetup(Material paintedMat)
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            IsPainted = false;
            paintedMatrial = paintedMat;
        }


        /// <summary>
        /// Update this ring piece to painted state.
        /// </summary>
        /// <param name="material"></param>
        public void UpdateToPaintedState()
        {
            IsPainted = true;
            meshRenderer.material = paintedMatrial;
        }
    }
}
