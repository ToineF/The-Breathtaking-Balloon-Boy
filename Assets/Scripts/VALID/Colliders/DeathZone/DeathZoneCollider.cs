
using BlownAway.Character;

namespace BlownAway.Hitbox.Death
{
    public class DeathZoneCollider : BoxTrigger
    {
        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += KillPlayer;
        }

        void KillPlayer()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            CharacterManager character = collider.Manager;

            character.States.SwitchState(character.States.DeathState);
            //character.Transition.SetTransition(() => character.States.SwitchState(character.States.DeathState));
        }
    }
}
