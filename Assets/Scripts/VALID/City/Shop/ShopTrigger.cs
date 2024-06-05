using UnityEngine;

namespace BlownAway.City
{
    public class ShopTrigger : BoxTrigger
    {
        [SerializeField] private Shop _shop;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += StartApparition;
        }

        private void StartApparition()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.States.SwitchState(collider.Manager.States.MenuState);

            _shop.CanvasGroup.alpha = 1;
            _shop.Open();
        }
    }
}