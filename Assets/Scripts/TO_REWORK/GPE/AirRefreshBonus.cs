using BlownAway.Player;
using UnityEngine;

namespace BlownAway.GPE
{
    [RequireComponent(typeof(Collider))]
    public class AirRefreshBonus : MonoBehaviour
    {
        [SerializeField] private AudioClip _refreshAir;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out BalloonBoyController character)) return;

            character.RefreshAir();

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClip(_refreshAir);

            Destroy(gameObject);
        }
    }
}