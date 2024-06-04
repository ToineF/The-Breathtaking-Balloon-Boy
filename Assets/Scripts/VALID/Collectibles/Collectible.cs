
namespace BlownAway.Collectibles
{
    public abstract class Collectible : SphereTrigger
    {
        protected CharacterCollider _owner;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += PickUp;
        }

        private void PickUp()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;
            if (_owner != null) return; // Ensure the collectible is taken only once

            _owner = collider;

            OnPickUp();
        }

        protected virtual void OnPickUp()
        {
            OnDeath();
        }

        protected virtual void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}