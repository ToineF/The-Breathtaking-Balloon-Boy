using UnityEngine;

namespace BlownAway.GPE
{
    public class AirRefreshBonus : SphereTrigger
    {
        [SerializeField] private AudioClip _refreshAir;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += RefreshAir;
        }

        private void RefreshAir()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.AirManager.RefreshAir();

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClip(_refreshAir);

            Destroy(gameObject);
        }
    }
}