using UnityEngine;
using UnityEngine.Events;
using AntoineFoucault.Utilities;

namespace BlownAway.Hitbox
{
    public abstract class HitboxTrigger : MonoBehaviour
    {
        [SerializeField] protected bool DestroyOnEnter;
        [SerializeField] protected LayerMask PlayerLayer;
        [SerializeField] protected UnityEvent OnHitboxEnter;
        [SerializeField] protected UnityEvent OnHitboxStay;
        [SerializeField] protected UnityEvent OnHitboxExit;

        virtual protected void OnTriggerEnter(Collider other)
        {
            if (!LayerExtensions.IsInLayerMask(other.gameObject.layer, PlayerLayer)) return;

            OnHitboxEnter?.Invoke();
            if (DestroyOnEnter) Destroy(gameObject);
        }

        virtual protected void OnTriggerStay(Collider other)
        {
            if (!LayerExtensions.IsInLayerMask(other.gameObject.layer, PlayerLayer)) return;

            OnHitboxStay?.Invoke();
        }

        virtual protected void OnTriggerExit(Collider other)
        {
            if (!LayerExtensions.IsInLayerMask(other.gameObject.layer, PlayerLayer)) return;

            OnHitboxExit?.Invoke();
        }
    }
}