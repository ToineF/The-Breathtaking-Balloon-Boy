using UnityEngine;


namespace Character.States
{
    public abstract class CharacterBaseState
    {
        public abstract void EnterState(CharacterStatesManager manager);
        public abstract void UpdateState(CharacterStatesManager manager);
        public abstract void FixedUpdateState(CharacterStatesManager manager);
        public abstract void LateUpdateState(CharacterStatesManager manager);
        public abstract void ExitState(CharacterStatesManager manager);
    }
}