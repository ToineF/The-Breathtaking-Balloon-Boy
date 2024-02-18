
using BlownAway.Character;

namespace BlownAway.Hitbox.Death
{
    public class DeathZoneCollider : BoxTrigger
    {
        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += SetCurrentCheckpoint;
        }

        void SetCurrentCheckpoint()
        {
            if (!_lastOtherCollider.transform.parent.parent.TryGetComponent(out CharacterManager character)) return;

            character.States.SwitchState(character.States.DeathState);
        }
    }
}
