using UnityEngine;
using DG.Tweening;

namespace BlownAway.GPE
{
    public class AirRefreshBonus : SphereTrigger
    {
        public AirRefreshManager Manager { get; set; }

        [Header("Feedbacks")]
        [SerializeField] private float _scaleDuration;
        [SerializeField] private AudioClip _refreshAir;


        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += RefreshAir;
        }

        private void Start()
        {
            float localScale = transform.localScale.magnitude;
            transform.localScale = Vector3.zero;
            transform.DOScale(localScale, _scaleDuration);
        }

        private void RefreshAir()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.AirManager.RefreshAir();
            collider.Manager.MovementManager.StartDeriveTimer(collider.Manager);
            collider.Manager.MovementManager.RefreshDashes(collider.Manager);


            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClip(_refreshAir);

            Manager.StartWaitForRespawn();
            Destroy(gameObject);
        }
    }
}