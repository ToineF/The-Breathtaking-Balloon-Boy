using UnityEngine;
using Cinemachine;
using BlownAway.Player;

namespace BlownAway.Camera
{
    public class NewBehaviourScript : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponent<CharacterControllerTest>() == null) return;

            _virtualCamera.gameObject.SetActive(false);
            _virtualCamera.gameObject.SetActive(true);
        }
    }
}
