using System;
using UnityEngine;

namespace BlownAway.City
{
    public class ShopTrigger : BoxTrigger
    {
        [SerializeField] private Shop _shop;
        [SerializeField] private GameObject _shopIsInZonePreview;

        private CharacterCollider _player;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += EnterZone;
            OnExitTrigger += ExitZone;

        }

        private void EnterZone()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            _shopIsInZonePreview.SetActive(true);
            _player = collider;
        }

        private void ExitZone()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            _shopIsInZonePreview.SetActive(false);
            _player = null;
        }

        private void Update()
        {
            CheckPlayer();
        }

        private void CheckPlayer()
        {
            if (_player == null) return;

            if (!_player.Manager.States.IsInMovableState())
            {
                ExitShop();
            } 
            else
            {
                EnterShop();
            }
        }

        private void ExitShop()
        {
            if (!_player.Manager.Inputs.CancelUIPressed && !_player.Manager.Inputs.PausePressed) return;

            _player.Manager.States.SwitchState(_player.Manager.States.IdleState);

            _shopIsInZonePreview.SetActive(true);

            _shop.Close();
        }

        private void EnterShop()
        {
            if (!_player.Manager.Inputs.ConfirmUIPressed) return;

            _player.Manager.States.SwitchState(_player.Manager.States.MenuState);

            _shopIsInZonePreview.SetActive(false);

            _shop.Open(_player.Manager.Collectibles);
        }
    }
}