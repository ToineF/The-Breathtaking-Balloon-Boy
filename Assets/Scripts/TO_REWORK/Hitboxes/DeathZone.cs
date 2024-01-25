using UnityEngine;
using UnityEngine.SceneManagement;
using BlownAway.Player;

namespace BlownAway.Hitbox
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<CharacterControllerTest>()) return;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
