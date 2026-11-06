using Photon.Pun;
using UnityEngine;

namespace SkillHub.TaskPhoton
{
    public class PlayerSetup : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;

    void Start()
    {
        if (photonView.IsMine)
        {
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(true);
            }
        }
        else
        {
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(false);
            }
        }
    }
}
}
